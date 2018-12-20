using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerBehavior : MonoBehaviour
{
    public GameObject Ball;
    public float DistanceWall;
    public bool IsGoal;

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
        var currentBall = Instantiate(Ball, new Vector3(0.0f, 0.0f, 0.0f), Ball.transform.rotation);
        currentBall.transform.name = "Ball";
		currentBall.GetComponent<BallBehavior> ().CurrentPlayer = currentPlayer.GetComponent<PlayerBehavior> ().Player;
		currentPlayer.GetComponent<PlayerBehavior>().GetTheDisc();
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
