﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public CurrentPlayer Player;
    public Direction Direction;
	public Direction LiftDirection;

    public bool IsDashing;
    public bool HasTheDisc;
	public bool IsControlledByAI;
	public ControlledAction ControlledAction;

    public float WalkDistance;
    public float DashDistance;
    public float Power;

	public AudioSource ThrowAudio;

    public Animator Animator;
	public BoxCollider2D BoxCollider;
	public SpriteRenderer CurrentSprite;
	public Sprite[] AngleSprites;
	public Sprite DashSprite;
    public GameObject DashEffect;
    public GameObject DashEffectParticles;
	public GameObject CatchEffect;

    private Quaternion _initialRotation;
	private Vector3 _initialPosition;
    private Vector2 _directionalVector;
    private float _throwAngle;
    private GameObject _gameManager;
    private GameObject _ball;
    private Vector3 _dashingStart;
    private Vector3 _dashingEnd;
    private Direction _dashingDirection;
    private float _dashCooldown;
    private bool _canDash;

    void Start ()
	{
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
	    _ball = GetBall();
        _dashingDirection = Direction.Standby;
	    _dashCooldown = 0.75f;
	    _canDash = true;
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
        return GameObject.Find("Ball");
    }

    public void Move(Direction direction)
    {
		if (HasTheDisc || IsDashing)
		{
			LiftDirection = direction;
			return;
		}

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

    public void Dash()
    {
        if (!_canDash || Direction == Direction.Standby)
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
		if (_ball == null)
			_ball = GetBall ();
		if (_ball.GetComponent<BallBehavior>().CatchCount >= 0) {
			var tmpCatchEffect = Instantiate(CatchEffect, transform.position, CatchEffect.transform.rotation);
			if (Player == CurrentPlayer.PlayerTwo)
				tmpCatchEffect.transform.eulerAngles = new Vector3 (0.0f, 0.0f, 180.0f);
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
	}

	public void Throw()
	{
		if (!HasTheDisc)
	    {
	        Dash();
	        return;
	    }
	    Invoke("ThrowBallAfterDelay", 0.15f);
		Animator.enabled = true;
        Animator.Play("Throw");
		Invoke ("ResetThrow", 0.4f);
	}

	public void Lift()
	{
		if (!HasTheDisc)
			return;
		Invoke("LiftBallAfterDelay", 0.15f);
		Animator.enabled = true;
		Animator.Play("Throw");
		Invoke ("ResetThrow", 0.4f);
	}

	private void ResetThrow()
	{
	    HasTheDisc = false;
        Animator.Play("Idle");
    }

    private void ThrowBallAfterDelay()
    {
		ThrowAudio.Play ();
        if (_ball == null)
            _ball = GetBall();
		if (_ball != null && _ball.GetComponent<BallBehavior> ().IsThrownBy != CurrentPlayer.None)
			return;
        if (_ball != null)
            _ball.GetComponent<BallBehavior>().Throw(_directionalVector + new Vector2(_throwAngle, 0.0f), Player, Power, false);
        _throwAngle = 0;
    }

	private void LiftBallAfterDelay()
	{
		ThrowAudio.Play ();
		if (_ball == null)
			_ball = GetBall();
		if (_ball.GetComponent<BallBehavior> ().IsThrownBy != CurrentPlayer.None)
			return;
		_ball.GetComponent<BallBehavior>().Throw(_directionalVector + new Vector2(_throwAngle, 0.0f), Player, Power, true);
		_throwAngle = 0;
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

}