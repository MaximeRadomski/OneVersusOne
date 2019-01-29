using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelManagerBehavior : MonoBehaviour
{
	public Sprite[] MapTemplates;

	// ---- AUDIOS ---- //
	public int MenuBip01AudioFileID;
	public int MenuBip02AudioFileID;
	// ---- AUDIOS ---- //

	private GameObject _p1BannerMapName, _p1MapDescription, _p1SelectAndConfirmButton;
	private GameObject _p2BannerMapName, _p2MapDescription, _p2SelectAndConfirmButton;
	private GameObject _mapTemplate, _mapTemplateContainer;

	private bool _p1Confirm;
	private bool _p2Confirm;

	void Start ()
	{
		AndroidNativeAudio.makePool();
		_p1BannerMapName = GameObject.Find ("P1BannerMapName");
		_p1MapDescription = GameObject.Find ("P1MapDescription");
		_p1SelectAndConfirmButton = GameObject.Find ("P1SelectAndConfirmButton");

		_p2BannerMapName = GameObject.Find ("P2BannerMapName");
		_p2MapDescription = GameObject.Find ("P2MapDescription");
		_p2SelectAndConfirmButton = GameObject.Find ("P2SelectAndConfirmButton");

		_mapTemplate = GameObject.Find ("MapTemplate");
		_mapTemplateContainer = GameObject.Find ("MapTemplateContainer");

		// ---- AUDIOS ---- //
		MenuBip01AudioFileID = AndroidNativeAudio.load("MenuBip01.mp3");
		MenuBip02AudioFileID = AndroidNativeAudio.load("MenuBip02.mp3");
		// ---- AUDIOS ---- //

		PlayerPrefs.SetInt ("SelectedMap", 0);

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

	public void ChangeSelectedMap(Direction direction)
	{
		var map = PlayerPrefs.GetInt ("SelectedMap");
		if (direction == Direction.Left)
			--map;
		else
			++map;
		if (map == -1)
			map = MapTemplates.Length - 1;
		else if (map >= MapTemplates.Length)
			map = 0;

		PlayerPrefs.SetInt ("SelectedMap", map);
		_p1Confirm = false;
		_p2Confirm = false;
		_mapTemplateContainer.GetComponent<SpriteRenderer> ().sprite = MapTemplates [map];
		if (direction == Direction.Left)
			StartCoroutine(InitiateLeft());
		else
			StartCoroutine(InitiateRight());
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
		GameObject.Find ("P" + player + "-Confirm").GetComponent<MapSelButtonBehavior>().SwitchSprite();

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

	private void LoadGameScene()
	{
		SceneManager.LoadScene("GameLoadingScene");
	}

	void OnDestroy()
	{
		AndroidNativeAudio.unload(MenuBip01AudioFileID);
		AndroidNativeAudio.unload(MenuBip02AudioFileID);
		AndroidNativeAudio.releasePool();
	}
}
