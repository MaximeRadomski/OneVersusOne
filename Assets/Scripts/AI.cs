using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
	private GameObject _ball;
	private GameObject _playerTwo;
	private bool _isThrowing;

	void Start ()
	{
		GetBall ();
		GetPlayers ();
		_isThrowing = false;
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
		if (_ball.GetComponent<BallBehavior> ().IsThrownBy == CurrentPlayer.PlayerOne) {
			if (_ball.transform.position.x + 0.2f < transform.position.x)
				_playerTwo.GetComponent<PlayerBehavior> ().Move(Direction.Left);
			else if (_ball.transform.position.x - 0.2f > transform.position.x)
				_playerTwo.GetComponent<PlayerBehavior> ().Move(Direction.Right);
		}
		if (_playerTwo.GetComponent<PlayerBehavior> ().HasTheDisc && _isThrowing == false)
		{
            _isThrowing = true;
			Invoke ("Throw", 0.5f);
		}
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
        _playerTwo.GetComponent<PlayerBehavior> ().Throw ();
		_isThrowing = false;
	}
}
