using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    public float Speed;
    public bool IsLinkedToPlayerOne;
    public bool IsLinkedToPlayerTwo;

    private GameObject _linkedPlayer;
    private float _spaceFromPlayer = 2.0f;

	void Start ()
	{
	    // Initial Velocity
	    GetComponent<Rigidbody2D>().velocity = Vector2.up * Speed;
    }

    void Update()
    {
        if (IsLinkedToPlayerOne || IsLinkedToPlayerTwo)
        {
            float spaceFromPlayer =
                _linkedPlayer.GetComponent<PlayerBehavior>().Player == PlayerBehavior.CurrentPlayer.PlayerOne
                    ? _spaceFromPlayer * -1
                    : _spaceFromPlayer;
            transform.position = new Vector3(_linkedPlayer.transform.position.x, spaceFromPlayer);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Note: 'col' holds the collision information. If the
        // Ball collided with a racket, then:
        //   col.gameObject is the racket
        //   col.transform.position is the racket's position
        //   col.collider is the racket's collider

        if (col.gameObject.tag == "Player")
        {
            _linkedPlayer = col.gameObject;
            if (_linkedPlayer.GetComponent<PlayerBehavior>().Player == PlayerBehavior.CurrentPlayer.PlayerOne)
                IsLinkedToPlayerOne = true;
            else
                IsLinkedToPlayerTwo = true;
        }
    }

    public void Throw(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().velocity = direction * Speed;
    }
}
