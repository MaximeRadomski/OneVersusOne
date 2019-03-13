using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuelMenuBehavior : MonoBehaviour
{
	public GameObject PopupNumbers;

	private GameObject _opponent, _score, _sets;
	private GameObject _opponentButtons, _scoreButtons, _setsButtons;
	//private GameObject _playerButton, _aiButton, _easyButton, _normalButton, _hardButton, _wallButton, _normalBouncesButton, _randomBouncesButton;
	//private GameObject _scoreMinusButton, _scoreNbButton, _scorePlusButton;
	//private GameObject _setsMinusButton, _setsNbButton, _setsPlusButton;
	//private GameObject _confirmButton;
	private GameObject _tmpPopup;
	private bool _isDisplayingPopup;

	private GenericMenuManagerBehavior _genericMenuManagerBehavior;

	void Start ()
	{
		_opponent = GameObject.Find ("Opponent");
		_score = GameObject.Find ("Score");
		_sets = GameObject.Find ("Sets");
		_opponentButtons = GameObject.Find ("OpponentButtons");
		_scoreButtons = GameObject.Find ("ScoreButtons");
		_setsButtons = GameObject.Find ("SetsButtons");
		_genericMenuManagerBehavior = GameObject.Find ("$GenericMenuManager").GetComponent<GenericMenuManagerBehavior>();

		GameObject.Find ("PlayerButtonBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = DisplayPopupNumbers;
		GameObject.Find ("AIButtonBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = DisplayPopupNumbers;
		GameObject.Find ("EasyButtonBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = DisplayPopupNumbers;
		GameObject.Find ("NormalButtonBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = DisplayPopupNumbers;
		GameObject.Find ("HardButtonBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = DisplayPopupNumbers;
		GameObject.Find ("WallButtonBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = DisplayPopupNumbers;
		GameObject.Find ("NormalBouncesButtonBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = DisplayPopupNumbers;
		GameObject.Find ("RandomBouncesButtonBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = DisplayPopupNumbers;
		GameObject.Find ("ScoreMinusButtonBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = DisplayPopupNumbers;
		GameObject.Find ("ScoreNbButtonBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = DisplayPopupNumbers;
		GameObject.Find ("ScorePlusButtonBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = DisplayPopupNumbers;
		GameObject.Find ("SetsMinusButtonBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = DisplayPopupNumbers;
		GameObject.Find ("SetsNbButtonBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = DisplayPopupNumbers;
		GameObject.Find ("SetsPlusButtonBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = DisplayPopupNumbers;
		GameObject.Find ("ConfirmBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = DisplayPopupNumbers;

		_tmpPopup = null;
		_isDisplayingPopup = false;

		StartCoroutine (InitiateLeft());
	}

	private IEnumerator InitiateLeft()
	{
		_opponent.GetComponent<Animator> ().Play ("CharSelLeftToRight");
		yield return new WaitForSeconds(0.05f);
		_opponentButtons.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
		yield return new WaitForSeconds(0.05f);
		_score.GetComponent<Animator> ().Play ("CharSelLeftToRight");
		yield return new WaitForSeconds(0.05f);
		_scoreButtons.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
		yield return new WaitForSeconds(0.05f);
		_sets.GetComponent<Animator> ().Play ("CharSelLeftToRight");
		yield return new WaitForSeconds(0.05f);
		_setsButtons.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
	}

	private void DisplayPopupNumbers()
	{
		if (_isDisplayingPopup)
			return;
		_isDisplayingPopup = true;
		AndroidNativeAudio.play (_genericMenuManagerBehavior.MenuBipReturnAudioFileID);
		_tmpPopup = Instantiate (PopupNumbers, new Vector3(0.0f, 0.0f, 0.0f), PopupNumbers.transform.rotation);
		GameObject.Find ("PopupTitle").GetComponent<UnityEngine.UI.Text>().text = "OOPS!";
		GameObject.Find ("PopupText").GetComponent<UnityEngine.UI.Text>().text = "NOT IMPLEMENTED YET";
		GameObject.Find ("Button01Background").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = PopupReturn;
		GameObject.Find ("PopupBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = PopupReturn;
	}

	private void PopupReturn()
	{
		Destroy (_tmpPopup);
		_isDisplayingPopup = false;
	}

	void Update ()
	{
		
	}
}
