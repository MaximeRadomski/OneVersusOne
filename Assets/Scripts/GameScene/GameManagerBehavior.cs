﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerBehavior : MonoBehaviour
{
    public GameObject Ball;
    public float DistanceWall;
    public bool IsGoal;
	public int ScorePlayerOne, ScorePlayerTwo;
	public int SetPlayerOne, SetPlayerTwo;

	public GameObject[] Characters;

	public AudioSource StageMusic;
	public AudioSource PointAudio;
	public AudioSource SetAudio;
	public AudioSource SlideAudio;

    private string _playerName;
	private GameObject _scoreP1, _scoreP2;
	private GameObject _scoreP1_1, _scoreP1_2, _scoreP2_1, _scoreP2_2;
	private GameObject _setP1, _setP2;
	private GameObject _setP1_1, _setP1_2, _setP2_1, _setP2_2;
	private GameObject _winner, _loser;
	private GameObject _playerOne, _playerTwo;

	void Start ()
	{
		StageMusic.Play ();
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

		_setP1 = GameObject.Find ("SetP1");
		_setP2 = GameObject.Find ("SetP2");
		_setP1_1 = GameObject.Find ("SetP1-1");
		_setP1_2 = GameObject.Find ("SetP1-2");
		_setP2_1 = GameObject.Find ("SetP2-1");
		_setP2_2 = GameObject.Find ("SetP2-2");

		_winner = GameObject.Find ("Winner");
		_loser = GameObject.Find ("Loser");

		//_playerOne = GameObject.Find ("PlayerOne");
		//_playerTwo = GameObject.Find ("PlayerTwo");

		_playerOne = CreateCharacter(CurrentPlayer.PlayerOne, 1);
		_playerTwo = CreateCharacter(CurrentPlayer.PlayerTwo, 2);

		PlaceBall();
	}

	private GameObject CreateCharacter(CurrentPlayer player, int playerNb)
	{
		int characterNb = PlayerPrefs.GetInt ("P" + playerNb + "Character");
		int multiplier = player == CurrentPlayer.PlayerOne ? -1 : 1;
		bool rotation = player == CurrentPlayer.PlayerOne ? false : true;

		var tmpPlayer = Instantiate (Characters[characterNb - 1], new Vector3(0.0f, 1.805f * multiplier, 0.0f), Characters[characterNb - 1].transform.rotation);
		if (rotation)
			tmpPlayer.transform.eulerAngles = new Vector3 (0.0f, 0.0f, 180.0f);
		tmpPlayer.transform.name = player.ToString ();
		tmpPlayer.GetComponent<PlayerBehavior> ().Player = player;
		tmpPlayer.GetComponent<AI> ().Player = player;
		return tmpPlayer;
	}

    private void PlaceBall()
    {
		if (BallAlreadyExists())
			return;
        var currentPlayer = GameObject.Find(_playerName);
        var currentBall = Instantiate(Ball, new Vector3(0.0f, 0.0f, 0.0f), Ball.transform.rotation);
        currentBall.transform.name = "Ball";
		currentBall.GetComponent<BallBehavior> ().CurrentPlayer = currentPlayer.GetComponent<PlayerBehavior> ().Player;
		//currentPlayer.GetComponent<PlayerBehavior>().CatchTheDisc();
		_scoreP1.GetComponent<Animator> ().Play ("StartingState");
		_scoreP2.GetComponent<Animator> ().Play ("StartingState");
		_setP1.GetComponent<Animator> ().Play ("StartingState");
		_setP2.GetComponent<Animator> ().Play ("StartingState");
		_winner.GetComponent<Animator> ().Play ("StartingState");
		_loser.GetComponent<Animator> ().Play ("StartingState");
		_playerOne.GetComponent<PlayerBehavior> ().IsControlledByAI = false;
		_playerTwo.GetComponent<PlayerBehavior> ().IsControlledByAI = false;
    }

	public bool BallAlreadyExists()
	{
		if (GameObject.Find ("Ball") != null)
			return true;
		return false;
	}

	private void EndGame()
	{
		SceneManager.LoadScene("CharSelScene");
	}

	private void CheckIfGame()
	{
		bool gameEnd = false;

		if (SetPlayerOne >= 2) {
			_winner.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
			_loser.transform.eulerAngles  = new Vector3(0.0f, 0.0f, 180.0f);
			_winner.GetComponent<Animator> ().Play ("DisplayFromBottom");
			_loser.GetComponent<Animator> ().Play ("DisplayFromTop");
			gameEnd = true;
		} else if (SetPlayerTwo >= 2) {
			_winner.transform.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
			_loser.transform.eulerAngles  = new Vector3(0.0f, 0.0f, 0.0f);
			_winner.GetComponent<Animator> ().Play ("DisplayFromTop");
			_loser.GetComponent<Animator> ().Play ("DisplayFromBottom");
			gameEnd = true;
		}

		if (gameEnd == true)
		{
			SetPlayerOne = 0;
			SetPlayerTwo = 0;
			ChangeAllSets ();
			Invoke("EndGame", 8.0f);
		}
		else
			PlaceBall ();
	}

	private void CheckIfSet()
	{
		bool reset = false;

		if (ScorePlayerOne >= 12) {
			++SetPlayerOne;
			_setP1.transform.GetChild (4).GetComponent<ScoreBackgroundBehavior> ().Win();
			_setP2.transform.GetChild (4).GetComponent<ScoreBackgroundBehavior> ().Loose();
			reset = true;
		} else if (ScorePlayerTwo >= 12) {
			++SetPlayerTwo;
			_setP1.transform.GetChild (4).GetComponent<ScoreBackgroundBehavior> ().Loose();
			_setP2.transform.GetChild (4).GetComponent<ScoreBackgroundBehavior> ().Win();
			reset = true;
		}

		if (reset) {
			ScorePlayerOne = 0;
			ScorePlayerTwo = 0;
			ChangeAllScores ();
			DisplaySets();
			Invoke("CheckIfGame", 3.0f);
		}
		else
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
		SlideAudio.Play ();
		_playerOne.GetComponent<PlayerBehavior> ().Recenter ();
		_playerTwo.GetComponent<PlayerBehavior> ().Recenter ();
		_scoreP1.GetComponent<Animator> ().Play ("DisplayScore01");
		_scoreP2.GetComponent<Animator> ().Play ("DisplayScore02");
		Invoke ("ChangeAllScores", 0.75f);
	}

	private void DisplaySets()
	{
		SlideAudio.Play ();
		_setP1.GetComponent<Animator> ().Play ("DisplayScore01");
		_setP2.GetComponent<Animator> ().Play ("DisplayScore02");
		Invoke ("ChangeAllSets", 0.75f);
	}

	private void ChangeAllScores()
	{
		if (ScorePlayerOne > 0 || ScorePlayerTwo > 0)
			PointAudio.Play ();
		ChangeScore (ScorePlayerOne, _scoreP1_1);
		ChangeScore (ScorePlayerTwo, _scoreP1_2);
		ChangeScore (ScorePlayerOne, _scoreP2_1);
		ChangeScore (ScorePlayerTwo, _scoreP2_2);
	}

	private void ChangeAllSets()
	{
		if (SetPlayerOne > 0 || SetPlayerTwo > 0)
			SetAudio.Play ();
		ChangeScore (SetPlayerOne, _setP1_1);
		ChangeScore (SetPlayerTwo, _setP1_2);
		ChangeScore (SetPlayerOne, _setP2_1);
		ChangeScore (SetPlayerTwo, _setP2_2);
	}

	private void ChangeScore(int score, GameObject scoreGameobject, bool isScore = true)
	{
		scoreGameobject.transform.GetChild (0).GetComponent<UnityEngine.UI.Text> ().text = GetFormatedString(score, isScore ? '0' : 'A');
		scoreGameobject.transform.GetChild (1).GetComponent<UnityEngine.UI.Text> ().text = GetFormatedString(score, isScore ? 'A' : 'a');
		scoreGameobject.transform.GetChild (2).GetComponent<UnityEngine.UI.Text> ().text = GetFormatedString(score, isScore ? 'K' : 'A');
		scoreGameobject.transform.GetChild (3).GetComponent<UnityEngine.UI.Text> ().text = GetFormatedString(score, isScore ? 'a' : 'a');
		scoreGameobject.transform.GetChild (4).GetComponent<UnityEngine.UI.Text> ().text = GetFormatedString(score, isScore ? 'k' : 'A');
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

/*
 * Idées musiques :
 * - Zadok - Myrone
 * - Tires on Fire - Coda
 */