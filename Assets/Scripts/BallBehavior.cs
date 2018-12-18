using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    public float Speed;
    public bool IsLinkedToPlayerOne;
    public bool IsLinkedToPlayerTwo;
	public CurrentPlayer IsThrownBy;
	public Animator Animator;

    private GameObject _linkedPlayer;
    private GameObject _gameManager;
    private float _spaceYFromPlayer = 1.55f;
	private float _spaceXFromPlayer = 0.125f;
	private int _catchCount;

	void Start ()
	{
	    // Initial Velocity
	    //GetComponent<Rigidbody2D>().velocity = Vector2.up * Speed;
        _gameManager = GameObject.Find("$GameManager");
		_catchCount = -1; //-1 because the first collision counts when serving
    }

    void Update()
    {
		if (IsLinkedToPlayerOne || IsLinkedToPlayerTwo)
		{
			Animator.SetBool ("IsRotating", false);
			if (_linkedPlayer == null)
				_linkedPlayer = GetLinkedPlayer ();
			float spaceYFromPlayer = _linkedPlayer.GetComponent<PlayerBehavior> ().Player == CurrentPlayer.PlayerOne 
				? _spaceYFromPlayer * -1
				: _spaceYFromPlayer;
			float spaceXFromPlayer = _linkedPlayer.GetComponent<PlayerBehavior> ().Player == CurrentPlayer.PlayerOne 
				? _spaceXFromPlayer
				: _spaceXFromPlayer * -1;
			transform.position = new Vector3 (_linkedPlayer.transform.position.x + spaceXFromPlayer, spaceYFromPlayer);
		}
		else
			Animator.SetBool ("IsRotating", true);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            _linkedPlayer = col.gameObject;
            if (_linkedPlayer.GetComponent<PlayerBehavior>().Player == CurrentPlayer.PlayerOne)
                IsLinkedToPlayerOne = true;
            else
                IsLinkedToPlayerTwo = true;
            _linkedPlayer.GetComponent<PlayerBehavior>().HasTheDisc = true;
            _linkedPlayer.GetComponent<PlayerBehavior>().ThrowAngle = 0.0f;
			if (++_catchCount % 2 == 0 && _catchCount != 0) // "_catchcount != 0" because it starts at -1
				Speed += 0.5f;
        }
        else if (col.gameObject.tag == "Goal")
        {
			col.gameObject.GetComponent<GoalBehavior> ().GoalHit ();
            _gameManager.GetComponent<GameManagerBehavior>().NewSet(
                col.gameObject.GetComponent<GoalBehavior>().Player);
            Destroy(gameObject);
        }
		else if (col.gameObject.tag == "Wall")
		{
			col.gameObject.GetComponent<WallBehavior>().WallHit();
		}
    }

    private GameObject GetLinkedPlayer()
    {
        if (IsLinkedToPlayerOne == true)
            return GameObject.Find("PlayerOne");
        else if (IsLinkedToPlayerTwo == true)
            return GameObject.Find("PlayerTwo");
        return null;
    }

	public void Throw(Vector2 direction, CurrentPlayer throwingPlayer)
    {
		IsThrownBy = throwingPlayer;
        IsLinkedToPlayerOne = false;
        IsLinkedToPlayerTwo = false;
		float speedQuarter = Speed / 4;
		float customSpeed = Speed - (Mathf.Abs(direction.x) * speedQuarter);
        GetComponent<Rigidbody2D>().velocity = direction * customSpeed;
    }
}
