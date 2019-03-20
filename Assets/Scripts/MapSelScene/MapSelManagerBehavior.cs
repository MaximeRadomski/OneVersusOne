using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelManagerBehavior : MonoBehaviour
{
	public Sprite[] MapTemplates;

	private GameObject _p1BannerMapName, _p1MapDescription, _p1SelectAndConfirmButton;
	private GameObject _p2BannerMapName, _p2MapDescription, _p2SelectAndConfirmButton;

	private GameObject _p1MapName, _p1Info1, _p1Info2, _p1Info4;
	private GameObject _p2MapName, _p2Info1, _p2Info2, _p2Info4;

	private GameObject _mapTemplate, _mapTemplateContainer;

	private bool _p1Confirm;
	private bool _p2Confirm;

	void Start ()
	{
		//Remove "PLAYERTWO" if Opponent is not a Player
		if (PlayerPrefs.GetInt ("Opponent") != Opponent.Player.GetHashCode ())
		{
			GameObject.Find ("PLAYER TWO").SetActive(false);
		}

		_p1BannerMapName = GameObject.Find ("P1BannerMapName");
		_p1MapDescription = GameObject.Find ("P1MapDescription");
		_p1SelectAndConfirmButton = GameObject.Find ("P1SelectAndConfirmButton");

		_p1MapName = GameObject.Find ("P1MapName");
		_p1Info1 = GameObject.Find ("P1Info1");
		_p1Info2 = GameObject.Find ("P1Info2");
		_p1Info4 = GameObject.Find ("P1Info4");
		_p1SelectAndConfirmButton.GetComponent<Animator> ().Play ("MapSelButtons");

		if (PlayerPrefs.GetInt ("Opponent") == Opponent.Player.GetHashCode ())
		{
			_p2BannerMapName = GameObject.Find ("P2BannerMapName");
			_p2MapDescription = GameObject.Find ("P2MapDescription");
			_p2SelectAndConfirmButton = GameObject.Find ("P2SelectAndConfirmButton");

			_p2MapName = GameObject.Find ("P2MapName");
			_p2Info1 = GameObject.Find ("P2Info1");
			_p2Info2 = GameObject.Find ("P2Info2");
			_p2Info4 = GameObject.Find ("P2Info4");
			_p2SelectAndConfirmButton.GetComponent<Animator> ().Play ("MapSelButtons");
		}

		_mapTemplate = GameObject.Find ("MapTemplate");
		_mapTemplateContainer = GameObject.Find ("MapTemplateContainer");

		PlayerPrefs.SetInt ("SelectedMap", 1);

		StartCoroutine(InitiateLeft(true));
		StartCoroutine (ChangeMapInfo (1));
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
		if (_p2BannerMapName != null)
			_p2BannerMapName.GetComponent<Animator> ().Play ("StartNameBanner");
		_p1MapDescription.GetComponent<Animator> ().Play ("StartLeftToRight");
		if (_p2MapDescription != null)
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
		if (_p2BannerMapName != null)
			_p2BannerMapName.GetComponent<Animator> ().Play ("StartNameBanner");
		_p1MapDescription.GetComponent<Animator> ().Play ("StartLeftToRight");
		if (_p2MapDescription != null)
			_p2MapDescription.GetComponent<Animator> ().Play ("StartLeftToRight");
		yield return new WaitForSeconds(0.5f);
		Destroy (tmpMapTemplate);
		EnableButtons ();
	}

	private IEnumerator ChangeMapInfo(int map)
	{
		yield return new WaitForSeconds(0.1f);

		_p1MapName.GetComponent<UnityEngine.UI.Text> ().text = MapsData.Maps[map].Name;
		_p1Info1.GetComponent<UnityEngine.UI.Text> ().text = MapsData.Maps[map].Country;
		_p1Info2.GetComponent<UnityEngine.UI.Text> ().text = MapsData.Maps[map].City;
		_p1Info4.GetComponent<UnityEngine.UI.Text> ().text = "EFFECT: " + MapsData.Maps[map].Effect;

		if (_p2MapName != null)
		{
			_p2MapName.GetComponent<UnityEngine.UI.Text> ().text = MapsData.Maps[map].Name;
			_p2Info1.GetComponent<UnityEngine.UI.Text> ().text = MapsData.Maps[map].Country;
			_p2Info2.GetComponent<UnityEngine.UI.Text> ().text = MapsData.Maps[map].City;
			_p2Info4.GetComponent<UnityEngine.UI.Text> ().text = "EFFECT: " + MapsData.Maps[map].Effect;
		}

		_mapTemplateContainer.GetComponent<SpriteRenderer> ().sprite = MapTemplates [map];
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
		else if (map > MapTemplates.Length - 1)
			map = 0;

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
		if (tmpConfirm != null)
			tmpConfirm.GetComponent<MapSelButtonBehavior> ().SpriteRenderer.sprite = tmpConfirm.GetComponent<MapSelButtonBehavior> ().SpriteOff;
	}

	public void Confirm(int player)
	{
		if (player == 1)
			_p1Confirm = !_p1Confirm;
		else
			_p2Confirm = !_p2Confirm;

		if (_p1Confirm && _p2Confirm ||
			(PlayerPrefs.GetInt ("Opponent") != Opponent.Player.GetHashCode () && _p1Confirm))
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
}
