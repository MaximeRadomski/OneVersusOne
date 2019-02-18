﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperBehavior : MonoBehaviour
{
	public SuperType Super;
	public GameObject Effect;

	private GameObject _ball;
	private float _superEffectDelay;
	private float _zigzagDelay;
	private Vector2 _currentThrowDirection;
	private Vector2 _playerThrowDirection;
	private float _customSpeed;
	private float _zigzag;
	private int _bounceCount;
	private int _bounce;
	private float _gravity;

	void Start()
	{
		_superEffectDelay = 0.05f;
		_zigzagDelay = 0.25f;
		_zigzag = 0.0f;
		_bounceCount = 1;
		_bounce = 0;
		_gravity = 15.0f;
	}

	private void BasicEffectThrow (Vector2 direction, CurrentPlayer throwingPlayer, float addedPower)
	{
		_ball = GameObject.Find("Ball");	
		_ball.GetComponent<BallBehavior>().SetGravityScaleFromPower (addedPower);
		_ball.GetComponent<BallBehavior>().IsThrownBy = throwingPlayer;
		_ball.GetComponent<BallBehavior>().CurrentPlayer = CurrentPlayer.None;
		float speedQuarter = (_ball.GetComponent<BallBehavior>().Speed + addedPower) / 4;
		_customSpeed = (_ball.GetComponent<BallBehavior>().Speed + addedPower) - (Mathf.Abs(direction.x) * speedQuarter);
		_customSpeed = _customSpeed * 1.2f;
		if (Effect != null)
			Invoke ("InstantiateSuperEffect", _superEffectDelay);
		_ball.GetComponent<Rigidbody2D>().velocity = direction * _customSpeed;
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
		_ball.GetComponent<Rigidbody2D>().velocity = _playerThrowDirection * (_customSpeed * 1.5f);
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
			return false;
		}
	}

	public void LaunchSupper(Vector2 direction, CurrentPlayer throwingPlayer, float addedPower, Vector2 playerThrowDirection)
	{
		_currentThrowDirection = direction;
		_playerThrowDirection = playerThrowDirection;
		switch (Super)
		{
		case SuperType.Super01:
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
