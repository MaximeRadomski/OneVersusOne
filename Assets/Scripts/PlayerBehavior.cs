using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public CurrentPlayer Player;
    public Direction Direction;

    public bool IsDashing;
    public bool HasTheDisc;

    public float DashCooldown;
    public float WalkDistance;
    public float DashDistance;
    public float Power;

    public Animator Animator;
	public BoxCollider2D BoxCollider;
	public SpriteRenderer CurrentSprite;
	public Sprite[] AngleSprites;
	public Sprite DashSprite;

	private Quaternion _initialRotation;
	private Vector3 _initialPosition;
    private Vector2 _directionalVector;
    private float _throwAngle;
    private GameObject _gameManager;
    private GameObject _ball;
    private Vector3 _dashingEnd;

	void Start ()
	{
		_initialPosition = transform.position;
		_initialRotation = transform.rotation;
        if (Player == CurrentPlayer.PlayerOne)
            _directionalVector = Vector2.up;
        else
            _directionalVector = Vector2.down;
	    _throwAngle = 0;
        _gameManager = GameObject.Find("$GameManager");
	    _ball = GetBall();
	}

    void Update()
    {
        if (IsDashing)
        {
            if (HasTheDisc)
                ResetDash();

            float distance = WalkDistance;
            if (Direction == Direction.Left)
                distance = -distance;

            SetOrientation();
            if (Vector3.Distance(transform.position, _dashingEnd) > 0.1f)
            {
                transform.position += new Vector3(distance * 3.0f, 0.0f, 0.0f);
            }
            else
            {
                IsDashing = false;
            }

            /*_dashingEnd = transform.position + new Vector3(distance, 0.0f, 0.0f);
            if (_dashingEnd.x < -_gameManager.GetComponent<GameManagerBehavior>().DistanceWall)
                _dashingEnd = new Vector3(-_gameManager.GetComponent<GameManagerBehavior>().DistanceWall, _dashingEnd.y, 0.0f);
            if (_dashingEnd.x > _gameManager.GetComponent<GameManagerBehavior>().DistanceWall)
                _dashingEnd = new Vector3(_gameManager.GetComponent<GameManagerBehavior>().DistanceWall, _dashingEnd.y, 0.0f);*/
        }
    }

    private GameObject GetBall()
    {
        return GameObject.Find("Ball");
    }

    public void Move(Direction direction)
    {
        if (HasTheDisc || IsDashing)
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
        if (tmpThrowAngle >= -1.5f && tmpThrowAngle <= 1.5f)
            _throwAngle = tmpThrowAngle;
		SetSpriteFromAngle ();
    }

    public void DecrementAngle()
    {
        if (!HasTheDisc)
            return;
        var tmpThrowAngle = _throwAngle - 0.5f;
        if (tmpThrowAngle >= -1.5f && tmpThrowAngle <= 1.5f)
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
		else if (_throwAngle >= 1.5f)
			CurrentSprite.sprite = AngleSprites [4];
	}

    public void Dash()
    {
        if (IsDashing || Direction == Direction.Standby)
            return;

        IsDashing = true;
        float distance = DashDistance;
        if (Direction == Direction.Left)
            distance = -distance;

		SetOrientation ();
        _dashingEnd = transform.position + new Vector3(distance, 0.0f, 0.0f);
        if (_dashingEnd.x < -_gameManager.GetComponent<GameManagerBehavior>().DistanceWall)
            _dashingEnd = new Vector3(-_gameManager.GetComponent<GameManagerBehavior>().DistanceWall, _dashingEnd.y, 0.0f);
        if (_dashingEnd.x > _gameManager.GetComponent<GameManagerBehavior>().DistanceWall)
            _dashingEnd = new Vector3(_gameManager.GetComponent<GameManagerBehavior>().DistanceWall, _dashingEnd.y, 0.0f);

		Animator.enabled = false;
		CurrentSprite.sprite = DashSprite;
        Invoke("ResetDash", DashCooldown);
    }

    private void ResetDash()
    {
        IsDashing = false;
        Direction = Direction.Standby;
		SetOrientation ();
        if (HasTheDisc == false)
		{
			Animator.enabled = true;
			Animator.Play("Player01Idle");
		}
    }

	public void GetTheDisc()
	{
		HasTheDisc = true;
		_throwAngle = 0.0f;
		Animator.enabled = false;
		CurrentSprite.sprite = AngleSprites [2];
		Direction = Direction.Standby;
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
        Animator.Play("Player01Throw");
		Invoke ("ResetThrow", 0.4f);
	}

	private void ResetThrow()
	{
	    HasTheDisc = false;
        Animator.Play("Player01Idle");
    }

    private void ThrowBallAfterDelay()
    {
        if (_ball == null)
            _ball = GetBall();
        _ball.GetComponent<BallBehavior>().Throw(_directionalVector + new Vector2(_throwAngle, 0.0f), Player, Power);
        _throwAngle = 0;
    }

    public void ResetInitialPosition()
	{
		transform.position = _initialPosition;
	}

}
