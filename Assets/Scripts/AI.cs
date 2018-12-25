﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
	private GameObject _ball;
	private GameObject _playerTwo;
	private bool _isThrowing;
    private float _repeatDashCooldown;
    private bool _canDash;

	void Start ()
	{
		GetBall ();
		GetPlayers ();
		_isThrowing = false;
	    _repeatDashCooldown = 1.0f;
	    _canDash = true;
	}

	private GameObject GetBall()
	{
		return GameObject.Find ("Ball");
	}

	private void GetPlayers()
	{
		_playerTwo = GameObject.Find ("PlayerTwo");
	}

	void Update ()
	{
		if (_ball == null)
			_ball = GetBall ();
		if (_ball == null)
			return;
		if (_playerTwo == null)
			GetPlayers();
		if (_ball.GetComponent<BallBehavior> ().IsThrownBy == CurrentPlayer.PlayerOne)
		    ActFromBallPosition();
		else if (_ball.GetComponent<BallBehavior>().IsThrownBy == CurrentPlayer.PlayerTwo ||
		         _ball.GetComponent<BallBehavior>().IsThrownBy == CurrentPlayer.None)
            Recenter();
        if (_playerTwo.GetComponent<PlayerBehavior> ().HasTheDisc && _isThrowing == false)
		{
            _isThrowing = true;
			Invoke ("Throw", 0.5f);
		}
	}

    private void ActFromBallPosition()
    {
        if (_ball.transform.position.y < 0.5f)
            return;
        if (_ball.transform.position.x + _playerTwo.GetComponent<PlayerBehavior>().DashDistance < transform.position.x && _canDash)
        {
            _playerTwo.GetComponent<PlayerBehavior>().Direction = Direction.Left;
            _playerTwo.GetComponent<PlayerBehavior>().Dash();
            _canDash = false;
            Invoke("ResetDashPossibility", _repeatDashCooldown);
        }
        else if (_ball.transform.position.x - _playerTwo.GetComponent<PlayerBehavior>().DashDistance  > transform.position.x && _canDash)
        {
            _playerTwo.GetComponent<PlayerBehavior>().Direction = Direction.Right;
            _playerTwo.GetComponent<PlayerBehavior>().Dash();
            _canDash = false;
            Invoke("ResetDashPossibility", _repeatDashCooldown);
        }
        else if (_ball.transform.position.x + 0.2f < transform.position.x)
            _playerTwo.GetComponent<PlayerBehavior>().Move(Direction.Left);
        else if (_ball.transform.position.x - 0.2f > transform.position.x)
            _playerTwo.GetComponent<PlayerBehavior>().Move(Direction.Right);
    }

    private void Recenter()
    {
        if (transform.position.x + 0.1f < 0)
            _playerTwo.GetComponent<PlayerBehavior>().Move(Direction.Right);
        else if (transform.position.x - 0.1f > 0)
            _playerTwo.GetComponent<PlayerBehavior>().Move(Direction.Left);
        else
            _playerTwo.GetComponent<PlayerBehavior>().Standby();
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
				_playerTwo.GetComponent<PlayerBehavior> ().DecrementAngle ();
			}
		}
		else
		{
			int angle = Random.Range (0, 3);
			for (int i = 0; i < angle; ++i)
			{
				_playerTwo.GetComponent<PlayerBehavior> ().IncrementAngle ();
			}
		}
		int nextThrow = Random.Range (0, 2);
		if (nextThrow == 0)
        	_playerTwo.GetComponent<PlayerBehavior> ().Throw ();
		else
		{
			_playerTwo.GetComponent<PlayerBehavior> ().Lift ();
			int liftDirection = Random.Range (0, 2);
			if (liftDirection == 0)
				_playerTwo.GetComponent<PlayerBehavior>().Move(Direction.Left);
			else
				_playerTwo.GetComponent<PlayerBehavior>().Move(Direction.Right);
		}
        Invoke("ResetThrowPossibility", 0.5f);
    }

    private void ResetThrowPossibility()
    {
        _isThrowing = false;
    }
}
