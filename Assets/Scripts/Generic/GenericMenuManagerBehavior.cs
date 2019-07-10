using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenericMenuManagerBehavior : MonoBehaviour
{
	public string BackSceneName;

	// ---- AUDIOS ---- //
	public int MenuBipDefaultAudioFileID;
	public int MenuBipGoToAudioFileID;
	public int MenuBipSelectAudioFileID;
	public int MenuBipConfirmAudioFileID;
	public int MenuBipReturnAudioFileID;

	public int SwitchTVONFileID;
	public int NamePresentationFileID;
	public int BorderMovementFileID;

	private bool _hasMadeAudioPool = false;
	private GameObject _backButton;

	void Start ()
	{
		var genericMenuList = GameObject.FindGameObjectsWithTag ("GenericMenu");
		if (genericMenuList.Length > 1)
		{
			foreach (var genericMenuManager in genericMenuList)
				genericMenuManager.GetComponent<GenericMenuManagerBehavior> ().BackSceneName = this.BackSceneName;
			Destroy(gameObject);
			return;
		}

		AndroidNativeAudio.makePool();
		_hasMadeAudioPool = true;

		SceneManager.sceneLoaded += OnSceneLoaded;
		HandleBackButton ();

		// ---- AUDIOS ---- //
		MenuBipDefaultAudioFileID = AndroidNativeAudio.load("MenuBipDefault.mp3");
		MenuBipGoToAudioFileID = AndroidNativeAudio.load("MenuBipGoTo.mp3");
		MenuBipSelectAudioFileID = AndroidNativeAudio.load("MenuBipSelect.mp3");
		MenuBipConfirmAudioFileID = AndroidNativeAudio.load("MenuBipConfirm.mp3");
		MenuBipReturnAudioFileID = AndroidNativeAudio.load("MenuBipReturn.mp3");

		SwitchTVONFileID = AndroidNativeAudio.load("SwitchTVON.mp3");
		NamePresentationFileID = AndroidNativeAudio.load("NamePresentation.mp3");
		BorderMovementFileID = AndroidNativeAudio.load("BorderMovement.mp3");
	}

	private void OnSceneLoaded (Scene arg0, LoadSceneMode arg1)
	{
		HandleBackButton ();
	}

	private void HandleBackButton()
	{
		_backButton = GameObject.Find ("BackButton");
		if (_backButton != null)
		{
			_backButton.GetComponent<GenericMenuButtonBehavior> ().buttonDelegate = OnBackButtonPressed;
		}
	}

	private void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			OnBackButtonPressed ();
		}
	}

	private void OnBackButtonPressed()
	{
		if (BackSceneName != string.Empty && BackSceneName != null)
		{
			SceneManager.LoadScene (BackSceneName);
			CustomAudio.PlayEffect(MenuBipReturnAudioFileID);
		}
	}

	void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
		if (_hasMadeAudioPool)
		{
			AndroidNativeAudio.unload (MenuBipDefaultAudioFileID);
			AndroidNativeAudio.unload (MenuBipGoToAudioFileID);
			AndroidNativeAudio.unload (MenuBipSelectAudioFileID);
			AndroidNativeAudio.unload (MenuBipConfirmAudioFileID);
			AndroidNativeAudio.unload (MenuBipReturnAudioFileID);

			AndroidNativeAudio.unload (SwitchTVONFileID);
			AndroidNativeAudio.unload (NamePresentationFileID);
			AndroidNativeAudio.unload (BorderMovementFileID);

			AndroidNativeAudio.releasePool ();
		}
	}
}
