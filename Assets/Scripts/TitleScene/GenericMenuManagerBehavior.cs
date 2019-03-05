using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericMenuManagerBehavior : MonoBehaviour
{
	// ---- AUDIOS ---- //
	public int MenuBipGoToAudioFileID;

	void Start ()
	{
		AndroidNativeAudio.makePool();

		// ---- AUDIOS ---- //
		MenuBipGoToAudioFileID = AndroidNativeAudio.load("MenuBipGoTo.mp3");
	}

	void OnDestroy()
	{
		AndroidNativeAudio.unload(MenuBipGoToAudioFileID);
		AndroidNativeAudio.releasePool();
	}
}
