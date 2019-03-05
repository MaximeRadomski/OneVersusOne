﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelManagerBehavior : MonoBehaviour
{
	public Sprite[] MapTemplates;

	// ---- AUDIOS ---- //
	public int MenuBipSelectAudioFileID;
	public int MenuBipConfirmAudioFileID;
	public int MenuBipReturnAudioFileID;
	// ---- AUDIOS ---- //

	private GameObject _p1BannerMapName, _p1MapDescription, _p1SelectAndConfirmButton;
	private GameObject _p2BannerMapName, _p2MapDescription, _p2SelectAndConfirmButton;

	private GameObject _p1MapName, _p1Info1, _p1Info2, _p1Info4;
	private GameObject _p2MapName, _p2Info1, _p2Info2, _p2Info4;

	private GameObject _mapTemplate, _mapTemplateContainer;

	private bool _p1Confirm;
	private bool _p2Confirm;

	void Start ()
	{
		AndroidNativeAudio.makePool();
		_p1BannerMapName = GameObject.Find ("P1BannerMapName");
		_p1MapDescription = GameObject.Find ("P1MapDescription");
		_p1SelectAndConfirmButton = GameObject.Find ("P1SelectAndConfirmButton");

		_p1MapName = GameObject.Find ("P1MapName");
		_p1Info1 = GameObject.Find ("P1Info1");
		_p1Info2 = GameObject.Find ("P1Info2");
		_p1Info4 = GameObject.Find ("P1Info4");

		_p2BannerMapName = GameObject.Find ("P2BannerMapName");
		_p2MapDescription = GameObject.Find ("P2MapDescription");
		_p2SelectAndConfirmButton = GameObject.Find ("P2SelectAndConfirmButton");

		_p2MapName = GameObject.Find ("P2MapName");
		_p2Info1 = GameObject.Find ("P2Info1");
		_p2Info2 = GameObject.Find ("P2Info2");
		_p2Info4 = GameObject.Find ("P2Info4");

		_mapTemplate = GameObject.Find ("MapTemplate");
		_mapTemplateContainer = GameObject.Find ("MapTemplateContainer");

		// ---- AUDIOS ---- //
		MenuBipSelectAudioFileID = AndroidNativeAudio.load("MenuBipSelect.mp3");
		MenuBipConfirmAudioFileID = AndroidNativeAudio.load("MenuBipConfirm.mp3");
		MenuBipReturnAudioFileID = AndroidNativeAudio.load("MenuBipReturn.mp3");
		// ---- AUDIOS ---- //

		PlayerPrefs.SetInt ("SelectedMap", 1);

		_p1SelectAndConfirmButton.GetComponent<Animator> ().Play ("MapSelButtons");
		_p2SelectAndConfirmButton.GetComponent<Animator> ().Play ("MapSelButtons");
		StartCoroutine(InitiateLeft(true));
	}

	private IEnumerator InitiateLeft(bool firstTime = false)
	{
		DisableButtons ();
		GameObject tmpMapTemplate = null;
		if (!firstTime)
		{
			tmpMapTemplate = Instantiate (_mapTemplate, _mapTemplate.transform.position, _mapTemplate.transform.rotation);
			tmpMapTemplate.GetComponent<Animator> ().Play ("StartCenterToRight");
		}
		_mapTemplate.GetComponent<Animator> ().Play ("StartLeftToRight");
		_p1BannerMapName.GetComponent<Animator> ().Play ("StartNameBanner");
		_p2BannerMapName.GetComponent<Animator> ().Play ("StartNameBanner");
		_p1MapDescription.GetComponent<Animator> ().Play ("StartLeftToRight");
		_p2MapDescription.GetComponent<Animator> ().Play ("StartLeftToRight");
		yield return new WaitForSeconds(0.5f);
		if (tmpMapTemplate != null)
			Destroy (tmpMapTemplate);
		EnableButtons ();
	}

	private IEnumerator InitiateRight()
	{
		DisableButtons ();
		var tmpMapTemplate = Instantiate (_mapTemplate, _mapTemplate.transform.position, _mapTemplate.transform.rotation);
		tmpMapTemplate.GetComponent<Animator> ().Play ("StartCenterToLeft");
		_mapTemplate.GetComponent<Animator> ().Play ("StartRightToLeft");
		_p1BannerMapName.GetComponent<Animator> ().Play ("StartNameBanner");
		_p2BannerMapName.GetComponent<Animator> ().Play ("StartNameBanner");
		_p1MapDescription.GetComponent<Animator> ().Play ("StartLeftToRight");
		_p2MapDescription.GetComponent<Animator> ().Play ("StartLeftToRight");
		yield return new WaitForSeconds(0.5f);
		Destroy (tmpMapTemplate);
		EnableButtons ();
	}

	private IEnumerator ChangeMapInfo(int map)
	{
		yield return new WaitForSeconds(0.1f);

		_p1MapName.GetComponent<UnityEngine.UI.Text> ().text = MapsData.Maps[map - 1].Name;
		_p1Info1.GetComponent<UnityEngine.UI.Text> ().text = MapsData.Maps[map - 1].Country;
		_p1Info2.GetComponent<UnityEngine.UI.Text> ().text = MapsData.Maps[map - 1].City;
		_p1Info4.GetComponent<UnityEngine.UI.Text> ().text = "EFFECT: " + MapsData.Maps[map - 1].Effect;

		_p2MapName.GetComponent<UnityEngine.UI.Text> ().text = MapsData.Maps[map - 1].Name;
		_p2Info1.GetComponent<UnityEngine.UI.Text> ().text = MapsData.Maps[map - 1].Country;
		_p2Info2.GetComponent<UnityEngine.UI.Text> ().text = MapsData.Maps[map - 1].City;
		_p2Info4.GetComponent<UnityEngine.UI.Text> ().text = "EFFECT: " + MapsData.Maps[map - 1].Effect;

		_mapTemplateContainer.GetComponent<SpriteRenderer> ().sprite = MapTemplates [map - 1];
	}

	public void ChangeSelectedMap(Direction direction)
	{
		var map = PlayerPrefs.GetInt ("SelectedMap");
		if (direction == Direction.Left)
			--map;
		else
			++map;
		if (map == 0)
			map = MapTemplates.Length;
		else if (map > MapTemplates.Length)
			map = 1;

		if (direction == Direction.Left) {
			StartCoroutine (InitiateLeft ());
		} else {
			StartCoroutine(InitiateRight());
		}
		StartCoroutine (ChangeMapInfo (map));

		PlayerPrefs.SetInt ("SelectedMap", map);
		_p1Confirm = false;
		_p2Confirm = false;

		var tmpConfirm = GameObject.Find ("P1-Confirm");
		tmpConfirm.GetComponent<MapSelButtonBehavior> ().SpriteRenderer.sprite = tmpConfirm.GetComponent<MapSelButtonBehavior> ().SpriteOff;
		tmpConfirm = GameObject.Find ("P2-Confirm");
		tmpConfirm.GetComponent<MapSelButtonBehavior> ().SpriteRenderer.sprite = tmpConfirm.GetComponent<MapSelButtonBehavior> ().SpriteOff;
	}

	public void Confirm(int player)
	{
		if (player == 1)
			_p1Confirm = !_p1Confirm;
		else
			_p2Confirm = !_p2Confirm;

		if (_p1Confirm && _p2Confirm)
		{
			DisableButtons ();	
			Invoke ("LoadGameScene", 0.5f);
		}
	}

	private void DisableButtons()
	{
		var tmpButtons = GameObject.FindGameObjectsWithTag("Button");
		foreach (var button in tmpButtons)
		{
			button.GetComponent<BoxCollider2D> ().enabled = false;
		}
	}

	private void EnableButtons()
	{
		var tmpButtons = GameObject.FindGameObjectsWithTag("Button");
		foreach (var button in tmpButtons)
		{
			button.GetComponent<BoxCollider2D> ().enabled = true;
		}
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			AndroidNativeAudio.play (MenuBipReturnAudioFileID);
			Invoke ("LoadPreviousScene", 0.5f);
		}
	}

	private void LoadPreviousScene()
	{
		SceneManager.LoadScene("CharSelScene");
	}

	private void LoadGameScene()
	{
		SceneManager.LoadScene("GameLoadingScene");
	}

	void OnDestroy()
	{
		AndroidNativeAudio.unload(MenuBipSelectAudioFileID);
		AndroidNativeAudio.unload(MenuBipConfirmAudioFileID);
		AndroidNativeAudio.unload(MenuBipReturnAudioFileID);
		AndroidNativeAudio.releasePool();
	}
}
