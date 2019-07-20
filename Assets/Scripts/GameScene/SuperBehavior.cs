using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperBehavior : MonoBehaviour
{
	public SuperType Super;
	public GameObject Effect;
	public GameObject WhiteBall;

	private GameObject _ball;
	private GameObject _shadow;
	private float _superEffectDelay;
	private float _baseSuperEffectDelay;
	private float _zigzagDelay;
	private Vector2 _currentThrowDirection;
	private Vector2 _playerThrowDirection;
	//private GameObject _currentPlayer;
	private float _customSpeed;
	private float _zigzag;
	private int _bounceCount;
	private int _bounce;
	private float _bounceYPosition;
	private float _gravity;
	private bool _isDoingSuper;
	//private bool _hasInstantiateNewDisc;

	void Start()
	{
		_superEffectDelay = 0.05f;
		_baseSuperEffectDelay = _superEffectDelay;
		_zigzagDelay = 0.25f;
		_zigzag = 0.0f;
		_bounceCount = 1;
		_bounce = 0;
		_bounceYPosition = 1.75f;
		_gravity = 15.0f;
	}

	private Vector2 ExtremeFromCurrentDirection()
	{
		float x = 0.0f;
		if (_currentThrowDirection.x == 0) {
			if (_ball.transform.position.x >= 0)
				x = 1.0f;
			else
				x = -1.0f;
		} else if (_currentThrowDirection.x > 0) {
			x = 1.0f;
		} else {
			x = -1.0f;
		}
		return new Vector2 (x, _currentThrowDirection.y);
	}

	private Vector2 OnlySideFromCurrentDirection()
	{
		float x = _currentThrowDirection.x;
		if (_currentThrowDirection.x == 0) {
			if (_ball.transform.position.x >= 0)
				x = 0.5f;
			else
				x = -0.5f;
		}
		return new Vector2 (x, _currentThrowDirection.y);
	}

	private void BasicEffectThrow (Vector2 direction, CurrentPlayer throwingPlayer, float addedPower, float fixedSpeed = 0)
	{
		_ball.GetComponent<BallBehavior>().SetGravityScaleFromPower (addedPower);
		_ball.GetComponent<BallBehavior>().IsThrownBy = throwingPlayer;
		_ball.GetComponent<BallBehavior>().CurrentPlayer = CurrentPlayer.None;
		float speedQuarter = (_ball.GetComponent<BallBehavior>().Speed + addedPower) / 4;
		_customSpeed = (_ball.GetComponent<BallBehavior>().Speed + addedPower) - (Mathf.Abs(direction.x) * speedQuarter);
		_customSpeed = _customSpeed * 1.2f;
		if (fixedSpeed != 0)
			_customSpeed = fixedSpeed;
		if (Effect != null)
			Invoke ("InstantiateSuperEffect", _superEffectDelay);
		_ball.GetComponent<Rigidbody2D>().velocity = direction * _customSpeed;
	}

	private void ShadowThrow (Vector2 direction, CurrentPlayer throwingPlayer, float addedPower, float fixedSpeed = 0)
	{
		_shadow = Instantiate (_ball, _ball.transform.position, _ball.transform.rotation);
		_shadow.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 0.66f);
		_shadow.tag = "DiscShadow";
		_shadow.GetComponent<BallBehavior>().SetGravityScaleFromPower (addedPower);
		_shadow.GetComponent<BallBehavior>().IsThrownBy = throwingPlayer;
		_shadow.GetComponent<BallBehavior>().CurrentPlayer = CurrentPlayer.None;
		float speedQuarter = (_shadow.GetComponent<BallBehavior>().Speed + addedPower) / 4;
		_customSpeed = (_shadow.GetComponent<BallBehavior>().Speed + addedPower) - (Mathf.Abs(direction.x) * speedQuarter);
		_customSpeed = _customSpeed * 1.2f;
		if (fixedSpeed != 0)
			_customSpeed = fixedSpeed;
		_shadow.GetComponent<Rigidbody2D>().velocity = direction * _customSpeed;
	}

	private void InstantiateSuperEffect()
	{
		if (_ball == null || _ball.GetComponent<BallBehavior>().IsThrownBy == CurrentPlayer.None)
			return;
		Instantiate (Effect, _ball.transform.position, _ball.transform.rotation);
		Invoke ("InstantiateSuperEffect", _superEffectDelay);
	}

	private void InstantiateZigzag()
	{
		if (_ball == null || _ball.GetComponent<BallBehavior>().IsThrownBy == CurrentPlayer.None)
			return;
		if (_zigzag == 0.0f) {
			if (_currentThrowDirection.x == 0) {
				if (_ball.transform.position.x >= 0)
					_zigzag = -1;
				else
					_zigzag = 1;
			} else if (_currentThrowDirection.x > 0) {
				_zigzag = 1;
			} else {
				_zigzag = -1;
			}
		} else {
			_zigzag *= -1;
		}
		_ball.GetComponent<Rigidbody2D>().velocity = (_currentThrowDirection + new Vector2(_zigzag, 0.0f) ) * (_customSpeed * 0.8f);
		_zigzag = 0;
		//Invoke ("InstantiateZigzag", _zigzagDelay);
	}

	private bool StraightOnCollision ()
	{
		_ball.GetComponent<Rigidbody2D>().velocity = _playerThrowDirection * (_customSpeed * 1.2f);
		_ball.GetComponent<BallBehavior> ().onWallCollisionDelegate = null;
		return true;
	}

	private bool PanzerBounce()
	{
		if (_bounce == 0) {
			Physics2D.gravity = new Vector2 (0.0f, 0.0f);
			return true;
		} else {
			--_bounce;
			Physics2D.gravity = new Vector2 (0.0f, _gravity * _playerThrowDirection.y);
			_ball.transform.position = new Vector3 (_ball.transform.position.x, _bounceYPosition * _ball.transform.position.y > 0 ? 1.0f : -1.0f, 0.0f);
			_ball.GetComponent<Rigidbody2D>().velocity = new Vector2(_ball.transform.position.x > 0 ? -1.0f : 1.0f, _playerThrowDirection.y * -1.0f) * (_customSpeed / 2);
			return false;
		}
	}

	public void FreezeGoals()
	{
		string tmpGoalName = "Top";
		if (this.GetComponentInParent<PlayerBehavior>().Player == CurrentPlayer.PlayerOne)
			tmpGoalName = "Bot";

		List<int> availableGoals = new List<int>{};
		int nbGoals = 3;
		if (PlayerPrefs.GetInt ("SelectedMap") == 4)
			nbGoals = 2;
		for (int i = 1; i <= nbGoals; ++i)
		{
			if (GameObject.Find ("Goal" + tmpGoalName + i.ToString ("D2")).GetComponent<GoalBehavior> ().IsFrozen == false)
				availableGoals.Add (i);
		}

		if (availableGoals.Count == 0)
			return;

		var tmpNumber = Random.Range (0,availableGoals.Count);
		var tmpGoal = GameObject.Find ("Goal"+tmpGoalName+availableGoals[tmpNumber].ToString("D2")).GetComponent<GoalBehavior>();
		tmpGoal.Freeze ();
	}

	/*private void InstantiateNewDisc ()
	{
		_hasInstantiateNewDisc = true;
		var nbBalls = GameObject.FindGameObjectsWithTag ("Disc").Length;
		var tmpNextBall = Instantiate(WhiteBall, new Vector3(-3.0f, 0.0f, 0.0f), WhiteBall.transform.rotation);
		tmpNextBall.transform.name = "Ball"+nbBalls;
		tmpNextBall.GetComponent<BallBehavior> ().MapId = PlayerPrefs.GetInt ("SelectedMap");
		tmpNextBall.GetComponent<BallBehavior> ().CurrentPlayer = _currentPlayer.GetComponent<PlayerBehavior> ().Player;
		tmpNextBall.GetComponent<BallBehavior> ().IsNextBall = true;
		_currentPlayer.GetComponent<PlayerBehavior>().NextBalls.Add(tmpNextBall);
	}*/

	/*private void DisableInstantiate()
	{
		_hasInstantiateNewDisc = false;
	}*/

	private void ResetSuperEffectDelay()
	{
		_superEffectDelay = _baseSuperEffectDelay;
	}

	public void LaunchSupper(Vector2 direction, CurrentPlayer throwingPlayer, float addedPower, Vector2 playerThrowDirection, GameObject ball)
	{
		_ball = ball;
		_currentThrowDirection = direction;
		_playerThrowDirection = playerThrowDirection;
		//_currentPlayer = GameObject.Find (throwingPlayer.ToString());
		switch (Super)
		{
		case SuperType.Super01:
			direction = ExtremeFromCurrentDirection ();
			BasicEffectThrow (direction, throwingPlayer, addedPower);
			_ball.GetComponent<BallBehavior> ().onWallCollisionDelegate = StraightOnCollision;
			break;
		case SuperType.Super02:
			BasicEffectThrow (playerThrowDirection, throwingPlayer, addedPower);
			Invoke ("InstantiateZigzag", _zigzagDelay);
			break;
		case SuperType.Super03:
			BasicEffectThrow (direction, throwingPlayer, addedPower);
			_bounce = _bounceCount;
			_ball.GetComponent<BallBehavior> ().onPlayerCollisionDelegate = PanzerBounce;
			break;
		case SuperType.Super04:
			FreezeGoals ();
			BasicEffectThrow (direction, throwingPlayer, addedPower);
			break;
		case SuperType.Super05:
			_superEffectDelay = _superEffectDelay / 1.5f;
			BasicEffectThrow (direction, throwingPlayer, addedPower, 8);
			Invoke ("ResetSuperEffectDelay", 1.0f);
			break;
		case SuperType.Super06:
			//if (!_hasInstantiateNewDisc) InstantiateNewDisc();
			//_superEffectDelay = _superEffectDelay * 3;
			direction = OnlySideFromCurrentDirection ();
			BasicEffectThrow (direction, throwingPlayer, addedPower);
			ShadowThrow (new Vector2(direction.x * -1.0f, direction.y), throwingPlayer, addedPower);
			//Invoke ("DisableInstantiate", 1.0f);
			//Invoke ("ResetSuperEffectDelay", 5.0f);
			break;
		default :
			break;
		}
	}

	public enum SuperType
	{
		Super01 = 1,
		Super02 = 2,
		Super03 = 3,
		Super04 = 4,
		Super05 = 5,
		Super06 = 6
	}
}
