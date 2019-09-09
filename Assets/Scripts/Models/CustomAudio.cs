using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomAudio
{
	public static void PlayEffect(int id, float customRate = 1.0f)
	{
		float level = PlayerPrefs.GetInt ("Effects", 1) == 1 ? 1.0f : 0.0f;
        AndroidNativeAudio.play (id, level, rate:customRate);
    }
}
