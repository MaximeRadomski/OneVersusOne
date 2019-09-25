using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManagerBehavior : MonoBehaviour
{
	public GameObject PopupYesNo;
	public GameObject PopupSingle;

	private GameObject _duelButton, _challengesButton, _howToPlayButton, _dashesText, _optionsButton, _aboutButton, _loadButton;
	private bool _isDisplayingPopup;
	private GameObject _tmpPopup;

	private GenericMenuManagerBehavior _genericMenuManagerBehavior;

	private int _titleClick;

	void Start ()
	{
		_duelButton = GameObject.Find ("DuelButton");
		_challengesButton = GameObject.Find ("ChallengesButton");
		_howToPlayButton = GameObject.Find ("HowToPlayButton");
		_dashesText = GameObject.Find ("DashesText");
		_optionsButton = GameObject.Find ("OptionsButton");
		_aboutButton = GameObject.Find ("AboutButton");
        _loadButton = GameObject.Find("LoadButton");
        _genericMenuManagerBehavior = GameObject.Find ("$GenericMenuManager").GetComponent<GenericMenuManagerBehavior>();
		_isDisplayingPopup = false;
		_titleClick = 0;

        //_challengesButton.transform.GetChild(1).GetComponent<GenericMenuButtonBehavior>().buttonDelegate = DisplayPopupNotImplemented;
        _duelButton.transform.GetChild(1).GetComponent<GenericMenuButtonBehavior>().buttonDelegate = GoToDuel;
        _howToPlayButton.transform.GetChild(1).GetComponent<GenericMenuButtonBehavior>().buttonDelegate = GoToHowToPlay;
        _loadButton.transform.GetChild(1).GetComponent<GenericMenuButtonBehavior>().buttonDelegate = LoadGameInProgress;
        GameObject.Find("GameName").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = IncrementTitleClick;
		GameObject.Find("VersionWatermark").GetComponent<UnityEngine.UI.Text>().text = Application.version;

		StartCoroutine (InitiateLeft());
	}

	private void ResetPlayerPrefs()
	{
        PlayerPrefs.SetInt ("GameInProgress", 0);
		PlayerPrefs.SetInt ("P1Character", 1);
		PlayerPrefs.SetInt ("P2Character", 1);
		PlayerPrefs.SetInt ("Opponent", Opponent.Player.GetHashCode());
		PlayerPrefs.SetInt ("Difficulty", Difficulty.Easy.GetHashCode());
		PlayerPrefs.SetInt ("Bounce", Bounce.Normal.GetHashCode());
		PlayerPrefs.SetInt ("MaxScore", 12);
		PlayerPrefs.SetInt ("MaxSets", 2);
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			if (_isDisplayingPopup)
			{
				CustomAudio.PlayEffect (_genericMenuManagerBehavior.MenuBipGoToAudioFileID);
				PopupReturn ();
			}
			else
				DisplayPopupQuit ();
		}
	}

	private void DisplayPopupQuit()
	{
		_isDisplayingPopup = true;
		CustomAudio.PlayEffect (_genericMenuManagerBehavior.MenuBipReturnAudioFileID);
		_tmpPopup = Instantiate (PopupYesNo, new Vector3(0.0f, 0.0f, 0.0f), PopupYesNo.transform.rotation);
		GameObject.Find ("PopupTitle").GetComponent<UnityEngine.UI.Text>().text = "LEAVING GAME";
		GameObject.Find ("PopupText").GetComponent<UnityEngine.UI.Text>().text = "DO YOU WANT TO LEAVE THE GAME?";
		GameObject.Find ("Button01Background").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = Application.Quit;
		GameObject.Find ("Button02Background").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = PopupReturn;
		GameObject.Find ("PopupBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = PopupReturn;
	}

	private void DisplayPopupNotImplemented()
	{
		_isDisplayingPopup = true;
		CustomAudio.PlayEffect (_genericMenuManagerBehavior.MenuBipReturnAudioFileID);
		_tmpPopup = Instantiate (PopupSingle, new Vector3(0.0f, 0.0f, 0.0f), PopupSingle.transform.rotation);
		GameObject.Find ("PopupTitle").GetComponent<UnityEngine.UI.Text>().text = "OOPS!";
		GameObject.Find ("PopupText").GetComponent<UnityEngine.UI.Text>().text = "NOT IMPLEMENTED YET";
		GameObject.Find ("Button01Background").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = PopupReturn;
		GameObject.Find ("PopupBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = PopupReturn;
	}

	private void DisplayPopupEraseData()
	{
		_isDisplayingPopup = true;
		CustomAudio.PlayEffect (_genericMenuManagerBehavior.MenuBipReturnAudioFileID);
		_tmpPopup = Instantiate (PopupYesNo, new Vector3(0.0f, 0.0f, 0.0f), PopupYesNo.transform.rotation);
		GameObject.Find ("PopupTitle").GetComponent<UnityEngine.UI.Text>().text = "Watchout";
		GameObject.Find ("PopupText").GetComponent<UnityEngine.UI.Text>().text = "DO YOU WANT TO ERASE YOUR PROGRESSION?";
		GameObject.Find ("Button01Background").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = DeletePlayerPrefsAndPopupReturn;
		GameObject.Find ("Button02Background").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = PopupReturn;
		GameObject.Find ("PopupBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = PopupReturn;
	}

	private void DisplayPopupCheat()
	{
		_isDisplayingPopup = true;
		CustomAudio.PlayEffect (_genericMenuManagerBehavior.MenuBipReturnAudioFileID);
		_tmpPopup = Instantiate (PopupSingle, new Vector3(0.0f, 0.0f, 0.0f), PopupSingle.transform.rotation);
		GameObject.Find ("PopupTitle").GetComponent<UnityEngine.UI.Text>().text = "HEHE BOY";
		GameObject.Find ("PopupText").GetComponent<UnityEngine.UI.Text>().text = "Give your meat a good old rub!";
		GameObject.Find ("Button01Background").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = PopupReturn;
		GameObject.Find ("PopupBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = PopupReturn;
	}

	private void DeletePlayerPrefsAndPopupReturn()
	{
		PlayerPrefs.DeleteAll();
		PlayerPrefs.SetInt ("FirstTime", 0);
		Destroy (_tmpPopup);
		_isDisplayingPopup = false;
	}

	private void PopupReturn()
	{
		Destroy (_tmpPopup);
		_isDisplayingPopup = false;
	}

    private void GoToDuel()
    {
        ResetPlayerPrefs();
        SceneManager.LoadScene("DuelMenu");
    }

    private void GoToHowToPlay ()
	{
		PlayerPrefs.SetInt ("GameMode", GameMode.Duel.GetHashCode ());
		PlayerPrefs.SetInt ("Opponent", Opponent.Wall.GetHashCode ());
		PlayerPrefs.SetInt ("Bounce", Bounce.Normal.GetHashCode ());
		PlayerPrefs.SetInt ("P1Character", 2);
		PlayerPrefs.SetInt ("SelectedMap", -1);
		SceneManager.LoadScene("GameLoadingScene");
	}

    private void LoadGameInProgress()
    {
        if (PlayerPrefs.GetInt("GameMode") == GameMode.Tournament.GetHashCode())
            SceneManager.LoadScene("TournamentMatch");
        else
            SceneManager.LoadScene("GameLoadingScene");
        
    }

	private void IncrementTitleClick()
	{
		++_titleClick;
		if (_titleClick % 5 == 0)
			DisplayPopupEraseData ();
		else if (_titleClick % 42 == 0)
		{
			PlayerPrefs.SetInt ("Targets", 4);
			PlayerPrefs.SetInt ("Catch", 4);
			PlayerPrefs.SetInt ("Breakout", 4);
			PlayerPrefs.SetInt ("Tournament", 0);
			DisplayPopupCheat ();
		}
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
        yield return new WaitForSeconds(0.05f);
        if (PlayerPrefs.GetInt("GameInProgress", 0) == 1)
            _loadButton.GetComponent<Animator>().Play("LeftOut-RightMiddle");
    }
}
