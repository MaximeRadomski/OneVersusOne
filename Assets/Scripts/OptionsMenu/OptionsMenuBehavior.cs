using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuBehavior : MonoBehaviour
{
	//public GameObject PopupConfirm;

	private GameObject _gameTitle, _graphicsTitle, _audioTitle;
	private GameObject _gameButtons, _graphicsButtons, _audioButtons;
	//private GameObject _tmpPopup;
	//private GenericMenuManagerBehavior _genericMenuManagerBehavior;

	private bool _ads;
	private bool _scanlines;
	private bool _music;
	private bool _effects;

	private bool _isDisplayingPopup;

	void Start ()
	{
		_gameTitle = GameObject.Find ("Game");
		_graphicsTitle = GameObject.Find ("Graphics");
		_audioTitle = GameObject.Find ("Audio");
		_gameButtons = GameObject.Find ("GameButtons");
		_graphicsButtons = GameObject.Find ("GraphicsButtons");
		_audioButtons = GameObject.Find ("AudioButtons");
		//_genericMenuManagerBehavior = GameObject.Find ("$GenericMenuManager").GetComponent<GenericMenuManagerBehavior>();

		SetAds ();
		GameObject.Find ("AdsButtonBackground").GetComponent<GenericMenuButtonBehavior>().buttonDelegate = SetAds;
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
	}

	private void SetAds ()
	{
		_ads = !_ads;
		if (_ads) {
			GameObject.Find ("AdsButtonText").GetComponent<UnityEngine.UI.Text> ().text = "ON";
			GameObject.Find ("AdsButtonBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOn ();
		} else {
			GameObject.Find ("AdsButtonText").GetComponent<UnityEngine.UI.Text> ().text = "OFF";
			GameObject.Find ("AdsButtonBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		}
	}

	private void SetScanlines ()
	{
		_scanlines = !_scanlines;
		if (_scanlines) {
			GameObject.Find ("ScanlinesButtonText").GetComponent<UnityEngine.UI.Text> ().text = "ON";
			GameObject.Find ("ScanlinesButtonBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOn ();
		} else {
			GameObject.Find ("ScanlinesButtonText").GetComponent<UnityEngine.UI.Text> ().text = "OFF";
			GameObject.Find ("ScanlinesButtonBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		}
	}

	private void SetMusic ()
	{
		_music = !_music;
		if (_music) {
			GameObject.Find ("MusicButtonText").GetComponent<UnityEngine.UI.Text> ().text = "ON";
			GameObject.Find ("MusicButtonBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOn ();
		} else {
			GameObject.Find ("MusicButtonText").GetComponent<UnityEngine.UI.Text> ().text = "OFF";
			GameObject.Find ("MusicButtonBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		}
	}

	private void SetEffects ()
	{
		_effects = !_effects;
		if (_effects) {
			GameObject.Find ("EffectsButtonText").GetComponent<UnityEngine.UI.Text> ().text = "ON";
			GameObject.Find ("EffectsButtonBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOn ();
		} else {
			GameObject.Find ("EffectsButtonText").GetComponent<UnityEngine.UI.Text> ().text = "OFF";
			GameObject.Find ("EffectsButtonBackground").GetComponent<GenericMenuButtonBehavior> ().SetSpriteOff();
		}
	}
}
