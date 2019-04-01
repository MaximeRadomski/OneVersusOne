using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAudio : MonoBehaviour
{
	public static void PlayEffect(int id)
	{
		var level = PlayerPrefs.GetInt ("Effects");
		AndroidNativeAudio.play (id, (float)level);
	}
}
