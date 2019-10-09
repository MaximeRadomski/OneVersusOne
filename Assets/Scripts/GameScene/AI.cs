using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
	public CurrentPlayer Player;
    public bool HasHitPredictionDisc;

	private GameObject _ball;
	private GameObject _linkedPlayer;
	private bool _isThrowing;
    private float _repeatDashCooldown;
	private float _yBallLimit;
    private bool _canDash;
	private bool _canDahEarly;
	private CurrentPlayer _rival;

	private float _throwDelay;
	private float _startReactDistance;
	private float _startCastDistance;

	private bool _checkSelfGoal;
    private bool _canRecenter;
    private float _canRecenterDelay;
    private bool _isBackCourt;
    private float _backCourtY;
    private MiddleWallBehavior _middleWallBehavior;

    /// <summary>
    /// Easy Difficulty:	Current Setting minus Recentering
    /// Medium Difficulty:	Current Settings
    /// Hard Difficulty:	_throwDelay = 0.05f and _startReactDistance = 0.5f
    /// </summary>

    void Start ()
	{
		GetBall ();
		GetPlayers ();
		ResetRandomFactors ();
		_isThrowing = false;
	    _canDash = true;
		_checkSelfGoal = false;
		if (Player == CurrentPlayer.PlayerOne)
			_rival = CurrentPlayer.PlayerTwo;
		else if (Player == CurrentPlayer.PlayerTwo)
			_rival = CurrentPlayer.PlayerOne;
        _canRecenter = true;
        _isBackCourt = true;
        _backCourtY = -1.805f;
        HasHitPredictionDisc = false;
        _middleWallBehavior = GameObject.Find("MiddleWall01")?.GetComponent<MiddleWallBehavior>();
        if (_linkedPlayer.GetComponent<PlayerBehavior>().CharacterNumber >= 5)
            _linkedPlayer.GetComponent<PlayerBehavior>().DashDistance = 1.1f;
    }

	private void ResetRandomFactors()
	{
		if (PlayerPrefs.GetInt ("Difficulty", 0) == Difficulty.Easy.GetHashCode ()) {
			_throwDelay = 0.5f;
            _startReactDistance = Random.Range(0.25f, 0.45f);
            _startCastDistance = 1.0f;
			_repeatDashCooldown = 1.0f;
			_yBallLimit = 1.5f;
            _canRecenterDelay = 0.75f;
            var tmpCanDashEarly = Random.Range (0, 3);
			_canDahEarly = tmpCanDashEarly == 0 ? false : true;
		} else if (PlayerPrefs.GetInt ("Difficulty", 0) == Difficulty.Normal.GetHashCode ()) {
			_throwDelay = Random.Range (0.0f, 0.5f);
			_startReactDistance = Random.Range (0.3f, 0.4f);
			_startCastDistance = Random.Range (0.5f, 1.0f);
			_repeatDashCooldown = 0.75f;
			_yBallLimit = 1.25f;
            _canRecenterDelay = 0.625f;
            var tmpCanDashEarly = Random.Range (0, 2);
			_canDahEarly = tmpCanDashEarly == 0 ? false : true;
		} else {
			_throwDelay = 0.0f;
			_startReactDistance = 0.35f;
			_startCastDistance = 0.75f;
			_repeatDashCooldown = 0.5f;
			_yBallLimit = 1.0f;
            _canRecenterDelay = 0.5f;
            _canDahEarly = false;
		}
		if (Player == CurrentPlayer.PlayerOne)
			_yBallLimit = -_yBallLimit;
        HasHitPredictionDisc = false;
    }

	private string GetFocusedPayerName()
	{
		if (Player == CurrentPlayer.PlayerOne)
			return "PlayerOne";
		return "PlayerTwo";
	}

	private GameObject GetBall()
	{
		var ball = GameObject.FindGameObjectWithTag ("Disc");
		var ballShadow = GameObject.FindGameObjectWithTag ("DiscShadow");

		if (ballShadow != null && ball != null) {
			if (Vector2.Distance (ball.transform.position, this.transform.position) <
			    Vector2.Distance (ballShadow.transform.position, this.transform.position))
				return ball;
			else
				return ballShadow;
		}
		return ball;
	}

    private GameObject GetPredictionDisc()
    {
        var predictionDisc = GameObject.FindGameObjectWithTag("PredictionDisc");
        return predictionDisc;
    }

	private void GetPlayers()
	{
		_linkedPlayer = GameObject.Find (GetFocusedPayerName());
	}

	void Update ()
	{
		if (Time.timeScale == 0)
			return;
		_ball = GetBall ();
		if (_ball == null)
			return;
		if (_linkedPlayer == null)
			GetPlayers();
		if (_ball.GetComponent<BallBehavior> ().IsThrownBy == _rival ||
			(_ball.GetComponent<BallBehavior> ().IsThrownBy == Player && _checkSelfGoal &&  Vector3.Distance(new Vector3(0.0f, _linkedPlayer.transform.position.y, 0.0f), new Vector3(0.0f, _ball.transform.position.y, 0.0f)) < 1.0f))
		    ActFromBallPosition();
		else if ((_ball.GetComponent<BallBehavior>().IsThrownBy == Player ||
		         _ball.GetComponent<BallBehavior>().IsThrownBy == CurrentPlayer.None)
                 && _canRecenter && !_isThrowing)
            Recenter();
        else
            _linkedPlayer.GetComponent<PlayerBehavior>().Standby();
        if (_linkedPlayer.GetComponent<PlayerBehavior> ().HasTheDisc && !_isThrowing)
		{
            _isThrowing = true;
			Invoke ("Throw", _throwDelay);
		}

	}

    private void ActFromBallPosition()
    {
        if (HasHitPredictionDisc && _isBackCourt)
        {
            if (_linkedPlayer.GetComponent<PlayerBehavior>().SPCooldown <= 0)
                _linkedPlayer.GetComponent<PlayerBehavior>().Lift(); //CastSP
            return;
        }
        var predictionDisc = GetPredictionDisc();
        if (predictionDisc != null && predictionDisc.transform.position.y < transform.position.y
            && GenericHelpers.FloatEqualsPrecision(predictionDisc.transform.position.x, transform.position.x, 0.1f)
            && transform.position.y - predictionDisc.transform.position.y <= 1.25f
            && _canDash
            && _isBackCourt)
        {
            var rand = Random.Range(0, 3);
            if (rand != 0 && (_middleWallBehavior == null || _middleWallBehavior.Order != 2))
            {
                _linkedPlayer.GetComponent<PlayerBehavior>().Dash();
                _canDash = false;
                Invoke("ResetDashPossibility", _repeatDashCooldown);
                return;
            }
        }
		bool shouldDashLimit = false;
		if (_ball.transform.position.y < _startReactDistance && Player == CurrentPlayer.PlayerTwo)
            return;
		if (_ball.transform.position.y > -_startReactDistance && Player == CurrentPlayer.PlayerOne)
			return;
		if (_ball.transform.position.y > _yBallLimit && Player == CurrentPlayer.PlayerTwo)
			shouldDashLimit = true;
		if (_ball.transform.position.y < _yBallLimit && Player == CurrentPlayer.PlayerOne)
			shouldDashLimit = true;
		if ((_canDahEarly && _ball.transform.position.x + _linkedPlayer.GetComponent<PlayerBehavior> ().DashDistance < transform.position.x && _canDash) ||
			(shouldDashLimit == true && _ball.transform.position.x + 0.1f < transform.position.x && _canDash)) {
			_linkedPlayer.GetComponent<PlayerBehavior> ().Direction = Direction.Left;
			_linkedPlayer.GetComponent<PlayerBehavior> ().Dash ();
			_canDash = false;
			Invoke ("ResetDashPossibility", _repeatDashCooldown);
		} else if ((shouldDashLimit == false && _ball.transform.position.x - _linkedPlayer.GetComponent<PlayerBehavior> ().DashDistance > transform.position.x && _canDash) ||
			(shouldDashLimit == true && _ball.transform.position.x - 0.1f > transform.position.x && _canDash)) {
			_linkedPlayer.GetComponent<PlayerBehavior> ().Direction = Direction.Right;
			_linkedPlayer.GetComponent<PlayerBehavior> ().Dash ();
			_canDash = false;
			Invoke ("ResetDashPossibility", _repeatDashCooldown);
		} else if (_ball.transform.position.x + 0.2f < transform.position.x)
			_linkedPlayer.GetComponent<PlayerBehavior> ().Move (Direction.Left);
		else if (_ball.transform.position.x - 0.2f > transform.position.x)
			_linkedPlayer.GetComponent<PlayerBehavior> ().Move (Direction.Right);
		else if (_linkedPlayer.GetComponent<PlayerBehavior> ().SPCooldown <= 0 && Vector2.Distance (transform.position, _ball.transform.position) <= _startCastDistance)
			_linkedPlayer.GetComponent<PlayerBehavior> ().Lift (); //CastSP
		else {
			_linkedPlayer.GetComponent<PlayerBehavior> ().Standby ();
		}
    }

    //private void Recenter()
    //{
    //    if (transform.position.x + 0.1f < 0)
    //        _linkedPlayer.GetComponent<PlayerBehavior>().Move(Direction.Right);
    //    else if (transform.position.x - 0.1f > 0)
    //        _linkedPlayer.GetComponent<PlayerBehavior>().Move(Direction.Left);
    //    else
    //        _linkedPlayer.GetComponent<PlayerBehavior>().Standby();
    //}

    public void Recenter()
    {
        HasHitPredictionDisc = false;
        bool canStandbyHorizontal = false;
        bool canStandbyVertical = false;
        if (transform.position.x + 0.1f < 0)
            _linkedPlayer.GetComponent<PlayerBehavior>().Move(Direction.Right);
        else if (transform.position.x - 0.1f > 0)
            _linkedPlayer.GetComponent<PlayerBehavior>().Move(Direction.Left);
        else
            canStandbyHorizontal = true;

        if (Player == CurrentPlayer.PlayerOne == transform.position.y > _backCourtY)
            transform.position += new Vector3(0.0f, -_linkedPlayer.GetComponent<PlayerBehavior>().WalkDistance, 0.0f);
        else if (Player == CurrentPlayer.PlayerTwo == transform.position.y < -_backCourtY)
            transform.position += new Vector3(0.0f, _linkedPlayer.GetComponent<PlayerBehavior>().WalkDistance, 0.0f);
        else
            canStandbyVertical = true;

        if (!canStandbyVertical && GenericHelpers.FloatEqualsPrecision(transform.position.x, 0.0f, 0.2f))
        {
            _linkedPlayer.GetComponent<PlayerBehavior>().Direction = Direction.BackDash;
            _linkedPlayer.GetComponent<PlayerBehavior>().SetOrientation();
        }

        if (canStandbyHorizontal && canStandbyVertical)
            _linkedPlayer.GetComponent<PlayerBehavior>().Standby();
        _isBackCourt = true;
    }

    private void ResetDashPossibility()
    {
        _canDash = true;
        if (GenericHelpers.FloatEqualsPrecision(Mathf.Abs(transform.position.y), Mathf.Abs(_backCourtY), 0.25f))
            _isBackCourt = true;
        else
            _isBackCourt = false;
    }

    private void Throw()
	{
		_checkSelfGoal = false;
		int leftRight = Random.Range (0,2);
		if (leftRight == 0)
		{
			int angle = Random.Range (0, 3);
			for (int i = 0; i < angle; ++i)
			{
				_linkedPlayer.GetComponent<PlayerBehavior> ().DecrementAngle ();
            }
		}
		else
		{
			int angle = Random.Range (0, 3);
			for (int i = 0; i < angle; ++i)
			{
				_linkedPlayer.GetComponent<PlayerBehavior> ().IncrementAngle ();
            }
		}
		if (_linkedPlayer.GetComponent<PlayerBehavior> ().IsCastingSP)
		{
			_linkedPlayer.GetComponent<PlayerBehavior> ().Super ();
		}
		else
		{
			int nextThrow = Random.Range (0, 2);
			if (nextThrow == 0)
				_linkedPlayer.GetComponent<PlayerBehavior> ().Throw ();
			else {
				_linkedPlayer.GetComponent<PlayerBehavior> ().Lift ();
				int liftDirection = Random.Range (0, 2);
				if (liftDirection == 0)
					_linkedPlayer.GetComponent<PlayerBehavior> ().Move (Direction.Left);
				else
					_linkedPlayer.GetComponent<PlayerBehavior> ().Move (Direction.Right);
			}
		}
        _canRecenter = false;
        Invoke("ResetThrowPossibility", 0.5f);
        Invoke("ResetCanRecenter", _canRecenterDelay);
    }

    private void ResetThrowPossibility()
    {
        _isThrowing = false;
		_checkSelfGoal = true;
		ResetRandomFactors ();
    }

    private void ResetCanRecenter()
    {
        if (!_isBackCourt)
        {
            var tmpCanRecenter = Random.Range(0, 3);
            _canRecenter = tmpCanRecenter != 0 ? true : false;
        }
        else
            _canRecenter = true;
    }
}
