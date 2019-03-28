using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanLinesBehaviours : MonoBehaviour
{
	private void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	public void SetOpacity(float opacity)
	{
		gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, opacity);
		Debug.Log ("\t[DEBGUG]\tScanLines Opacity set to " + (opacity * 100.0f) + "%.");
	}

	void Start()
	{
		var opacityInt = PlayerPrefs.GetInt ("ScanLines", 1);
		var newOpacity = (float)opacityInt * 0.10f;
		SetOpacity (newOpacity);
	}
}
