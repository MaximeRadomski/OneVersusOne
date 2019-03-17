using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DuelMenuBehavior : MonoBehaviour
{
	public GameObject PopupNumbers;

	private GameObject _opponentTitle, _scoreTitle, _setsTitle;
	private GameObject _opponentButtons, _scoreButtons, _setsButtons;
	//private GameObject _playerButton, _aiButton, _easyButton, _normalButton, _hardButton, _wallButton, _normalBouncesButton, _randomBouncesButton;
	//private GameObject _scoreMinusButton, _scoreNbButton, _scorePlusButton;
	//private GameObject _setsMinusButton, _setsNbButton, _setsPlusButton;
	private GameObject _confirmButton;
	private GameObject _tmpPopup;
	private GenericMenuManagerBehavior _genericMenuManagerBehavior;

	private Opponent _opponent;
	private Difficulty _difficulty;
	private Bounce _bounce;

	private bool _isDisplayingPopupScore;
	private bool _isDisplayingPopupSets;
	private int _nbScore;
	private int _nbSets;

	private bool _isDisplayingPopup
	{
		get
		{
			if (_isDisplayingPopupScore || _isDisplayingPopupSets)
				return true;
			return false;
		}
	}

	void Start ()
	{
		_opponentTitle = GameObject.Find ("Opponent");
		_scoreTitle = GameObject.Find ("Score");
		_setsTitle = GameObject.Find ("Sets");
		_opponentButtons = GameObject.Find ("OpponentButtons");
		_scoreButtons = GameObject.Find ("ScoreButtons");
		_setsButtons = GameObject.Find ("SetsButtons");
		_genericMenuManagerBehavior = GameObject.Find ("$GenericMenuManager").GetComponent<GenericMenuManagerBehavior>();
		_confirmButton = GameObject.Find ("ConfirmButton");
		_opponent = Opponent.Player;
		_difficulty = Difficulty.Normal;
		_bounce = Bounce.Normal;

		SetOpponentPlayer ();
		GameObject.Find ("PlayerBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = SetOpponentPlayer;
		GameObject.Find ("AIBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = SetOpponentAI;
		GameObject.Find ("EasyBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = SetAIEasy;
		GameObject.Find ("NormalBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = SetAINormal;
		GameObject.Find ("HardBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = SetAIHard;
		GameObject.Find ("WallBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = SetOpponentWall;
		GameObject.Find ("NormalBouncesBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = SetWallNormalBounce;
		GameObject.Find ("RandomBouncesBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = SetWallRandomBounce;
		GameObject.Find ("ScoreMinusBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = DecrementScore;
		GameObject.Find ("ScoreNbBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = DisplayPopupNumbersScore;
		GameObject.Find ("ScorePlusBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = IncrementScore;
		GameObject.Find ("SetsMinusBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = DecrementSets;
		GameObject.Find ("SetsNbBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = DisplayPopupNumbersSets;
		GameObject.Find ("SetsPlusBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = IncrementSets;
		GameObject.Find ("ConfirmBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = Confirm;

		_tmpPopup = null;
		_isDisplayingPopupScore = false;
		_isDisplayingPopupSets = false;
		_nbScore = 12;
		_nbSets = 2;

		StartCoroutine (InitiateLeft());
	}

	private IEnumerator InitiateLeft()
	{
		_opponentTitle.GetComponent<Animator> ().Play ("CharSelLeftToRight");
		yield return new WaitForSeconds(0.05f);
		_opponentButtons.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
		yield return new WaitForSeconds(0.05f);
		_scoreTitle.GetComponent<Animator> ().Play ("CharSelLeftToRight");
		yield return new WaitForSeconds(0.05f);
		_scoreButtons.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
		yield return new WaitForSeconds(0.05f);
		_setsTitle.GetComponent<Animator> ().Play ("CharSelLeftToRight");
		yield return new WaitForSeconds(0.05f);
		_setsButtons.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
		yield return new WaitForSeconds(0.05f);
		_confirmButton.GetComponent<Animator> ().Play ("MapSelButtons");
	}

	private void SetOpponentPlayer ()
	{
		_opponent = Opponent.Player;
		GameObject.Find ("PlayerBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOn();
		GameObject.Find ("AIBackground").GetComponent<GenericMenuButtonBehavior>().SetSpriteOff();
		GameObject.Find ("EasyBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("NormalBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("HardBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("WallBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("NormalBouncesBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("RandomBouncesBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
	}

	private void SetOpponentAI ()
	{
		_opponent = Opponent.AI;
		GameObject.Find ("PlayerBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("AIBackground").GetComponent<GenericMenuButtonBehavior>().SetSpriteOn();
		if (_difficulty == Difficulty.Easy)
			GameObject.Find ("EasyBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOn();
		else
			GameObject.Find ("EasyBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		if (_difficulty == Difficulty.Normal)
			GameObject.Find ("NormalBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOn();
		else
			GameObject.Find ("NormalBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		if (_difficulty == Difficulty.Hard)
			GameObject.Find ("HardBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOn();
		else
			GameObject.Find ("HardBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("WallBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("NormalBouncesBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("RandomBouncesBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
	}

	private void SetAIEasy ()
	{
		_opponent = Opponent.AI;
		_difficulty = Difficulty.Easy;
		GameObject.Find ("PlayerBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("AIBackground").GetComponent<GenericMenuButtonBehavior>().SetSpriteOn();
		GameObject.Find ("EasyBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOn();
		GameObject.Find ("NormalBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("HardBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("WallBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("NormalBouncesBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("RandomBouncesBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
	}

	private void SetAINormal ()
	{
		_opponent = Opponent.AI;
		_difficulty = Difficulty.Normal;
		GameObject.Find ("PlayerBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("AIBackground").GetComponent<GenericMenuButtonBehavior>().SetSpriteOn();
		GameObject.Find ("EasyBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("NormalBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOn();
		GameObject.Find ("HardBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("WallBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("NormalBouncesBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("RandomBouncesBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
	}

	private void SetAIHard ()
	{
		_opponent = Opponent.AI;
		_difficulty = Difficulty.Hard;
		GameObject.Find ("PlayerBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("AIBackground").GetComponent<GenericMenuButtonBehavior>().SetSpriteOn();
		GameObject.Find ("EasyBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("NormalBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("HardBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOn();
		GameObject.Find ("WallBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("NormalBouncesBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("RandomBouncesBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
	}

	private void SetOpponentWall ()
	{
		_opponent = Opponent.Wall;
		GameObject.Find ("PlayerBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("AIBackground").GetComponent<GenericMenuButtonBehavior>().SetSpriteOff();
		GameObject.Find ("EasyBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("NormalBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("HardBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("WallBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOn();
		if (_bounce == Bounce.Normal)
			GameObject.Find ("NormalBouncesBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOn();
		else
			GameObject.Find ("NormalBouncesBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		if (_bounce == Bounce.Random)
			GameObject.Find ("RandomBouncesBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOn();
		else
			GameObject.Find ("RandomBouncesBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
	}

	private void SetWallNormalBounce ()
	{
		_opponent = Opponent.Wall;
		_bounce = Bounce.Normal;
		GameObject.Find ("PlayerBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("AIBackground").GetComponent<GenericMenuButtonBehavior>().SetSpriteOff();
		GameObject.Find ("EasyBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("NormalBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("HardBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("WallBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOn();
		GameObject.Find ("NormalBouncesBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOn();
		GameObject.Find ("RandomBouncesBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
	}

	private void SetWallRandomBounce ()
	{
		_opponent = Opponent.Wall;
		_bounce = Bounce.Random;
		GameObject.Find ("PlayerBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("AIBackground").GetComponent<GenericMenuButtonBehavior>().SetSpriteOff();
		GameObject.Find ("EasyBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("NormalBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("HardBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("WallBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOn();
		GameObject.Find ("NormalBouncesBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		GameObject.Find ("RandomBouncesBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOn();
	}

	private void DecrementScore ()
	{
		var tmpNumber = _nbScore - 1;
		_nbScore = CheckScoreSets (tmpNumber, _nbScore);
		GameObject.Find ("ScoreNbText").GetComponent<UnityEngine.UI.Text>().text = _nbScore.ToString("D2");
	}

	private void IncrementScore ()
	{
		var tmpNumber = _nbScore + 1;
		_nbScore = CheckScoreSets (tmpNumber, _nbScore);
		GameObject.Find ("ScoreNbText").GetComponent<UnityEngine.UI.Text>().text = _nbScore.ToString("D2");
	}

	private void DecrementSets ()
	{
		var tmpNumber = _nbSets - 1;
		_nbSets = CheckScoreSets (tmpNumber, _nbSets);
		GameObject.Find ("SetsNbText").GetComponent<UnityEngine.UI.Text>().text = _nbSets.ToString("D2");
	}

	private void IncrementSets ()
	{
		var tmpNumber = _nbSets + 1;
		_nbSets = CheckScoreSets (tmpNumber, _nbSets);
		GameObject.Find ("SetsNbText").GetComponent<UnityEngine.UI.Text>().text = _nbSets.ToString("D2");
	}

	private void DisplayPopupNumbersScore()
	{
		if (_isDisplayingPopup)
			return;
		_isDisplayingPopupScore = true;
		AndroidNativeAudio.play (_genericMenuManagerBehavior.MenuBipReturnAudioFileID);
		_tmpPopup = Instantiate (PopupNumbers, new Vector3(0.0f, 0.0f, 0.0f), PopupNumbers.transform.rotation);
		GameObject.Find ("PopupTitle").GetComponent<UnityEngine.UI.Text>().text = "SCORE";
		GameObject.Find ("ButtonBackBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = PopupNumbersScoreReturn;
		GameObject.Find ("PopupBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = PopupNumbersScoreReturn;
	}

	private void PopupNumbersScoreReturn()
	{
		var tmpNumber = _tmpPopup.GetComponent<PopupNumbersBehavior> ().Number;
		_nbScore = CheckScoreSets (tmpNumber, _nbScore);
		GameObject.Find ("ScoreNbText").GetComponent<UnityEngine.UI.Text>().text = _nbScore.ToString("D2");
		Destroy (_tmpPopup);
		_isDisplayingPopupScore = false;
	}

	private void DisplayPopupNumbersSets()
	{
		if (_isDisplayingPopup)
			return;
		_isDisplayingPopupSets = true;
		AndroidNativeAudio.play (_genericMenuManagerBehavior.MenuBipReturnAudioFileID);
		_tmpPopup = Instantiate (PopupNumbers, new Vector3(0.0f, 0.0f, 0.0f), PopupNumbers.transform.rotation);
		GameObject.Find ("PopupTitle").GetComponent<UnityEngine.UI.Text>().text = "SETS";
		GameObject.Find ("ButtonBackBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = PopupNumbersSetsReturn;
		GameObject.Find ("PopupBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = PopupNumbersSetsReturn;
	}

	private void PopupNumbersSetsReturn()
	{
		var tmpNumber = _tmpPopup.GetComponent<PopupNumbersBehavior> ().Number;
		_nbSets = CheckScoreSets (tmpNumber, _nbSets);
		GameObject.Find ("SetsNbText").GetComponent<UnityEngine.UI.Text>().text = _nbSets.ToString("D2");
		Destroy (_tmpPopup);
		_isDisplayingPopupSets = false;
	}

	private int CheckScoreSets(int number, int initial)
	{
		if (number == 0)
			return initial;
		else if (number >= 100)
			return 99;
		return number;
	}

	void Update ()
	{
		if (Input.GetKeyUp (KeyCode.Escape))
		{
			if (_isDisplayingPopup) {
				if (_isDisplayingPopupScore)
					PopupNumbersScoreReturn ();
				else if (_isDisplayingPopupSets)
					PopupNumbersSetsReturn (); 
			} else
				SceneManager.LoadScene("TitleScene");
		}
	}

	private void Confirm()
	{
		PlayerPrefs.SetInt ("Opponent", _opponent.GetHashCode());
		PlayerPrefs.SetInt ("Difficulty", _difficulty.GetHashCode());
		PlayerPrefs.SetInt ("Bounce", _bounce.GetHashCode());

		PlayerPrefs.SetInt ("MaxScore", _nbScore);
		PlayerPrefs.SetInt ("MaxSets", _nbSets);

		SceneManager.LoadScene("CharSelScene");
	}
}
