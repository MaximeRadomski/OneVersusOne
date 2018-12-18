using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
	private GameObject _ball;
	private GameObject _playerTwo;
	private bool _isThrowing;
	private float _distanceWall;

	void Start ()
	{
		GetBall ();
		GetPlayers ();
		_isThrowing = false;
		_distanceWall = 1.5f;
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
		if (_ball.GetComponent<BallBehavior> ().IsThrownBy == CurrentPlayer.PlayerOne) {
			if (_ball.transform.position.x + 0.2f < transform.position.x)
				Move (-0.05f, true);
			else if (_ball.transform.position.x - 0.2f > transform.position.x)
				Move (0.05f, false);
		}
		if (_playerTwo == null)
			GetPlayers();
		if (_playerTwo.GetComponent<PlayerBehavior> ().HasTheDisc && _isThrowing == false)
		{
		    StopMoving();
            _isThrowing = true;
			Invoke ("Throw", 0.5f);
		}
	}

	private void Move(float distance, bool isLeft)
	{
		_playerTwo.transform.position += new Vector3(distance, 0.0f, 0.0f);
		_playerTwo.GetComponent<PlayerBehavior>().IsGoingLeft = isLeft;
		_playerTwo.GetComponent<PlayerBehavior>().IsGoingRight = !isLeft;
		if (_playerTwo.transform.position.x < -1.0f * _distanceWall)
			_playerTwo.transform.position = new Vector3(-1.0f * _distanceWall, _playerTwo.transform.position.y, 0.0f);
		if (_playerTwo.transform.position.x > _distanceWall)
			_playerTwo.transform.position = new Vector3(_distanceWall, _playerTwo.transform.position.y, 0.0f);
	}

	private void StopMoving ()
	{
		_playerTwo.GetComponent<PlayerBehavior>().IsGoingLeft = false;
		_playerTwo.GetComponent<PlayerBehavior>().IsGoingRight = false;
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
	    Invoke("ThrowBallAfterDelay", 0.15f);
        _playerTwo.GetComponent<PlayerBehavior> ().Throw ();
		_isThrowing = false;
	}

    private void ThrowBallAfterDelay()
    {
        _ball.GetComponent<BallBehavior>().Throw(_playerTwo.GetComponent<PlayerBehavior>().DirectionalVector +
                                                 new Vector2(_playerTwo.GetComponent<PlayerBehavior>().ThrowAngle, 0.0f), CurrentPlayer.PlayerTwo);
    }
}
