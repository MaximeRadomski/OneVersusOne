using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerBehavior : MonoBehaviour
{
    public GameObject Ball;

	void Start ()
	{
		PlaceBall(CurrentPlayer.PlayerOne.ToString());
	}

    private void PlaceBall(string playerName)
    {
        var currentPlayer = GameObject.Find(playerName);
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

    public void NewSet(CurrentPlayer Looser)
    {
        PlaceBall(Looser.ToString());
    }
}
