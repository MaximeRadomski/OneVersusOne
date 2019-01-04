using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerBehavior : MonoBehaviour
{
    public GameObject Ball;
    public float DistanceWall;
    public bool IsGoal;
	public int ScorePlayerOne, ScorePlayerTwo;
	public int SetPlayerOne, SetPlayerTwo;

    private string _playerName;
	private GameObject _scoreP1, _scoreP2;
	private GameObject _scoreP1_1, _scoreP1_2, _scoreP2_1, _scoreP2_2;
	private GameObject _playerOne, _playerTwo;

	void Start ()
	{
		ScorePlayerOne = 0;
		ScorePlayerTwo = 0;
		SetPlayerOne = 0;
		SetPlayerTwo = 0;
		_playerName = CurrentPlayer.PlayerOne.ToString();
		_scoreP1 = GameObject.Find ("ScoreP1");
		_scoreP2 = GameObject.Find ("ScoreP2");
		_scoreP1_1 = GameObject.Find ("ScoreP1-1");
		_scoreP1_2 = GameObject.Find ("ScoreP1-2");
		_scoreP2_1 = GameObject.Find ("ScoreP2-1");
		_scoreP2_2 = GameObject.Find ("ScoreP2-2");
		_playerOne = GameObject.Find ("PlayerOne");
		_playerTwo = GameObject.Find ("PlayerTwo");
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
		_scoreP1.GetComponent<Animator> ().Play ("StartingState");
		_scoreP2.GetComponent<Animator> ().Play ("StartingState");
		_playerOne.GetComponent<PlayerBehavior> ().IsControlledByAI = false;
		_playerTwo.GetComponent<PlayerBehavior> ().IsControlledByAI = false;
    }

	public bool BallAlreadyExists()
	{
		if (GameObject.Find ("Ball") != null)
			return true;
		return false;
	}

	private void CheckIfSet()
	{
		bool reset = false;
		if (ScorePlayerOne >= 12) {
			++SetPlayerOne;
			reset = true;
		} else if (ScorePlayerTwo >= 12) {
			++SetPlayerTwo;
			reset = true;
		}

		if (reset) {
			ScorePlayerOne = 0;
			ScorePlayerTwo = 0;
			ChangeAllScores ();
		}

		PlaceBall ();
	}

	public void NewBall(CurrentPlayer looser, int points)
    {
		_playerName = looser.ToString ();
		if (looser == CurrentPlayer.PlayerOne)
		{
			ScorePlayerTwo += points;
			_scoreP1.transform.GetChild (3).GetComponent<ScoreBackgroundBehavior> ().Loose();
			_scoreP2.transform.GetChild (3).GetComponent<ScoreBackgroundBehavior> ().Win();
		}
		else
		{
			ScorePlayerOne += points;
			_scoreP2.transform.GetChild (3).GetComponent<ScoreBackgroundBehavior> ().Loose();
			_scoreP1.transform.GetChild (3).GetComponent<ScoreBackgroundBehavior> ().Win();
		}
		Invoke("DisplayScores", 0.5f);
		Invoke("CheckIfSet", 3.0f);
    }

	private void DisplayScores()
	{
		_playerOne.GetComponent<PlayerBehavior> ().Recenter ();
		_playerTwo.GetComponent<PlayerBehavior> ().Recenter ();
		_scoreP1.GetComponent<Animator> ().Play ("DisplayScore01");
		_scoreP2.GetComponent<Animator> ().Play ("DisplayScore02");
		Invoke ("ChangeAllScores", 0.75f);
	}

	private void ChangeAllScores()
	{
		ChangeScore (ScorePlayerOne, _scoreP1_1);
		ChangeScore (ScorePlayerTwo, _scoreP1_2);
		ChangeScore (ScorePlayerOne, _scoreP2_1);
		ChangeScore (ScorePlayerTwo, _scoreP2_2);
	}

	private void ChangeScore(int score, GameObject scoreGameobject)
	{
		scoreGameobject.transform.GetChild (0).GetComponent<UnityEngine.UI.Text> ().text = GetFormatedString(score, '0');
		scoreGameobject.transform.GetChild (1).GetComponent<UnityEngine.UI.Text> ().text = GetFormatedString(score, 'A');
		scoreGameobject.transform.GetChild (2).GetComponent<UnityEngine.UI.Text> ().text = GetFormatedString(score, 'K');
		scoreGameobject.transform.GetChild (3).GetComponent<UnityEngine.UI.Text> ().text = GetFormatedString(score, 'a');
		scoreGameobject.transform.GetChild (4).GetComponent<UnityEngine.UI.Text> ().text = GetFormatedString(score, 'k');
	}

	private string GetFormatedString(int score, char baseCharacter)
	{
		string tmpStr = "";
		int tmpScore = score;
		bool firstZero = score == 0 ? true : false;
		while (tmpScore != 0 || firstZero)
		{
			int digit = tmpScore % 10;
			int tmpIntChar = (int)baseCharacter + digit;
			char c = (char)tmpIntChar;
			tmpStr = c.ToString () + tmpStr;
			tmpScore = tmpScore / 10;
			firstZero = false;
		}
		return tmpStr;
	}
}
