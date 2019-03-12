﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManagerBehavior : MonoBehaviour
{
	public GameObject PopupYesNo;
	public GameObject PopupSingle;

	private GameObject _duelButton, _challengesButton, _howToPlayButton, _dashesText, _optionsButton, _aboutButton;
	private bool _isDisplayingPopup;
	private GameObject _tmpPopup;

	private GenericMenuManagerBehavior _genericMenuManagerBehavior;

	void Start ()
	{
		_duelButton = GameObject.Find ("DuelButton");
		_challengesButton = GameObject.Find ("ChallengesButton");
		_howToPlayButton = GameObject.Find ("HowToPlayButton");
		_dashesText = GameObject.Find ("DashesText");
		_optionsButton = GameObject.Find ("OptionsButton");
		_aboutButton = GameObject.Find ("AboutButton");
		_genericMenuManagerBehavior = GameObject.Find ("$GenericMenuManager").GetComponent<GenericMenuManagerBehavior>();
		_isDisplayingPopup = false;

		_challengesButton.transform.GetChild(1).GetComponent<GenericMenuButtonBehavior>().buttonDelegate = DisplayPopupSingle;
		_howToPlayButton.transform.GetChild(1).GetComponent<GenericMenuButtonBehavior>().buttonDelegate = DisplayPopupSingle;
		_optionsButton.transform.GetChild(1).GetComponent<GenericMenuButtonBehavior>().buttonDelegate = DisplayPopupSingle;
		_aboutButton.transform.GetChild(1).GetComponent<GenericMenuButtonBehavior>().buttonDelegate = DisplayPopupSingle;

		StartCoroutine (InitiateLeft());
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			if (_isDisplayingPopup)
			{
				AndroidNativeAudio.play (_genericMenuManagerBehavior.MenuBipGoToAudioFileID);
				PopupReturn ();
			}
			else
				DisplayPopupYesNo ();
		}
	}

	private void DisplayPopupYesNo()
	{
		_isDisplayingPopup = true;
		AndroidNativeAudio.play (_genericMenuManagerBehavior.MenuBipReturnAudioFileID);
		_tmpPopup = Instantiate (PopupYesNo, new Vector3(0.0f, 0.0f, 0.0f), PopupYesNo.transform.rotation);
		GameObject.Find ("PopupTitle").GetComponent<UnityEngine.UI.Text>().text = "LEAVING GAME";
		GameObject.Find ("PopupText").GetComponent<UnityEngine.UI.Text>().text = "DO YOU WANT TO LEAVE THE GAME ?";
		GameObject.Find ("Button01Background").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = Application.Quit;
		GameObject.Find ("Button02Background").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = PopupReturn;
		GameObject.Find ("PopupBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = PopupReturn;
	}

	private void DisplayPopupSingle()
	{
		_isDisplayingPopup = true;
		AndroidNativeAudio.play (_genericMenuManagerBehavior.MenuBipReturnAudioFileID);
		_tmpPopup = Instantiate (PopupSingle, new Vector3(0.0f, 0.0f, 0.0f), PopupSingle.transform.rotation);
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

	private IEnumerator InitiateLeft()
	{
		_duelButton.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
		yield return new WaitForSeconds(0.05f);
		_challengesButton.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
		yield return new WaitForSeconds(0.05f);
		_howToPlayButton.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
		yield return new WaitForSeconds(0.05f);
		_dashesText.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
		yield return new WaitForSeconds(0.05f);
		_optionsButton.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
		yield return new WaitForSeconds(0.05f);
		_aboutButton.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
	}
}
