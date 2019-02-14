using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoadingManager : MonoBehaviour
{
	public Sprite[] MapTemplates;
	public Sprite[] CharactersSprites;

	private GameObject _p1BannerPlayerName, _p1PlayerInfo, _p1LoadingContainer, _p1CharacterSprite;
	private GameObject _p2BannerPlayerName, _p2PlayerInfo, _p2LoadingContainer, _p2CharacterSprite;
	private GameObject _mapTemplateContainer;

	void Start ()
	{
		_p1BannerPlayerName = GameObject.Find ("P1BannerPlayerName");
		_p1PlayerInfo = GameObject.Find ("P1PlayerInfo");
		_p1LoadingContainer = GameObject.Find ("P1LoadingContainer");
		_p1CharacterSprite = GameObject.Find ("P1CharacterSprite");

		_p2BannerPlayerName = GameObject.Find ("P2BannerPlayerName");
		_p2PlayerInfo = GameObject.Find ("P2PlayerInfo");
		_p2LoadingContainer = GameObject.Find ("P2LoadingContainer");
		_p2CharacterSprite = GameObject.Find ("P2CharacterSprite");

		_mapTemplateContainer = GameObject.Find ("MapTemplateContainer");

		var map = PlayerPrefs.GetInt ("SelectedMap");
		_mapTemplateContainer.GetComponent<SpriteRenderer> ().sprite = MapTemplates [map - 1];

		int p1Character = PlayerPrefs.GetInt ("P1Character");
		_p1CharacterSprite.GetComponent<SpriteRenderer> ().sprite = CharactersSprites[p1Character - 1];

		int p2Character = PlayerPrefs.GetInt ("P2Character");
		_p2CharacterSprite.GetComponent<SpriteRenderer> ().sprite = CharactersSprites[p2Character - 1];

		_p1LoadingContainer.GetComponent<Animator> ().Play ("MapSelButtons");
		_p2LoadingContainer.GetComponent<Animator> ().Play ("MapSelButtons");

		_p1BannerPlayerName.GetComponent<Animator> ().Play ("CharSelLeftToRight");
		_p2BannerPlayerName.GetComponent<Animator> ().Play ("CharSelLeftToRight");

		_p1PlayerInfo.GetComponent<Animator> ().Play ("MapSelDescLeftToRight");
		_p2PlayerInfo.GetComponent<Animator> ().Play ("MapSelDescLeftToRight");

		Invoke ("LoadGameScene", 2.0f);
	}

	private void LoadGameScene()
	{
		var map = PlayerPrefs.GetInt ("SelectedMap");
		SceneManager.LoadScene("Map"+map.ToString("D2"));
	}
}
