using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public CurrentPlayer Player;
    public Vector2 DirectionalVector;
    public bool CanDash = true;
    public float DashCooldown;
    public float ThrowAngle;
	public Animator Animator;
	public BoxCollider2D BoxCollider;
	public SpriteRenderer CurrentSprite;
	public Sprite[] AngleSprites;
	public Sprite DashSprite;

	private Quaternion _initialRotation;
	private Vector3 _initialPosition;
	private bool _isGoingLeft, _isGoingRight, _hasTheDisc;

	public bool IsGoingLeft	{
		get	{
			return _isGoingLeft;
		}
		set {
			_isGoingLeft = value;
			SetOrientation ();
		}
	}

	public bool IsGoingRight {
		get	{
			return _isGoingRight;
		}
		set {
			_isGoingRight = value;
			SetOrientation ();
		}
	}

	public bool HasTheDisc {
		get {
			return _hasTheDisc;
		}
		set {
			_hasTheDisc = value;
			if (_hasTheDisc == true) {
				Animator.enabled = false;
				BoxCollider.enabled = false;
				CurrentSprite.sprite = AngleSprites [2];
			} else {
				Animator.enabled = true;
				BoxCollider.enabled = true;
			}
		}
	}

	void Start ()
	{
		_initialPosition = transform.position;
		_initialRotation = transform.rotation;
	}

	private void SetOrientation ()
	{
		if (_isGoingLeft) {
			transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f); 
			Animator.SetBool ("IsMoving", true);
		} else if (_isGoingRight) {
			transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f); 
			Animator.SetBool ("IsMoving", true);
		} else {
			transform.rotation = _initialRotation;
			Animator.SetBool ("IsMoving", false);
		}
	}

    public void IncrementAngle()
    {
        var tmpThrowAngle = ThrowAngle + 0.5f;
        if (tmpThrowAngle >= -1.5f && tmpThrowAngle <= 1.5f)
            ThrowAngle = tmpThrowAngle;
		SetSpriteFromAngle ();
    }

    public void DecrementAngle()
    {
        var tmpThrowAngle = ThrowAngle - 0.5f;
        if (tmpThrowAngle >= -1.5f && tmpThrowAngle <= 1.5f)
            ThrowAngle = tmpThrowAngle;
		SetSpriteFromAngle ();
    }

	private void SetSpriteFromAngle ()
	{
		if (ThrowAngle <= -1.0f)
			CurrentSprite.sprite = AngleSprites [0];
		else if (ThrowAngle == -0.5f)
			CurrentSprite.sprite = AngleSprites [1];
		else if (ThrowAngle == 0.0f)
			CurrentSprite.sprite = AngleSprites [2];
		else if (ThrowAngle == 0.5f)
			CurrentSprite.sprite = AngleSprites [3];
		else if (ThrowAngle >= 1.5f)
			CurrentSprite.sprite = AngleSprites [4];
	}

    public void Dash()
    {
        CanDash = false;
		Animator.enabled = false;
		CurrentSprite.sprite = DashSprite;
        Invoke("ResetDash", DashCooldown);
    }

    private void ResetDash()
    {
        CanDash = true;
		if (HasTheDisc == false) {
			IsGoingLeft = false;
			IsGoingRight = false;
			Animator.enabled = true;
			Animator.Play("Player01Idle");
		}
    }

	public void Throw()
	{
		_hasTheDisc = false;
		//Animator.SetBool ("IsThrowing", true);
		Animator.enabled = true;
        Animator.Play("Player01Throw");
		Invoke ("ResetThrow", 0.4f);
	}

	private void ResetThrow()
	{
		//Animator.SetBool ("IsThrowing", false);
		HasTheDisc = false;
	    Animator.Play("Player01Idle");
    }

	public void ResetInitialPosition()
	{
		transform.position = _initialPosition;
	}

}
