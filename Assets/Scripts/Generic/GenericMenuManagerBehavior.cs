using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenericMenuManagerBehavior : MonoBehaviour
{
	public string BackSceneName;

	// ---- AUDIOS ---- //
	public int MenuBipGoToAudioFileID;
	public int MenuBipReturnAudioFileID;

	void Start ()
	{
		AndroidNativeAudio.makePool();

		// ---- AUDIOS ---- //
		MenuBipGoToAudioFileID = AndroidNativeAudio.load("MenuBipGoTo.mp3");
		MenuBipReturnAudioFileID = AndroidNativeAudio.load("MenuBipReturn.mp3");
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			if (BackSceneName != string.Empty && BackSceneName != null)
				SceneManager.LoadScene (BackSceneName);
			else
				Application.Quit ();
			AndroidNativeAudio.play (MenuBipReturnAudioFileID);
		}
	}

	void OnDestroy()
	{
		AndroidNativeAudio.unload(MenuBipGoToAudioFileID);
		AndroidNativeAudio.unload(MenuBipReturnAudioFileID);
		AndroidNativeAudio.releasePool();
	}
}
