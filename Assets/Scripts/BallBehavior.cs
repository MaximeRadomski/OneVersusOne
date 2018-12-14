using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    public float Speed;
    public bool IsLinkedToPlayerOne;
    public bool IsLinkedToPlayerTwo;

    private GameObject _linkedPlayer;
    private GameObject _gameManager;
    private float _spaceFromPlayer = 2.0f;

	void Start ()
	{
	    // Initial Velocity
	    //GetComponent<Rigidbody2D>().velocity = Vector2.up * Speed;
        _gameManager = GameObject.Find("$GameManager");
    }

    void Update()
    {
        if (IsLinkedToPlayerOne || IsLinkedToPlayerTwo)
        {
            if (_linkedPlayer == null)
                _linkedPlayer = GetLinkedPlayer();
            float spaceFromPlayer =
                _linkedPlayer.GetComponent<PlayerBehavior>().Player == CurrentPlayer.PlayerOne
                    ? _spaceFromPlayer * -1
                    : _spaceFromPlayer;
            transform.position = new Vector3(_linkedPlayer.transform.position.x, spaceFromPlayer);
        }
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
        }
        else if (col.gameObject.tag == "Goal")
        {
            _gameManager.GetComponent<GameManagerBehavior>().NewSet(
                col.gameObject.GetComponent<GoalBehavior>().Player);
            Destroy(gameObject);
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

    public void Throw(Vector2 direction)
    {
        IsLinkedToPlayerOne = false;
        IsLinkedToPlayerTwo = false;
        GetComponent<Rigidbody2D>().velocity = direction * Speed;
    }
}
