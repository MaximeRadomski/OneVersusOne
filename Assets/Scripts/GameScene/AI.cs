using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
	public CurrentPlayer Player;

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
	}

	private void ResetRandomFactors()
	{
		if (PlayerPrefs.GetInt ("Difficulty", 0) == Difficulty.Easy.GetHashCode ()) {
			_throwDelay = 0.5f;
			_startReactDistance = -0.5f;
			_startCastDistance = 1.0f;
			_repeatDashCooldown = 1.0f;
			_yBallLimit = 2.0f;
			var tmpCanDashEarly = Random.Range (0, 3);
			_canDahEarly = tmpCanDashEarly == 0 ? false : true;
		} else if (PlayerPrefs.GetInt ("Difficulty", 0) == Difficulty.Normal.GetHashCode ()) {
			_throwDelay = Random.Range (0.0f, 0.5f);
			_startReactDistance = Random.Range (-0.5f, 0.5f);
			_startCastDistance = Random.Range (0.5f, 1.0f);
			_repeatDashCooldown = 0.75f;
			_yBallLimit = 1.75f;
			var tmpCanDashEarly = Random.Range (0, 2);
			_canDahEarly = tmpCanDashEarly == 0 ? false : true;
		} else {
			_throwDelay = 0.0f;
			_startReactDistance = 0.35f;
			_startCastDistance = 0.75f;
			_repeatDashCooldown = 0.5f;
			_yBallLimit = 1.5f;
			_canDahEarly = false;
		}
		if (Player == CurrentPlayer.PlayerOne)
			_yBallLimit = -_yBallLimit;
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

		if (ballShadow != null) {
			if (Vector2.Distance (ball.transform.position, this.transform.position) <
			    Vector2.Distance (ballShadow.transform.position, this.transform.position))
				return ball;
			else
				return ballShadow;
		}
		return ball;
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
		else if (_ball.GetComponent<BallBehavior>().IsThrownBy == Player ||
		         _ball.GetComponent<BallBehavior>().IsThrownBy == CurrentPlayer.None)
            Recenter();
		if (_linkedPlayer.GetComponent<PlayerBehavior> ().HasTheDisc && _isThrowing == false)
		{
            _isThrowing = true;
			Invoke ("Throw", _throwDelay);
		}

	}

    private void ActFromBallPosition()
    {
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
			(shouldDashLimit == true && _ball.transform.position.x < transform.position.x && _canDash)) {
			_linkedPlayer.GetComponent<PlayerBehavior> ().Direction = Direction.Left;
			_linkedPlayer.GetComponent<PlayerBehavior> ().Dash ();
			_canDash = false;
			Invoke ("ResetDashPossibility", _repeatDashCooldown);
		} else if ((shouldDashLimit == false && _ball.transform.position.x - _linkedPlayer.GetComponent<PlayerBehavior> ().DashDistance > transform.position.x && _canDash) ||
			(shouldDashLimit == true && _ball.transform.position.x > transform.position.x && _canDash)) {
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

    private void Recenter()
    {
        if (transform.position.x + 0.1f < 0)
			_linkedPlayer.GetComponent<PlayerBehavior>().Move(Direction.Right);
        else if (transform.position.x - 0.1f > 0)
			_linkedPlayer.GetComponent<PlayerBehavior>().Move(Direction.Left);
        else
			_linkedPlayer.GetComponent<PlayerBehavior>().Standby();
    }

    private void ResetDashPossibility()
    {
        _canDash = true;
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
        Invoke("ResetThrowPossibility", 1.0f);
    }

    private void ResetThrowPossibility()
    {
        _isThrowing = false;
		_checkSelfGoal = true;
		ResetRandomFactors ();
    }
}
