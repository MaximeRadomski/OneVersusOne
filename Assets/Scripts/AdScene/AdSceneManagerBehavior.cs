using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdSceneManagerBehavior : MonoBehaviour
{
	private GameObject _gameIcon, _gameName, _gameImage, _gameDescription;
	private GameObject _leaveButton;

	private int _adId;

	void Start ()
	{
		_gameIcon = GameObject.Find ("GameIcon");
		_gameName = GameObject.Find ("GameName");
		_gameImage = GameObject.Find ("GameImage");
		_gameDescription = GameObject.Find ("GameDescriptionText");
		_leaveButton = GameObject.Find ("LeaveButton");

		StartCoroutine (InitiateLeft());
		SetAd ();
	}

	private void SetAd()
	{
		_adId = PlayerPrefs.GetInt ("CurrentAd", 0);
		PlayerPrefs.SetInt ("CurrentAd", _adId + 1 < AdsData.GameAds.Count ? _adId + 1 : 0);

		var folderName = AdsData.GameAds [_adId].Name.Replace (" ", "");
		_gameIcon.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite> ("Ads/" + folderName + "/GameIcon");
		_gameName.GetComponent<UnityEngine.UI.Text> ().text = AdsData.GameAds [_adId].Name;
		_gameImage.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite> ("Ads/" + folderName + "/GameImage");
		_gameDescription.GetComponent<UnityEngine.UI.Text> ().text = AdsData.GameAds [_adId].Description;
	}

	private IEnumerator InitiateLeft()
	{
		yield return new WaitForSeconds(1.5f);
		_leaveButton.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
	}

	void Update()
	{
		if (Input.GetMouseButtonDown (0)) {
			//We transform the touch position into word space from screen space and store it.
			var touchPosWorld = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			if (touchPosWorld.y > -2.0f)
				OpenCustomUrl ();
			else if (touchPosWorld.x > 1.0f)
				SetAd ();
		}
	}

	private void OpenCustomUrl()
	{
		if (AdsData.GameAds [_adId].IsMobileGame)
			Application.OpenURL ("market://details?id=" + AdsData.GameAds [_adId].UrlId);
		else
			Application.OpenURL (AdsData.GameAds [_adId].UrlId);
	}
}
