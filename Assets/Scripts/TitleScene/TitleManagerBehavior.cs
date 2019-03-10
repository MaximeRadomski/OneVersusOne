using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManagerBehavior : MonoBehaviour
{
	public GameObject Popup;

	private GameObject _duelButton, _challengesButton, _howToPlayButton, _dashesText, _optionsButton, _aboutButton;
	private bool _isBusy;
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

		_isBusy = false;

		_genericMenuManagerBehavior = GameObject.Find ("$GenericMenuManager").GetComponent<GenericMenuManagerBehavior>();

		StartCoroutine (InitiateLeft());
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			if (_isBusy)
			{
				AndroidNativeAudio.play (_genericMenuManagerBehavior.MenuBipGoToAudioFileID);
				PopupReturn ();
			}
			else
				DisplayPopup ();
		}
	}

	private void DisplayPopup()
	{
		_isBusy = true;
		AndroidNativeAudio.play (_genericMenuManagerBehavior.MenuBipReturnAudioFileID);
		_tmpPopup = Instantiate (Popup, new Vector3(0.0f, 0.0f, 0.0f), Popup.transform.rotation);
		GameObject.Find ("PopupTitle").GetComponent<UnityEngine.UI.Text>().text = "LEAVING GAME";
		GameObject.Find ("PopupText").GetComponent<UnityEngine.UI.Text>().text = "DO YOU WANT TO LEAVE THE GAME ?";
		GameObject.Find ("Button01Background").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = Application.Quit;
		GameObject.Find ("Button02Background").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = PopupReturn;
		GameObject.Find ("PopupBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = PopupReturn;
	}



	private void PopupReturn()
	{
		Destroy (_tmpPopup);
		_isBusy = false;
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
