using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomAudio
{
	public static float EffectLevel = 0.50f;

	public static void PlayEffect(int id)
	{
		bool isSoundEnabled = PlayerPrefs.GetInt ("Effects", 1) == 1 ? true : false;
		float level = isSoundEnabled ? EffectLevel : 0.0f;
		AndroidNativeAudio.play (id, level);
	}
}
