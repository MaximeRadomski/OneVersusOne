﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public CurrentPlayer Player;
    public Direction Direction;
	public Direction LiftDirection;
	public Color SuperColor;

    public bool IsDashing;
    public bool HasTheDisc;
	public bool IsControlledByAI;
	public bool IsCastingSP;
	public bool IsDoingSP;
	public bool IsEngaging;
	public ControlledAction ControlledAction;

    public float WalkDistance;
    public float DashDistance;
    public float Power;
	public int SPMaxCooldown;
	public int SPCooldown;

	public GameObject Ball;
    public Animator Animator;
	public BoxCollider2D BoxCollider;
	public SpriteRenderer CurrentSprite;
	public Sprite[] AngleSprites;
	public Sprite DashSprite;
	public Sprite SPSprite;
    public GameObject DashEffect;
    public GameObject DashEffectParticles;
	public GameObject CatchEffect;
	public GameObject CastSPEffect;

    private Quaternion _initialRotation;
	private Vector3 _initialPosition;
    private Vector2 _directionalVector;
    private float _throwAngle;
    private GameObject _gameManager;
    private Vector3 _dashingStart;
    private Vector3 _dashingEnd;
    private Direction _dashingDirection;
    private float _dashCooldown;
	private float _castSPCooldown;
    private bool _canDash;

    void Start ()
	{
		HasTheDisc = false;
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
	    //Ball = GetBall();
        _dashingDirection = Direction.Standby;
	    _dashCooldown = 0.75f;
		_castSPCooldown = 1.0f;
	    _canDash = true;
		SPCooldown = SPMaxCooldown;
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
		var ballTab = GameObject.FindGameObjectsWithTag ("Disc");
		int ballIndex = -1;
		for (int i = 0; i < ballTab.Length; ++i)
		{
			if (ballTab [i].GetComponent<BallBehavior> ().CurrentPlayer == Player)
			{
				ballIndex = i;
				break;
			}
		}
		if (ballIndex == -1)
			return null;
		return ballTab[ballIndex];
    }

    public void Move(Direction direction)
    {
		if (HasTheDisc || IsDashing)
		{
			LiftDirection = direction;
			return;
		}

		if (IsCastingSP)
			return;

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
		if (!_canDash || Direction == Direction.Standby || IsCastingSP)
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
		//Ball = GetBall ();
		if (Ball == null)
			return;
		if (Ball.GetComponent<BallBehavior>().CatchCount >= 0) {
			var tmpCatchEffect = Instantiate(CatchEffect, transform.position, CatchEffect.transform.rotation);
			if (Player == CurrentPlayer.PlayerTwo)
				tmpCatchEffect.transform.eulerAngles = new Vector3 (0.0f, 0.0f, 180.0f);
			SPCooldown = SPCooldown - 1 <= 0 ? 0 : SPCooldown - 1;
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
		if (IsCastingSP)
			IsDoingSP = true;
	}

	public void Super()
	{
		if (!HasTheDisc)
		{
			return;
		}
		SPCooldown = SPMaxCooldown;
		Invoke("SuperAfterDelay", 0.15f);
		Animator.enabled = true;
		Animator.Play("Throw");
		Invoke ("ResetThrow", 0.4f);
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
		{
			if (!IsCastingSP && !IsDashing)
				CastSP();
			return;
		}
		Invoke("LiftBallAfterDelay", 0.15f);
		Animator.enabled = true;
		Animator.Play("Throw");
		Invoke ("ResetThrow", 0.4f);
	}

	private void ResetThrow()
	{
	    HasTheDisc = false;
        Animator.Play("Idle");
		IsDoingSP = false;
		IsEngaging = false;
    }

    private void ThrowBallAfterDelay()
    {
		AndroidNativeAudio.play(_gameManager.GetComponent<GameManagerBehavior>().ThrowAudioFileID);
        //Ball = GetBall();
		if (Ball != null && Ball.GetComponent<BallBehavior> ().IsThrownBy != CurrentPlayer.None)
			return;
        if (Ball != null)
            Ball.GetComponent<BallBehavior>().Throw(_directionalVector + new Vector2(_throwAngle, 0.0f), Player, Power, false);
        _throwAngle = 0;
    }

	private void LiftBallAfterDelay()
	{
		AndroidNativeAudio.play(_gameManager.GetComponent<GameManagerBehavior>().ThrowAudioFileID);
		//Ball = GetBall();
		if (Ball != null && Ball.GetComponent<BallBehavior> ().IsThrownBy != CurrentPlayer.None)
			return;
		if (Ball != null)
			Ball.GetComponent<BallBehavior>().Throw(_directionalVector + new Vector2(_throwAngle, 0.0f), Player, Power, true);
		_throwAngle = 0;
	}

	private void SuperAfterDelay()
	{
		AndroidNativeAudio.play(_gameManager.GetComponent<GameManagerBehavior>().ThrowAudioFileID);
		//Ball = GetBall();
		if (Ball != null && Ball.GetComponent<BallBehavior> ().IsThrownBy != CurrentPlayer.None)
			return;
		if (Ball != null)
			this.GetComponent<SuperBehavior>().LaunchSupper(_directionalVector + new Vector2(_throwAngle, 0.0f), Player, Power, _directionalVector);
		_throwAngle = 0;
		var tmpSpEffect = Instantiate(CastSPEffect, transform.position, transform.rotation);
		tmpSpEffect.GetComponent<SpriteRenderer> ().color = SuperColor;
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

	private void CastSP()
	{
		if (IsEngaging || SPCooldown > 0)
			return;
		IsCastingSP = true;
		Invoke ("ResetCastSP", _castSPCooldown);

		Direction = Direction.Standby;
		SetOrientation ();
		Animator.enabled = false;
		CurrentSprite.sprite = SPSprite;
		Instantiate(CastSPEffect, transform.position, transform.rotation);
	}

	private void ResetCastSP()
	{
		IsCastingSP = false;
		if (!HasTheDisc)
		{
			Animator.enabled = true;
			Animator.Play("Idle");
		}
	}

	void OnDestroy()
	{
	}
}
