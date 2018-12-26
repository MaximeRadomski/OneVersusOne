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
    private bool _canDash;
	private CurrentPlayer _rival;

	void Start ()
	{
		GetBall ();
		GetPlayers ();
		_isThrowing = false;
	    _repeatDashCooldown = 1.0f;
	    _canDash = true;
		if (Player == CurrentPlayer.PlayerOne)
			_rival = CurrentPlayer.PlayerTwo;
		else if (Player == CurrentPlayer.PlayerTwo)
			_rival = CurrentPlayer.PlayerOne;
	}

	private string GetFocusedPayerName()
	{
		if (Player == CurrentPlayer.PlayerOne)
			return "PlayerOne";
		return "PlayerTwo";
	}

	private GameObject GetBall()
	{
		return GameObject.Find ("Ball");
	}

	private void GetPlayers()
	{
		_linkedPlayer = GameObject.Find (GetFocusedPayerName());
	}

	void Update ()
	{
		if (_ball == null)
			_ball = GetBall ();
		if (_ball == null)
			return;
		if (_linkedPlayer == null)
			GetPlayers();
		if (_ball.GetComponent<BallBehavior> ().IsThrownBy == _rival)
		    ActFromBallPosition();
		else if (_ball.GetComponent<BallBehavior>().IsThrownBy == Player ||
		         _ball.GetComponent<BallBehavior>().IsThrownBy == CurrentPlayer.None)
            Recenter();
		if (_linkedPlayer.GetComponent<PlayerBehavior> ().HasTheDisc && _isThrowing == false)
		{
            _isThrowing = true;
			Invoke ("Throw", 0.5f);
		}
	}

    private void ActFromBallPosition()
    {
		if (_ball.transform.position.y < 0.5f && Player == CurrentPlayer.PlayerTwo)
            return;
		if (_ball.transform.position.y > -0.5f && Player == CurrentPlayer.PlayerOne)
			return;
		if (_ball.transform.position.x + _linkedPlayer.GetComponent<PlayerBehavior>().DashDistance < transform.position.x && _canDash)
        {
			_linkedPlayer.GetComponent<PlayerBehavior>().Direction = Direction.Left;
			_linkedPlayer.GetComponent<PlayerBehavior>().Dash();
            _canDash = false;
            Invoke("ResetDashPossibility", _repeatDashCooldown);
        }
		else if (_ball.transform.position.x - _linkedPlayer.GetComponent<PlayerBehavior>().DashDistance  > transform.position.x && _canDash)
        {
			_linkedPlayer.GetComponent<PlayerBehavior>().Direction = Direction.Right;
			_linkedPlayer.GetComponent<PlayerBehavior>().Dash();
            _canDash = false;
            Invoke("ResetDashPossibility", _repeatDashCooldown);
        }
        else if (_ball.transform.position.x + 0.2f < transform.position.x)
			_linkedPlayer.GetComponent<PlayerBehavior>().Move(Direction.Left);
        else if (_ball.transform.position.x - 0.2f > transform.position.x)
			_linkedPlayer.GetComponent<PlayerBehavior>().Move(Direction.Right);
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
		int nextThrow = Random.Range (0, 2);
		if (nextThrow == 0)
			_linkedPlayer.GetComponent<PlayerBehavior> ().Throw ();
		else
		{
			_linkedPlayer.GetComponent<PlayerBehavior> ().Lift ();
			int liftDirection = Random.Range (0, 2);
			if (liftDirection == 0)
				_linkedPlayer.GetComponent<PlayerBehavior>().Move(Direction.Left);
			else
				_linkedPlayer.GetComponent<PlayerBehavior>().Move(Direction.Right);
		}
        Invoke("ResetThrowPossibility", 0.5f);
    }

    private void ResetThrowPossibility()
    {
        _isThrowing = false;
    }
}
