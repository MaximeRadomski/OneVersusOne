﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackgroundBehavior : MonoBehaviour
{
	public float ScrollSpeed;
	public MeshRenderer Renderer;

	private void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	void Start()
	{
		float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
		Debug.Log ("[DEBUG] worldScreenHeight: " + worldScreenHeight);
		transform.localScale = new Vector3(transform.localScale.x, worldScreenHeight, 1);

		var menuBackgroundList = GameObject.FindGameObjectsWithTag ("MenuBackground");
		if (menuBackgroundList.Length > 1)
			Destroy(gameObject);
		Renderer.sortingLayerName = "Background";
		if (PlayerPrefs.GetInt ("Music", 1) == 0)
			this.gameObject.GetComponent<AudioSource> ().volume = 0.0f;
	}

	void Update ()
	{
		float offset = Time.time * ScrollSpeed;
		Renderer.material.mainTextureOffset = new Vector2(offset, offset/2);
	}

	public void PlayAudio()
	{
		this.gameObject.GetComponent<AudioSource> ().Play ();
	}
}
