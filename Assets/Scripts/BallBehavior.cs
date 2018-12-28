using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    public float Speed;
	public CurrentPlayer CurrentPlayer;
	public CurrentPlayer IsThrownBy;
	public Animator Animator;

	public GameObject LiftEffect;
	public GameObject QuickEffect;
    public GameObject WallHitEffect;
    public GameObject GoalExplosionEffect;

    private GameObject _linkedPlayer;
    private GameObject _gameManager;
    private GameObject _camera;
    private float _spaceYFromPlayer;
	private float _spaceXFromPlayer;
	private int _catchCount;
	private Direction _liftDirection;
	private float _gravity;
	private bool _gravityIsSet;
	private int _nbCol;
	private float _liftEffectDelay;
	private float _quickEffectDelay;
	private bool _isQuickThrow;

	void Start ()
	{
	    // Initial Velocity
	    //GetComponent<Rigidbody2D>().velocity = Vector2.up * Speed;
        _gameManager = GameObject.Find("$GameManager");
        _camera = GameObject.Find("Camera");
		_catchCount = -1; //-1 because the first collision counts when serving
	    IsThrownBy = CurrentPlayer.None;
		_liftDirection = Direction.Standby;
		_gravity = 10.0f;
		_gravityIsSet = false;
		_nbCol = 0;
		_liftEffectDelay = 0.1f;
		_quickEffectDelay = 0.05f;
		_isQuickThrow = false;
	}

    void Update()
    {
		if (CurrentPlayer != CurrentPlayer.None)
		{
			Animator.SetBool ("IsRotating", false);
			if (_linkedPlayer == null)
				_linkedPlayer = GetLinkedPlayer ();
			float spaceYFromPlayer = _linkedPlayer.GetComponent<PlayerBehavior> ().Player == CurrentPlayer.PlayerOne 
				? -1.6385f
				: 1.5269f;
			float spaceXFromPlayer = _linkedPlayer.GetComponent<PlayerBehavior> ().Player == CurrentPlayer.PlayerOne 
				? 0.111f
				: -0.083f;
			transform.position = new Vector3 (_linkedPlayer.transform.position.x + spaceXFromPlayer, spaceYFromPlayer);
		}
		else
			Animator.SetBool ("IsRotating", true);

		if (_liftDirection != Direction.Standby && !_gravityIsSet)
		{
			AddGravity (1.0f);
		}
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
        if (col.gameObject.tag == "Player")
        {
			_nbCol = 0;
            _linkedPlayer = col.gameObject;
			CurrentPlayer = _linkedPlayer.GetComponent<PlayerBehavior> ().Player;
			_linkedPlayer.GetComponent<PlayerBehavior>().CatchTheDisc();
			if (++_catchCount % 2 == 0 && _catchCount != 0) // "_catchcount != 0" because it starts at -1
				Speed += 0.25f;
            IsThrownBy = CurrentPlayer.None;
			_liftDirection = Direction.Standby;
			Physics2D.gravity = new Vector2 (0.0f,0.0f);
			_gravityIsSet = false;
			_isQuickThrow = true;
			Invoke ("DisableQuickThrow", 0.35f);
        }
        else if (col.gameObject.tag == "Goal")
        {
            var yGoalEffect = 1.833f;
            if (transform.position.y < 0)
                yGoalEffect = -yGoalEffect;
            var tmpGoalEffect = Instantiate(GoalExplosionEffect, new Vector3(transform.position.x, yGoalEffect, 0.0f), transform.rotation);
            if (transform.position.y > 0)
                tmpGoalEffect.GetComponent<SpriteRenderer>().flipY = true;

            _camera.GetComponent<CameraBehavior>().GoalHit();
			col.gameObject.GetComponent<GoalBehavior> ().GoalHit ();
            _gameManager.GetComponent<GameManagerBehavior>().NewSet(
                col.gameObject.GetComponent<GoalBehavior>().Player);
            Destroy(gameObject);
        }
		else if (col.gameObject.tag == "Wall")
		{
			++_nbCol;
			if (_liftDirection != Direction.Standby && _nbCol <= 1)
				AddGravity (1.5f);
            _camera.GetComponent<CameraBehavior>().WallHit();
			col.gameObject.GetComponent<WallBehavior>().WallHit();

		    var xWallEffect = 1.472f;
		    if (transform.position.x < 0)
		        xWallEffect = -xWallEffect;
            var tmpWallHitEffect = Instantiate(WallHitEffect, new Vector3(xWallEffect, transform.position.y, 0.0f), transform.rotation);
		    if (transform.position.x < 0)
		        tmpWallHitEffect.GetComponent<SpriteRenderer>().flipX = true;
		    if (GetComponent<Rigidbody2D>().velocity.y > 0)
		        tmpWallHitEffect.GetComponent<SpriteRenderer>().flipY = true;
        }

		if (_nbCol >= 10)
		{
			_gameManager.GetComponent<GameManagerBehavior>().NewSet(IsThrownBy);
			Destroy(gameObject);
		}
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
		IsThrownBy = throwingPlayer;
		CurrentPlayer = CurrentPlayer.None;
		float speedQuarter = (Speed + addedPower) / 4;
		float customSpeed = (Speed + addedPower) - (Mathf.Abs(direction.x) * speedQuarter);
		if (isLifted)
		{
			_isQuickThrow = false;
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
