﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChallengesMenuBehavior : MonoBehaviour
{
	private GameObject _tournamentTitle, _targetsTitle, _catchTitle, _breakoutTitle;
	private GameObject _tournamentButtons, _targetsButtons, _catchButtons, _breakoutButtons;

	private string[] _challengesNames = new string[] {"Targets", "Catch", "Breakout", "Tournament"};
	private List<int> _challengesProgression;
	private int _localTournamentProgression;

	void Start ()
	{
		_tournamentTitle = GameObject.Find ("Tournament");
		_tournamentButtons = GameObject.Find ("TournamentButtons");
		_targetsTitle = GameObject.Find ("Targets");
		_targetsButtons = GameObject.Find ("TargetsButtons");
		_catchTitle = GameObject.Find ("Catch");
		_catchButtons = GameObject.Find ("CatchButtons");
		_breakoutTitle = GameObject.Find ("Breakout");
		_breakoutButtons = GameObject.Find ("BreakoutButtons");

		GameObject.Find ("Targets01ButtonBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = Target01;
		GameObject.Find ("Targets02ButtonBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = Target02;
		GameObject.Find ("Targets03ButtonBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = Target03;

		SetButtons ();
		StartCoroutine (InitiateLeft());
	}

	private void Target01()
	{
		PlayerPrefs.SetInt ("CurrentChallengeDifficulty", 1);
		PlayerPrefs.SetInt ("Opponent", Opponent.Target.GetHashCode ());
		PlayerPrefs.SetInt ("Difficulty", Difficulty.Easy.GetHashCode ());
		PlayerPrefs.SetInt ("GameMode", GameMode.Target.GetHashCode ());
		PlayerPrefs.SetInt ("SelectedMap", 1);
		PlayerPrefs.SetInt ("P1Character", 2);
		PlayerPrefs.SetInt ("MaxScore", 10);
		LoadGameScene ();
	}

	private void Target02()
	{
		PlayerPrefs.SetInt ("CurrentChallengeDifficulty", 2);
		PlayerPrefs.SetInt ("Opponent", Opponent.Target.GetHashCode ());
		PlayerPrefs.SetInt ("Difficulty", Difficulty.Normal.GetHashCode ());
		PlayerPrefs.SetInt ("GameMode", GameMode.Target.GetHashCode ());
		PlayerPrefs.SetInt ("SelectedMap", 6);
		PlayerPrefs.SetInt ("P1Character", 4);
		PlayerPrefs.SetInt ("MaxScore", 20);
		LoadGameScene ();
	}

	private void Target03()
	{
		PlayerPrefs.SetInt ("CurrentChallengeDifficulty", 3);
		PlayerPrefs.SetInt ("Opponent", Opponent.Target.GetHashCode ());
		PlayerPrefs.SetInt ("Difficulty", Difficulty.Normal.GetHashCode ());
		PlayerPrefs.SetInt ("GameMode", GameMode.Target.GetHashCode ());
		PlayerPrefs.SetInt ("SelectedMap", 4);
		PlayerPrefs.SetInt ("P1Character", 5);
		PlayerPrefs.SetInt ("MaxScore", 30);
		LoadGameScene ();
	}

	private void SetButtons ()
	{
		_challengesProgression = new List<int> ();
		_challengesProgression.Add (PlayerPrefs.GetInt(_challengesNames[0], 1));
		_challengesProgression.Add (PlayerPrefs.GetInt(_challengesNames[1], 1));
		_challengesProgression.Add (PlayerPrefs.GetInt(_challengesNames[2], 1));
		_challengesProgression.Add (PlayerPrefs.GetInt(_challengesNames[3], 0));
		_localTournamentProgression = 0;

		for (int i = 1; i < 4; ++i)
		{
			if (_challengesProgression [0] > i &&
				_challengesProgression [1] > i &&
				_challengesProgression [2] > i)
			{
				++_localTournamentProgression;
			}
		}

		if (_challengesProgression [3] == 4)
			++_localTournamentProgression;

		for (int i = 0; i < _challengesProgression.Count; ++i)
		{
			for (int j = 1; j <= 4; ++j)
			{
				var tmpButton = GameObject.Find (_challengesNames [i] + j.ToString ("D2") + "Button");
				if (tmpButton == null)
					continue;
				if (_challengesProgression [i] > j)
					tmpButton.transform.GetChild (1).GetComponent<GenericMenuButtonBehavior>().SwitchSprite();
				else if (i != 3 && j > _challengesProgression [i] || i == 3 && j > _localTournamentProgression)
				{
					tmpButton.transform.GetChild (0).GetComponent<UnityEngine.UI.Text> ().color = new Color (1.0f, 1.0f, 1.0f, 0.5f);
					tmpButton.transform.GetChild (1).GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 0.5f);
					tmpButton.transform.GetChild (1).GetComponent<BoxCollider2D> ().enabled = false;
				}
			}
		}
	}

	private IEnumerator InitiateLeft()
	{
		_tournamentTitle.GetComponent<Animator> ().Play ("CharSelLeftToRight");
		yield return new WaitForSeconds(0.05f);
		_tournamentButtons.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
		yield return new WaitForSeconds(0.05f);
		_targetsTitle.GetComponent<Animator> ().Play ("CharSelLeftToRight");
		yield return new WaitForSeconds(0.05f);
		_targetsButtons.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
		yield return new WaitForSeconds(0.05f);
		_catchTitle.GetComponent<Animator> ().Play ("CharSelLeftToRight");
		yield return new WaitForSeconds(0.05f);
		_catchButtons.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
		yield return new WaitForSeconds(0.05f);
		_breakoutTitle.GetComponent<Animator> ().Play ("CharSelLeftToRight");
		yield return new WaitForSeconds(0.05f);
		_breakoutButtons.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
	}

	private void LoadGameScene()
	{
		SceneManager.LoadScene("GameLoadingScene");
	}
}
