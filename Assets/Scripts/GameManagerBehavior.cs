using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerBehavior : MonoBehaviour
{
    public GameObject Ball;
    public float DistanceWall;
    public bool IsGoal;
	public int ScorePlayerOne;
	public int ScorePlayerTwo;

    private string _playerName;
	private GameObject _scoreP1_1, _scoreP1_2, _scoreP2_1, _scoreP2_2;

	void Start ()
	{
		ScorePlayerOne = 0;
		ScorePlayerTwo = 0;
		_playerName = CurrentPlayer.PlayerOne.ToString();
		_scoreP1_1 = GameObject.Find ("ScoreP1-1");
		_scoreP1_2 = GameObject.Find ("ScoreP1-2");
		_scoreP2_1 = GameObject.Find ("ScoreP2-1");
		_scoreP2_2 = GameObject.Find ("ScoreP2-2");
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
		currentPlayer.GetComponent<PlayerBehavior>().CatchTheDisc();
    }

	public bool BallAlreadyExists()
	{
		if (GameObject.Find ("Ball") != null)
			return true;
		return false;
	}

	public void NewSet(CurrentPlayer looser, int points)
    {
		_playerName = looser.ToString ();
		if (looser == CurrentPlayer.PlayerOne)
			ScorePlayerTwo += points;
		else
			ScorePlayerOne += points;
		DisplayScores();
		Invoke("PlaceBall", 1.0f);
    }

	private void DisplayScores()
	{
		ChangeScores (ScorePlayerOne, _scoreP1_1);
		ChangeScores (ScorePlayerTwo, _scoreP1_2);
		ChangeScores (ScorePlayerOne, _scoreP2_1);
		ChangeScores (ScorePlayerTwo, _scoreP2_2);
	}

	private void ChangeScores(int score, GameObject scoreGameobject)
	{
		if (score != 0)
		{
			scoreGameobject.transform.GetChild (0).GetComponent<UnityEngine.UI.Text> ().text = GetFormatedString(score, '0');
			scoreGameobject.transform.GetChild (1).GetComponent<UnityEngine.UI.Text> ().text = GetFormatedString(score, 'A');
			scoreGameobject.transform.GetChild (2).GetComponent<UnityEngine.UI.Text> ().text = GetFormatedString(score, 'K');
			scoreGameobject.transform.GetChild (3).GetComponent<UnityEngine.UI.Text> ().text = GetFormatedString(score, 'a');
			scoreGameobject.transform.GetChild (4).GetComponent<UnityEngine.UI.Text> ().text = GetFormatedString(score, 'k');
		}
	}

	private string GetFormatedString(int score, char baseCharacter)
	{
		string tmpStr = "";
		int tmpScore = score;
		while (tmpScore != 0)
		{
			int digit = tmpScore % 10;
			int tmpIntChar = (int)baseCharacter + digit;
			char c = (char)tmpIntChar;
			tmpStr = c.ToString () + tmpStr;
			tmpScore = tmpScore / 10;
		}
		return tmpStr;
	}
}
