using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public CurrentPlayer Player;
    public Direction Direction;
	public Direction LiftDirection;
	public Color SuperColor;

    public bool IsDashing;
    public bool HasTheDisc;
	public bool IsControlledByAI;
	public bool IsCastingSP;
	public bool IsDoingSP;
	public bool IsEngaging;
	public ControlledAction ControlledAction;

    public float WalkDistance;
    public float DashDistance;
    public float Power;
	public int SPMaxCooldown;
	public int SPCooldown;
	public int ConsecutiveHit;
	public int CharacterNumber;

	public GameObject Ball;
	public List<GameObject> NextBalls;
    public Animator Animator;
	public BoxCollider2D BoxCollider;
	public SpriteRenderer CurrentSprite;
	public Sprite[] AngleSprites;
	public Sprite DashSprite;
	public Sprite SPSprite;
    public GameObject DashEffect;
    public GameObject DashEffectParticles;
	public GameObject CatchEffect;
	public GameObject CastSPEffect;
	public GameObject ConsecutiveHitEffect;

    private Quaternion _initialRotation;
	private Vector3 _initialPosition;
    private Vector2 _directionalVector;
    private float _throwAngle;
    private GameObject _gameManager;
	private GameObject _mimicShadow;
    private Vector3 _dashingStart;
    private Vector3 _dashingEnd;
    private Direction _dashingDirection;
    private float _dashCooldown;
	private float _castSPCooldown;
    private bool _canDash;
	private bool _isAgainstWall;
	private bool _isAgainstAI;

    void Start ()
	{
		HasTheDisc = false;
		_initialPosition = transform.position;
		_initialRotation = transform.rotation;
        if (Player == CurrentPlayer.PlayerOne)
            _directionalVector = Vector2.up;
	    else
        {
            SetPlayerTwoAngleSprites();
            _directionalVector = Vector2.down;
        }
		LiftDirection = Direction.Standby;
	    _throwAngle = 0;
        _gameManager = GameObject.Find("$GameManager");
	    //Ball = GetBall();
        _dashingDirection = Direction.Standby;
	    _dashCooldown = 0.75f;
		_castSPCooldown = 1.0f;
	    _canDash = true;
		SPCooldown = SPMaxCooldown;
		if (PlayerPrefs.GetInt ("Opponent") == Opponent.Wall.GetHashCode ())
			_isAgainstWall = true;
		else if (PlayerPrefs.GetInt ("Opponent") == Opponent.AI.GetHashCode ())
			_isAgainstAI = true;
		ConsecutiveHit = 0;
		var mimicShadowInstance = Resources.Load<GameObject> ("Prefabs/MimicShadow");
		_mimicShadow = Instantiate (mimicShadowInstance, transform.position, transform.rotation);
		_mimicShadow.GetComponent<MimicShadow> ().LinkedGameObject = this.gameObject;
	}

    private void SetPlayerTwoAngleSprites()
    {
        var tmpSprite = AngleSprites[0];
        AngleSprites[0] = AngleSprites[4];
        AngleSprites[4] = tmpSprite;
        tmpSprite = AngleSprites[1];
        AngleSprites[1] = AngleSprites[3];
        AngleSprites[3] = tmpSprite;
    }

    void Update()
    {
        if (IsDashing)
        {
            if (HasTheDisc)
                EndDash();

            float distance = WalkDistance;
            if (_dashingDirection == Direction.Left)
                distance = -distance;

            if ((_dashingStart.x < _dashingEnd.x && transform.position.x < _dashingEnd.x - DashDistance / 3) ||
                (_dashingStart.x > _dashingEnd.x && transform.position.x > _dashingEnd.x + DashDistance / 3))
            {
                transform.position += new Vector3(distance * 3, 0.0f, 0.0f); //Dash Speed
            }
            else if ((_dashingStart.x < _dashingEnd.x && transform.position.x < _dashingEnd.x) ||
                     (_dashingStart.x > _dashingEnd.x && transform.position.x > _dashingEnd.x))
            {
                if (Vector3.Distance(_dashingStart, _dashingEnd) <= DashDistance / 2)
                    transform.position += new Vector3(distance * 3, 0.0f, 0.0f); //Dash Speed
                else
                    transform.position += new Vector3(distance / 2, 0.0f, 0.0f); //End Dash Speed
            }
            else
            {
                EndDash();
            }
        }

		if (IsControlledByAI && ControlledAction == ControlledAction.Recenter)
		{
			Recenter ();
		}
    }

    private GameObject GetBall()
    {
		var ballTab = GameObject.FindGameObjectsWithTag ("Disc");
		int ballIndex = -1;
		for (int i = 0; i < ballTab.Length; ++i)
		{
			if (ballTab [i].GetComponent<BallBehavior> ().CurrentPlayer == Player)
			{
				ballIndex = i;
				break;
			}
		}
		if (ballIndex == -1)
			return null;
		return ballTab[ballIndex];
    }

    public void Move(Direction direction)
    {
		if (HasTheDisc || IsDashing)
		{
			LiftDirection = direction;
			return;
		}

		if (IsCastingSP)
			return;

        float distance = WalkDistance;
        if (direction == Direction.Left)
            distance = -distance;

        Direction = direction;
		SetOrientation ();
        transform.position += new Vector3(distance, 0.0f, 0.0f);
        if (transform.position.x < -_gameManager.GetComponent<GameManagerBehavior>().DistanceWall)
            transform.position = new Vector3(-_gameManager.GetComponent<GameManagerBehavior>().DistanceWall, transform.position.y, 0.0f);
        if (transform.position.x > _gameManager.GetComponent<GameManagerBehavior>().DistanceWall)
            transform.position = new Vector3(_gameManager.GetComponent<GameManagerBehavior>().DistanceWall, transform.position.y, 0.0f);
    }

    public void Standby()
    {
        if (IsDashing == true)
            return;
        Direction = Direction.Standby;
		SetOrientation ();
    }

    private void SetOrientation ()
	{
		if (Direction == Direction.Left) {
			transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f); 
			Animator.SetBool ("IsMoving", true);
		} else if (Direction == Direction.Right) {
			transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f); 
			Animator.SetBool ("IsMoving", true);
		} else {
			transform.rotation = _initialRotation;
			Animator.SetBool ("IsMoving", false);
		}
	}

    public void IncrementAngle()
    {
        if (!HasTheDisc)
            return;
        var tmpThrowAngle = _throwAngle + 0.5f;
        if (tmpThrowAngle >= -1f && tmpThrowAngle <= 1f)
            _throwAngle = tmpThrowAngle;
		SetSpriteFromAngle ();
    }

    public void DecrementAngle()
    {
        if (!HasTheDisc)
            return;
        var tmpThrowAngle = _throwAngle - 0.5f;
        if (tmpThrowAngle >= -1f && tmpThrowAngle <= 1f)
            _throwAngle = tmpThrowAngle;
		SetSpriteFromAngle ();
    }

	private void SetSpriteFromAngle ()
	{
		if (_throwAngle <= -1.0f)
			CurrentSprite.sprite = AngleSprites [0];
		else if (_throwAngle == -0.5f)
			CurrentSprite.sprite = AngleSprites [1];
		else if (_throwAngle == 0.0f)
			CurrentSprite.sprite = AngleSprites [2];
		else if (_throwAngle == 0.5f)
			CurrentSprite.sprite = AngleSprites [3];
		else if (_throwAngle >= 1.0f)
			CurrentSprite.sprite = AngleSprites [4];
	}

	public void Eject(Vector2 ballPosition)
	{
		_canDash = true;
		IsCastingSP = false;
		if (Vector2.Distance (ballPosition, transform.position) > 2.0f)
			return;
		if (ballPosition.x < transform.position.x)
			Direction = Direction.Right;
		else
			Direction = Direction.Left;
		Dash ();
		if (Direction == Direction.Right)
			Direction = Direction.Left;
		else
			Direction = Direction.Right;
		SetOrientation ();
	}

    public void Dash()
    {
		if (!_canDash || Direction == Direction.Standby || IsCastingSP)
            return;

        _canDash = false;
        IsDashing = true;
        float distance = DashDistance;
        if (Direction == Direction.Left)
            distance = -distance;

		SetOrientation ();
        _dashingDirection = Direction;
        _dashingStart = transform.position;
        _dashingEnd = transform.position + new Vector3(distance, 0.0f, 0.0f);
        if (_dashingEnd.x < -_gameManager.GetComponent<GameManagerBehavior>().DistanceWall)
            _dashingEnd = new Vector3(-_gameManager.GetComponent<GameManagerBehavior>().DistanceWall, _dashingEnd.y, 0.0f);
        if (_dashingEnd.x > _gameManager.GetComponent<GameManagerBehavior>().DistanceWall)
            _dashingEnd = new Vector3(_gameManager.GetComponent<GameManagerBehavior>().DistanceWall, _dashingEnd.y, 0.0f);

		Animator.enabled = false;
		CurrentSprite.sprite = DashSprite;
        Invoke("ResetDash", _dashCooldown);
		var tmpDashEffect = Instantiate(DashEffect, transform.position, transform.rotation);
		tmpDashEffect.transform.eulerAngles += new Vector3 (0.0f, 0.0f, 180.0f);
        var tmpDashParticles = Instantiate(DashEffectParticles, transform.position, gameObject.transform.rotation);
        tmpDashParticles.GetComponent<EffectBehavior>().ObjectToFollow = gameObject;
    }

    private void ResetDash()
    {
        _canDash = true;
    }

    private void EndDash()
    {
        IsDashing = false;
        Direction = Direction.Standby;
        _dashingDirection = Direction.Standby;
		SetOrientation ();
        if (!HasTheDisc)
		{
			Animator.enabled = true;
			Animator.Play("Idle");
		}
    }

	public void CatchTheDisc()
	{
		//Ball = GetBall ();
		if (Mathf.Abs (transform.position.x) > 2)
			transform.position = new Vector3 (0.0f, transform.position.y, 0.0f);
		if (Ball == null)
			return;
		if (Ball.GetComponent<BallBehavior>().CatchCount >= 0) {
			var tmpCatchEffect = Instantiate(CatchEffect, transform.position, CatchEffect.transform.rotation);
			if (Player == CurrentPlayer.PlayerTwo)
				tmpCatchEffect.transform.eulerAngles = new Vector3 (0.0f, 0.0f, 180.0f);
			if (Ball.GetComponent<BallBehavior>().LastThrownBy != Player)
				SPCooldown = SPCooldown - 1 <= 0 ? 0 : SPCooldown - 1;
		}
		HasTheDisc = true;
		_throwAngle = 0.0f;
		LiftDirection = Direction.Standby;
		Animator.enabled = false;
		CurrentSprite.sprite = AngleSprites [2];
		Direction = Direction.Standby;
		float resetYPos = 1.805f; //Player Y Position
		if (Player == CurrentPlayer.PlayerOne)
			resetYPos = -resetYPos;
		transform.position = new Vector3 (transform.position.x, resetYPos, 0.0f);
		SetOrientation ();
		if (IsCastingSP)
			IsDoingSP = true;
		if (_isAgainstWall)
		{
			var consecutiveHitEffect = Instantiate (ConsecutiveHitEffect, gameObject.transform.position, ConsecutiveHitEffect.transform.rotation);
			consecutiveHitEffect.transform.SetParent (GameObject.Find("Canvas").transform);
			consecutiveHitEffect.transform.position = gameObject.transform.position;
			consecutiveHitEffect.transform.GetChild(0).GetComponent<ConsecutiveHitBehavior> ().Number = ConsecutiveHit;
			++ConsecutiveHit;
		}
	}

	public void Super()
	{
		if (!HasTheDisc)
		{
			return;
		}
		Punchline();
		_mimicShadow.GetComponent<MimicShadow> ().IsDoingAction = true;
		SPCooldown = SPMaxCooldown;
		Invoke("SuperAfterDelay", 0.15f);
		Animator.enabled = true;
		Animator.Play("Throw");
		Invoke ("CheckIfNextBall", 0.4f);
	}

	private void Punchline()
	{
		var punchlineModel = Resources.Load<GameObject> ("Prefabs/Punchline");
		var punchlineInstance = Instantiate (punchlineModel, gameObject.transform.position, gameObject.transform.rotation);
		punchlineInstance.transform.SetParent (GameObject.Find("Canvas").transform);
		punchlineInstance.transform.position = gameObject.transform.position;
		if (_isAgainstAI && Player == CurrentPlayer.PlayerTwo) {
			punchlineInstance.transform.GetChild(0).transform.Rotate(0.0f, 0.0f, 180.0f);
			//punchlineInstance.transform.Rotate(0.0f, 0.0f, 180.0f);
		}
		var punchlinesCount = PunchlinesData.Punchlines [CharacterNumber-1].Count;
		punchlineInstance.transform.GetChild(0).GetComponent<PunchlineBehavior> ().Text = PunchlinesData.Punchlines[CharacterNumber-1][Random.Range(0, punchlinesCount)];
	}

	public void Throw()
	{
		if (!HasTheDisc)
	    {
	        Dash();
	        return;
	    }
		_mimicShadow.GetComponent<MimicShadow> ().IsDoingAction = true;
	    Invoke("ThrowBallAfterDelay", 0.15f);
		Animator.enabled = true;
        Animator.Play("Throw");
		Invoke ("CheckIfNextBall", 0.4f);
	}

	public void Lift()
	{
		if (!HasTheDisc)
		{
			if (!IsCastingSP && !IsDashing)
				CastSP();
			return;
		}
		_mimicShadow.GetComponent<MimicShadow> ().IsDoingAction = true;
		Invoke("LiftBallAfterDelay", 0.15f);
		Animator.enabled = true;
		Animator.Play("Throw");
		Invoke ("CheckIfNextBall", 0.4f);
	}

	public void CheckIfNextBall ()
	{
		if (NextBalls.Count > 0) {
			IsEngaging = true;
			HasTheDisc = false;
			IsDoingSP = false;
			Ball = NextBalls[0].gameObject;
			NextBalls.RemoveAt (0);
			Ball.GetComponent<BallBehavior> ().CurrentPlayer = Player;
			Ball.GetComponent<BallBehavior> ().IsNextBall = false;
			Ball.GetComponent<BallBehavior> ().HasHitPlayer = false;
		} else {
			ResetThrow ();
		}
	}

	private void ResetThrow()
	{
		if (NextBalls.Count == 0) {
			HasTheDisc = false;
			Animator.Play ("Idle");	
		} else {
			CheckIfNextBall ();
		}
		IsDoingSP = false;
		IsEngaging = false;
    }

    private void ThrowBallAfterDelay()
    {
		CustomAudio.PlayEffect(_gameManager.GetComponent<GameManagerBehavior>().ThrowAudioFileID);
        //Ball = GetBall();
		if (Ball != null && Ball.GetComponent<BallBehavior> ().IsThrownBy != CurrentPlayer.None)
			return;
        if (Ball != null)
            Ball.GetComponent<BallBehavior>().Throw(_directionalVector + new Vector2(_throwAngle, 0.0f), Player, Power, false);
        _throwAngle = 0;
    }

	private void LiftBallAfterDelay()
	{
		CustomAudio.PlayEffect(_gameManager.GetComponent<GameManagerBehavior>().ThrowAudioFileID);
		//Ball = GetBall();
		if (Ball != null && Ball.GetComponent<BallBehavior> ().IsThrownBy != CurrentPlayer.None)
			return;
		if (Ball != null)
			Ball.GetComponent<BallBehavior>().Throw(_directionalVector + new Vector2(_throwAngle, 0.0f), Player, Power, true);
		_throwAngle = 0;
	}

	private void SuperAfterDelay()
	{
		CustomAudio.PlayEffect(_gameManager.GetComponent<GameManagerBehavior>().ThrowAudioFileID);
		//Ball = GetBall();
		if (Ball != null && Ball.GetComponent<BallBehavior> ().IsThrownBy != CurrentPlayer.None)
			return;
		if (Ball != null)
			this.GetComponent<SuperBehavior>().LaunchSupper(_directionalVector + new Vector2(_throwAngle, 0.0f), Player, Power, _directionalVector, Ball);
		_throwAngle = 0;
		var tmpSpEffect = Instantiate(CastSPEffect, transform.position, transform.rotation);
		tmpSpEffect.GetComponent<SpriteRenderer> ().color = SuperColor;
	}

    public void ResetInitialPosition()
	{
		transform.position = _initialPosition;
	}

	public void Recenter()
	{
		IsControlledByAI = true;
		if (HasTheDisc)
		{
			HasTheDisc = false;
			Animator.enabled = true;
			Animator.Play("Idle");
		}
		ControlledAction = ControlledAction.Recenter;
		if (transform.position.x + 0.1f < 0)
			Move (Direction.Right);
		else if (transform.position.x - 0.1f > 0)
			Move (Direction.Left);
		else
		{
			Standby();
		}
	}

	private void CastSP()
	{
		if (IsEngaging || SPCooldown > 0)
			return;
		IsCastingSP = true;
		Invoke ("ResetCastSP", _castSPCooldown);

		Direction = Direction.Standby;
		SetOrientation ();
		Animator.enabled = false;
		CurrentSprite.sprite = SPSprite;
		Instantiate(CastSPEffect, transform.position, transform.rotation);
	}

	private void ResetCastSP()
	{
		IsCastingSP = false;
		if (!HasTheDisc)
		{
			Animator.enabled = true;
			Animator.Play("Idle");
		}
	}

	void OnDestroy()
	{
	}
}
