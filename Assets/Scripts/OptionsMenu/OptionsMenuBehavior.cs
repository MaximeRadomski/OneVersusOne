using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuBehavior : MonoBehaviour
{
	//public GameObject PopupConfirm;

	private GameObject _gameTitle, _graphicsTitle, _audioTitle;
	private GameObject _gameButtons, _graphicsButtons, _audioButtons;
	private GameObject _scanLinesObject;
	private GameObject _whyAdsContent;
	//private GameObject _tmpPopup;
	//private GenericMenuManagerBehavior _genericMenuManagerBehavior;

	private int _ads;
	private int _showBack;
	private int _scanlines;
	private int _music;
	private int _effects;

	private bool _isDisplayingPopup;
	private bool _init;

	void Start ()
	{
		_gameTitle = GameObject.Find ("Game");
		_graphicsTitle = GameObject.Find ("Graphics");
		_audioTitle = GameObject.Find ("Audio");
		_gameButtons = GameObject.Find ("GameButtons");
		_graphicsButtons = GameObject.Find ("GraphicsButtons");
		_audioButtons = GameObject.Find ("AudioButtons");
		_scanLinesObject = GameObject.Find ("ScanLines");
		_whyAdsContent = GameObject.Find ("WhyAdsContent");
		//_genericMenuManagerBehavior = GameObject.Find ("$GenericMenuManager").GetComponent<GenericMenuManagerBehavior>();

		_ads = PlayerPrefs.GetInt ("Ads", 1);
		_showBack = PlayerPrefs.GetInt ("ShowBack", 0);
		_scanlines = PlayerPrefs.GetInt ("ScanLines", 1);
		_music = PlayerPrefs.GetInt ("Music", 1);
		_effects = PlayerPrefs.GetInt ("Effects", 1);

		_init = true;
		SetAds ();
		SetScanlines ();
		SetMusic ();
		SetEffects ();
		_init = false;

		GameObject.Find ("AdsButtonBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = SetAds;
		GameObject.Find ("ShowBackButtonBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = SetShowBack;
		GameObject.Find ("ScanlinesButtonBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = SetScanlines;
		GameObject.Find ("MusicButtonBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = SetMusic;
		GameObject.Find ("EffectsButtonBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = SetEffects;

		//_tmpPopup = null;

		StartCoroutine (InitiateLeft());
	}

	private IEnumerator InitiateLeft()
	{
		_gameTitle.GetComponent<Animator> ().Play ("CharSelLeftToRight");
		yield return new WaitForSeconds(0.05f);
		_gameButtons.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
		yield return new WaitForSeconds(0.05f);
		_graphicsTitle.GetComponent<Animator> ().Play ("CharSelLeftToRight");
		yield return new WaitForSeconds(0.05f);
		_graphicsButtons.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
		yield return new WaitForSeconds(0.05f);
		_audioTitle.GetComponent<Animator> ().Play ("CharSelLeftToRight");
		yield return new WaitForSeconds(0.05f);
		_audioButtons.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
		yield return new WaitForSeconds(1.0f);
		_whyAdsContent.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
	}

	private void SetAds ()
	{
		if (!_init)
			_ads = _ads == 1 ? 0 : 1;
		if (_ads == 1) {
			GameObject.Find ("AdsButtonText").GetComponent<UnityEngine.UI.Text> ().text = "ON";
			GameObject.Find ("AdsButtonBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOn ();
		} else {
			GameObject.Find ("AdsButtonText").GetComponent<UnityEngine.UI.Text> ().text = "OFF";
			GameObject.Find ("AdsButtonBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		}
		PlayerPrefs.SetInt("Ads", _ads);
	}

	private void SetShowBack ()
	{
		if (!_init)
			_showBack = _showBack == 1 ? 0 : 1;
		if (_showBack == 1) {
			GameObject.Find ("ShowBackButtonText").GetComponent<UnityEngine.UI.Text> ().text = "ON";
			GameObject.Find ("ShowBackButtonBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOn ();
			GameObject.Find ("BackButton").GetComponent<BackButtonBehavior> ().Enable ();
		} else {
			GameObject.Find ("ShowBackButtonText").GetComponent<UnityEngine.UI.Text> ().text = "OFF";
			GameObject.Find ("ShowBackButtonBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
			GameObject.Find ("BackButton").GetComponent<BackButtonBehavior> ().Disable ();
		}
		PlayerPrefs.SetInt("ShowBack", _showBack);
	}

	private void SetScanlines ()
	{
		if (!_init)
			_scanlines = _scanlines + 1 > 4 ? 0 : _scanlines + 1;
		if (_scanlines > 0) {
			GameObject.Find ("ScanlinesButtonText").GetComponent<UnityEngine.UI.Text> ().text = _scanlines.ToString ();
			GameObject.Find ("ScanlinesButtonBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOn ();
		} else {
			GameObject.Find ("ScanlinesButtonText").GetComponent<UnityEngine.UI.Text> ().text = "OFF";
			GameObject.Find ("ScanlinesButtonBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		}
		var newOpacity = (float)_scanlines * 0.10f;
		if (_scanLinesObject != null)
			GameObject.Find ("ScanLines").GetComponent<ScanLinesBehaviours> ().SetOpacity (newOpacity);
		PlayerPrefs.SetInt("ScanLines", _scanlines);
	}

	private void SetMusic ()
	{
		if (!_init)
			_music = _music == 1 ? 0 : 1;
		if (_music == 1) {
			GameObject.Find ("MusicButtonText").GetComponent<UnityEngine.UI.Text> ().text = "ON";
			GameObject.Find ("MusicButtonBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOn ();
			GameObject.Find("MovingBackground").GetComponent<AudioSource> ().volume = 1.0f;
		} else {
			GameObject.Find ("MusicButtonText").GetComponent<UnityEngine.UI.Text> ().text = "OFF";
			GameObject.Find ("MusicButtonBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
			GameObject.Find("MovingBackground").GetComponent<AudioSource> ().volume = 0.0f;
		}
		PlayerPrefs.SetInt("Music", _music);
	}

	private void SetEffects ()
	{
		if (!_init)
			_effects = _effects == 1 ? 0 : 1;
		if (_effects == 1) {
			GameObject.Find ("EffectsButtonText").GetComponent<UnityEngine.UI.Text> ().text = "ON";
			GameObject.Find ("EffectsButtonBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOn ();
		} else {
			GameObject.Find ("EffectsButtonText").GetComponent<UnityEngine.UI.Text> ().text = "OFF";
			GameObject.Find ("EffectsButtonBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		}
		PlayerPrefs.SetInt("Effects", _effects);
	}
}
