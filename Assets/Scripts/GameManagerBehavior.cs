using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerBehavior : MonoBehaviour
{
    public GameObject Ball;
    public float DistanceWall;

    private string _playerName;

	void Start ()
	{
		_playerName = CurrentPlayer.PlayerOne.ToString();
		PlaceBall();
	}

    private void PlaceBall()
    {
		if (BallAlreadyExists())
			return;
        var currentPlayer = GameObject.Find(_playerName);
        float positionXMultiplier = 1.0f;
        if (currentPlayer.GetComponent<PlayerBehavior>().Player == CurrentPlayer.PlayerOne)
            positionXMultiplier = -1.0f;
        var currentBall = Instantiate(Ball, new Vector3(currentPlayer.transform.position.x, 2.0f * positionXMultiplier, 0.0f), Ball.transform.rotation);
        currentBall.transform.name = "Ball";
        if (currentPlayer.GetComponent<PlayerBehavior>().Player == CurrentPlayer.PlayerOne)
            currentBall.GetComponent<BallBehavior>().IsLinkedToPlayerOne = true;
        else
            currentBall.GetComponent<BallBehavior>().IsLinkedToPlayerTwo = true;
        currentPlayer.GetComponent<PlayerBehavior>().HasTheDisc = true;
    }

	public bool BallAlreadyExists()
	{
		if (GameObject.Find ("Ball") != null)
			return true;
		return false;
	}

    public void NewSet(CurrentPlayer Looser)
    {
		_playerName = Looser.ToString ();
		Invoke("PlaceBall", 1.0f);
    }
}
