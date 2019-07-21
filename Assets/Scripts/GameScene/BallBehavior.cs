using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
	public int MapId;

    public float Speed;
	public int CatchCount;
	public int NbCol;
	public CurrentPlayer CurrentPlayer;
	public CurrentPlayer IsThrownBy;
	public CurrentPlayer LastThrownBy;
	public Animator Animator;
	public bool IsNextBall;
	public bool HasHitPlayer;

	public GameObject LiftEffect;
	public GameObject QuickEffect;
    public GameObject WallHitEffect;
    public GameObject GoalExplosionEffect;

	public delegate bool OnWallCollisionDelegate();
	public OnWallCollisionDelegate onWallCollisionDelegate;

	public delegate bool OnPlayerCollisionDelegate();
	public OnPlayerCollisionDelegate onPlayerCollisionDelegate;

    private GameObject _linkedPlayer;
    private GameObject _gameManager;
    private GameObject _camera;
    private float _spaceYFromPlayer;
	private float _spaceXFromPlayer;
	private Direction _liftDirection;
	private float _gravity;
	private bool _gravityIsSet;
	private bool _isRandomBounce;
	private float _liftEffectDelay;
	private float _quickEffectDelay;
	private bool _isQuickThrow;
	private bool _hasHitGoal;
	private int _currentMap;

	void Start ()
	{
	    // Initial Velocity
	    //GetComponent<Rigidbody2D>().velocity = Vector2.up * Speed;
        _gameManager = GameObject.Find("$GameManager");
        _camera = GameObject.Find("Camera");
		CatchCount = -1; //-1 because the first collision counts when serving
		if (this.tag != "DiscShadow")
	    	IsThrownBy = CurrentPlayer.None;
		LastThrownBy = IsThrownBy;
		_liftDirection = Direction.Standby;
		_gravity = 10.0f;
		_gravityIsSet = false;
		_isRandomBounce = PlayerPrefs.GetInt ("Bounce") == Bounce.Random.GetHashCode () ? true : false ;
		NbCol = 0;
		_liftEffectDelay = 0.1f;
		_quickEffectDelay = 0.05f;
		_isQuickThrow = false;
		_hasHitGoal = false;
		HasHitPlayer = false;
		onWallCollisionDelegate = null;
		onPlayerCollisionDelegate = null;
		_currentMap = PlayerPrefs.GetInt ("SelectedMap");
	}

    void Update()
    {
		if (CurrentPlayer != CurrentPlayer.None) {
			Animator.SetBool ("IsRotating", false);
			if (!IsNextBall) {
				PlaceBallFromPlayer ();
			} else {
				transform.position = new Vector3 (-3.0f, 0.0f, 0.0f);
			}
		} else {
			Animator.SetBool ("IsRotating", true);
			HasHitPlayer = false;
		}

		if (_liftDirection != Direction.Standby && !_gravityIsSet)
		{
			AddGravity (1.0f);
		}
    }

	public void PlaceBallFromPlayer()
	{
		_linkedPlayer = GetLinkedPlayer ();
		float spaceYFromPlayer = _linkedPlayer.GetComponent<PlayerBehavior> ().Player == CurrentPlayer.PlayerOne 
			? -1.6385f
			: 1.5269f;
		float spaceXFromPlayer = _linkedPlayer.GetComponent<PlayerBehavior> ().Player == CurrentPlayer.PlayerOne 
			? 0.111f
			: -0.083f;
		transform.position = new Vector3 (_linkedPlayer.transform.position.x + spaceXFromPlayer, spaceYFromPlayer);
	}

	private void AddGravity(float multiplier)
	{
		if (_liftDirection == Direction.Left)
			Physics2D.gravity += new Vector2 (-_gravity * multiplier,0.0f);
		else
			Physics2D.gravity += new Vector2 (_gravity * multiplier,0.0f);
		_gravityIsSet = true;
	}

    void OnCollisionEnter2D(Collision2D col)
    {
		if (gameObject.tag == "DiscShadow")
			if ((col.gameObject.tag == "Player" && col.gameObject.GetComponent<PlayerBehavior>().Player != this.IsThrownBy)
			|| col.gameObject.tag == "Goal"
			|| col.gameObject.tag == "FrozenWall"
			|| col.gameObject.tag == "TrainingWall")
				Destroy (gameObject);

		if (col.gameObject.tag == "Player" && gameObject.tag != "DiscShadow")
        {
			if (_hasHitGoal || HasHitPlayer)
				return;
			HasHitPlayer = true;
			onWallCollisionDelegate = null;
			if (onPlayerCollisionDelegate != null)
			{
				if (onPlayerCollisionDelegate () == false)
				{
					_linkedPlayer = col.gameObject;
					_linkedPlayer.GetComponent<PlayerBehavior> ().TriggerCatch ();
					return;
				}
			}
			if (_currentMap == 5) {
				GameObject.Find ("MiddleWall01").GetComponent<MiddleWallBehavior> ().Reset ();
			}
			NbCol = 0;
			LastThrownBy = IsThrownBy;
            _linkedPlayer = col.gameObject;
			CurrentPlayer = _linkedPlayer.GetComponent<PlayerBehavior> ().Player;
			if (_linkedPlayer.GetComponent<PlayerBehavior>().HasTheDisc == false) {
				_linkedPlayer.GetComponent<PlayerBehavior> ().Ball = this.gameObject;
				_linkedPlayer.GetComponent<PlayerBehavior> ().CatchTheDisc ();
			}
			else {
				IsNextBall = true;
				_linkedPlayer.GetComponent<PlayerBehavior> ().NextBalls.Add(this.gameObject);
			}
			if (++CatchCount % 2 == 0 && CatchCount != 0 && Speed < 6.0f) // "CatchCount != 0" because it starts at -1
				Speed += 0.25f;
            IsThrownBy = CurrentPlayer.None;
			_liftDirection = Direction.Standby;
			Physics2D.gravity = new Vector2 (0.0f,0.0f);
			_gravityIsSet = false;
			_isQuickThrow = true;
			Invoke ("DisableQuickThrow", 0.3f);
			Invoke ("PlayerGetBallLastCheck", 0.3f);
        }
		else if (col.gameObject.tag == "Goal" && gameObject.tag != "DiscShadow")
        {
			onWallCollisionDelegate = null;
			onPlayerCollisionDelegate = null;
			if (_hasHitGoal || IsThrownBy == CurrentPlayer.None || HasHitPlayer)
				return;
			_hasHitGoal = true;
			Physics2D.gravity = new Vector2 (0.0f,0.0f);
			var yGoalEffect = MapsData.Maps[MapId].GoalCollisionY;
            if (transform.position.y < 0)
                yGoalEffect = -yGoalEffect;
            var tmpGoalEffect = Instantiate(GoalExplosionEffect, new Vector3(transform.position.x, yGoalEffect, 0.0f), transform.rotation);
            if (transform.position.y > 0)
                tmpGoalEffect.GetComponent<SpriteRenderer>().flipY = true;

			if (!MoreThanOneBall() && _camera != null)
				_camera.GetComponent<CameraBehavior>().GoalHit(transform.position.y);
			/*if (transform.position.x >= col.gameObject.transform.position.x - (col.gameObject.GetComponent<BoxCollider2D> ().size.x / 2) &&
			    transform.position.x <= col.gameObject.transform.position.x + (col.gameObject.GetComponent<BoxCollider2D> ().size.x / 2))
			{*/
				col.gameObject.GetComponent<GoalBehavior> ().GoalHit ();
			//}
			var looserGameObject = GameObject.Find (col.gameObject.GetComponent<GoalBehavior>().Player.ToString ());
			if (looserGameObject != null)
			{
				looserGameObject.GetComponent<PlayerBehavior> ().Eject(transform.position);
				CollideElement (looserGameObject.transform.position);
			}
			if (_gameManager != null)
				_gameManager.GetComponent<GameManagerBehavior>().NewBall(col.gameObject.GetComponent<GoalBehavior>().Player, col.gameObject.GetComponent<GoalBehavior>().Points, MoreThanOneBall());
            Destroy(gameObject);
        }
		else if (col.gameObject.tag == "TrainingWall" && gameObject.tag != "DiscShadow")
		{
			NbCol = 0;
			var tmpWallHitEffect = Instantiate(WallHitEffect, new Vector3(gameObject.transform.position.x, col.transform.position.y - 0.39f, 0.0f), transform.rotation);
			tmpWallHitEffect.transform.Rotate (0.0f, 0.0f, 90.0f);
			_camera.GetComponent<CameraBehavior>().WallHit();
			IsThrownBy = CurrentPlayer.PlayerTwo;
			if (_isRandomBounce)
				GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, -2.0f)) * Speed;
		}
		else if (col.gameObject.tag == "MiddleWall")
		{
			Vector2 point;
			if (col.contacts.Length > 0)
				point = col.contacts [0].point;
			else
				point = this.transform.position;
			CollideElement (point);
		}
		else if (col.gameObject.tag == "Wall")
		{
			if (onWallCollisionDelegate != null)
			{
				onWallCollisionDelegate ();
			}
			++NbCol;
			if (_liftDirection != Direction.Standby && NbCol <= 1)
				AddGravity (1.5f);
            _camera.GetComponent<CameraBehavior>().WallHit();
			col.gameObject.GetComponent<WallBehavior>().WallHit();

			var xWallEffect = MapsData.Maps[MapId].WallCollisionX;
		    if (transform.position.x < 0)
		        xWallEffect = -xWallEffect;
            var tmpWallHitEffect = Instantiate(WallHitEffect, new Vector3(xWallEffect, transform.position.y, 0.0f), transform.rotation);
		    if (transform.position.x < 0)
		        tmpWallHitEffect.GetComponent<SpriteRenderer>().flipX = true;
		    if (GetComponent<Rigidbody2D>().velocity.y > 0)
		        tmpWallHitEffect.GetComponent<SpriteRenderer>().flipY = true;
        }
		else if (col.gameObject.tag == "FrozenWall" && gameObject.tag != "DiscShadow")
		{
			_camera.GetComponent<CameraBehavior>().WallHit();
			col.gameObject.GetComponent<GoalBehavior> ().GoalHit ();
			col.gameObject.GetComponent<GoalBehavior> ().Unfreeze ();
		}
		else if (col.gameObject.tag == "Target")
		{
			Vector2 point;
			if (col.contacts.Length > 0)
				point = col.contacts [0].point;
			else
				point = this.transform.position;
			CollideElement (point, true);
			_gameManager.GetComponent<GameManagerBehavior> ().NewBallChallenge ();
			Destroy(gameObject);
		}

		if (NbCol >= 10)
		{
			_gameManager.GetComponent<GameManagerBehavior>().NewBall(IsThrownBy, 2, MoreThanOneBall());
			Destroy(gameObject);
		}
    }

	private void CollideElement(Vector2 point, bool goalExplosion = false)
	{
		float effectRotation = 0.0f;
		float offset = 0.35f;
		float effectX = -offset;
		float effectY = 0.0f;
		if (point.x < this.transform.position.x - 0.1f) {
			effectRotation = 180.0f;
			effectX = offset;
			effectY = 0.0f;
		} else if (point.y < this.transform.position.y - 0.1f) {
			effectRotation = 270.0f;
			effectX = 0.0f;
			effectY = offset;
		} else if (point.y > this.transform.position.y + 0.1f) {
			effectRotation = 90.0f;
			effectX = 0.0f;
			effectY = -offset;
		}
		GameObject effect = WallHitEffect;
		if (goalExplosion) {
			effect = GoalExplosionEffect;
			effectRotation = 180;
			effectY = -0.7f;
		}
		var tmpWallHitEffect = Instantiate(effect, new Vector3(point.x + effectX, point.y + effectY, 0.0f), transform.rotation);
		tmpWallHitEffect.transform.Rotate (0.0f, 0.0f, effectRotation);
	}

	private void PlayerGetBallLastCheck()
	{
		if (CurrentPlayer != CurrentPlayer.None) {
			if (_linkedPlayer.GetComponent<PlayerBehavior> ().HasTheDisc == false)
				_linkedPlayer.GetComponent<PlayerBehavior> ().CatchTheDisc ();
		}
	}

	private bool MoreThanOneBall()
	{
		var ballTab = GameObject.FindGameObjectsWithTag ("Disc");
		return ballTab.Length > 1;
	}

	private void DisableQuickThrow()
	{
		_isQuickThrow = false;
	}

    private GameObject GetLinkedPlayer()
    {
		if (CurrentPlayer == CurrentPlayer.PlayerOne)
            return GameObject.Find("PlayerOne");
		else if (CurrentPlayer == CurrentPlayer.PlayerTwo)
            return GameObject.Find("PlayerTwo");
        return null;
    }

	public void Throw(Vector2 direction, CurrentPlayer throwingPlayer, float addedPower, bool isLifted)
    {
		SetGravityScaleFromPower (addedPower);
		IsThrownBy = throwingPlayer;

		CurrentPlayer = CurrentPlayer.None;
		float speedQuarter = (Speed + addedPower) / 4;
		float customSpeed = (Speed + addedPower) - (Mathf.Abs(direction.x) * speedQuarter);
		if (isLifted)
		{
			_isQuickThrow = false;
			if (PlayerPrefs.GetInt ("Opponent") == Opponent.Catch.GetHashCode ())
				_liftDirection = GameObject.Find("Launcher").GetComponent<LauncherBehavior> ().LiftDirection;
			else
				_liftDirection = _linkedPlayer.GetComponent<PlayerBehavior> ().LiftDirection;
			customSpeed = customSpeed * 0.8f;
			_gravityIsSet = true;
			Invoke ("CanSetGravity", 0.15f);
			Invoke ("InstantiateLiftEffect", _liftEffectDelay);
		}
		if (_isQuickThrow)
		{
			customSpeed = customSpeed * 1.2f;
			Invoke ("InstantiateQuickEffect", _quickEffectDelay);
		}
        GetComponent<Rigidbody2D>().velocity = direction * customSpeed;
    }

	public void SetGravityScaleFromPower(float power)
	{
		if (power <= 0.5f)
			GetComponent<Rigidbody2D> ().gravityScale = 0.7f;
		else if (power <= 1.0f)
			GetComponent<Rigidbody2D> ().gravityScale = 0.85f;
		else
			GetComponent<Rigidbody2D> ().gravityScale = 1f;
	}

	private void CanSetGravity()
	{
		_gravityIsSet = false;
	}

	private void InstantiateLiftEffect()
	{
		if (IsThrownBy == CurrentPlayer.None)
			return;
		Instantiate (LiftEffect, transform.position, transform.rotation);
		Invoke ("InstantiateLiftEffect", _liftEffectDelay);
	}

	private void InstantiateQuickEffect()
	{
		if (IsThrownBy == CurrentPlayer.None)
			return;
		var tmpQuickEffect = Instantiate (QuickEffect, transform.position, transform.rotation);
		tmpQuickEffect.GetComponent<SpriteRenderer> ().sprite = gameObject.GetComponent<SpriteRenderer> ().sprite;
		Invoke ("InstantiateQuickEffect", _quickEffectDelay);
	}
}
