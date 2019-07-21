using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherBehavior : MonoBehaviour
{
	public Direction LiftDirection;

	private GameObject _gameManager;
	private GameObject _currentBall;
	private float _throwAngle;
	private float _power;

	void Start()
	{
		GetGameManager ();
		//if (PlayerPrefs.GetInt ("CurrentChallengeDifficulty") == 1)
		_power = 1.5f;

	}

	private void GetGameManager()
	{
		_gameManager = GameObject.Find ("$GameManager");
	}

	public void Launch(GameObject ball)
	{
		_throwAngle = 0.0f;
		this.gameObject.GetComponent<Animator> ().Play ("Launch");
		_currentBall = Instantiate(ball, new Vector3(3.0f, 0.0f, 0.0f), ball.transform.rotation);
		_currentBall.transform.name = "Ball";
		_currentBall.GetComponent<BallBehavior> ().CatchCount = 1;
		_currentBall.GetComponent<BallBehavior> ().CurrentPlayer = CurrentPlayer.None;
		int leftRight = Random.Range (0,2);
		if (leftRight == 0)
		{
			int angle = Random.Range (0, 3);
			for (int i = 0; i < angle; ++i)
			{
				DecrementAngle ();
			}
		}
		else
		{
			int angle = Random.Range (0, 3);
			for (int i = 0; i < angle; ++i)
			{
				IncrementAngle ();
			}
		}
		int nextThrow = Random.Range (0, 2);
		if (nextThrow == 0)
			Invoke ("Throw", 0.1f);
		else {
			Invoke ("Lift", 0.1f);
			int liftDirection = Random.Range (0, 2);
			if (liftDirection == 0)
				LiftDirection = Direction.Left;
			else
				LiftDirection = Direction.Right;
		}
	}

	public void IncrementAngle()
	{
		var tmpThrowAngle = _throwAngle + 0.5f;
		if (tmpThrowAngle >= -1f && tmpThrowAngle <= 1f)
			_throwAngle = tmpThrowAngle;
	}

	public void DecrementAngle()
	{
		var tmpThrowAngle = _throwAngle - 0.5f;
		if (tmpThrowAngle >= -1f && tmpThrowAngle <= 1f)
			_throwAngle = tmpThrowAngle;
	}

	private void Throw()
	{
		if (_gameManager == null)
			GetGameManager ();
		_currentBall.GetComponent<BallBehavior> ().CatchCount = 1;
		CustomAudio.PlayEffect(_gameManager.GetComponent<GameManagerBehavior>().ThrowAudioFileID);
		_currentBall.transform.position = new Vector3 (transform.position.x, transform.position.y - 0.25f, 0.0f);
		_currentBall.GetComponent<BallBehavior>().Throw(Vector2.down + new Vector2(_throwAngle, 0.0f), CurrentPlayer.PlayerTwo, _power, false);
		_throwAngle = 0;
	}

	private void Lift()
	{
		if (_gameManager == null)
			GetGameManager ();
		_currentBall.GetComponent<BallBehavior> ().CatchCount = 1;
		CustomAudio.PlayEffect(_gameManager.GetComponent<GameManagerBehavior>().ThrowAudioFileID);
		_currentBall.transform.position = new Vector3 (transform.position.x, transform.position.y - 0.25f, 0.0f);
		_currentBall.GetComponent<BallBehavior>().Throw(Vector2.down + new Vector2(_throwAngle, 0.0f), CurrentPlayer.PlayerTwo, _power, true);
		_throwAngle = 0;
	}
}
