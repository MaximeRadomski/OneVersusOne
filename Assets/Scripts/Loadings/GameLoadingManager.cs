﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoadingManager : MonoBehaviour
{
	public Sprite[] MapTemplates;
	public Sprite[] CharactersSprites;
	public Sprite[] GameModeSprites;
	public AudioSource StageMusic;

	private GameObject _p1BannerPlayerName, _p1PlayerInfo, _p1LoadingContainer, _p1CharacterSprite;
	private GameObject _p2BannerPlayerName, _p2PlayerInfo, _p2LoadingContainer, _p2CharacterSprite;
	private GameObject _mapTemplateContainer;

	void Start ()
	{
		var menuBackground = GameObject.FindGameObjectWithTag ("MenuBackground");
		menuBackground.GetComponent<AudioSource> ().Stop ();
		Destroy(GameObject.Find ("$GenericMenuManager"));
		if (PlayerPrefs.GetInt ("Music", 1) == 0 && StageMusic != null)
			StageMusic.volume = 0.0f;
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
		map = CheckMap (map);
		if (map != -1)
			_mapTemplateContainer.GetComponent<SpriteRenderer> ().sprite = MapTemplates [map];
		else {
			_mapTemplateContainer.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
			GameObject.Find("MapTemplateBackground").GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
		}

		int p1Character = PlayerPrefs.GetInt ("P1Character");
		p1Character = CheckCharacter (1, p1Character);
		_p1CharacterSprite.GetComponent<SpriteRenderer> ().sprite = CharactersSprites[p1Character];

		int p2Character = PlayerPrefs.GetInt ("P2Character");
		p2Character = CheckCharacter (2, p2Character, p1Character);
		_p2CharacterSprite.GetComponent<SpriteRenderer> ().sprite = CharactersSprites[p2Character];

		_p1LoadingContainer.GetComponent<Animator> ().Play ("MapSelButtons");
		_p2LoadingContainer.GetComponent<Animator> ().Play ("MapSelButtons");

		_p1BannerPlayerName.GetComponent<Animator> ().Play ("CharSelLeftToRight");
		_p2BannerPlayerName.GetComponent<Animator> ().Play ("CharSelLeftToRight");

		_p1PlayerInfo.GetComponent<Animator> ().Play ("MapSelDescLeftToRight");
		if (PlayerPrefs.GetInt ("Opponent") == Opponent.Player.GetHashCode () ||
			PlayerPrefs.GetInt ("Opponent") == Opponent.AI.GetHashCode ())
			_p2PlayerInfo.GetComponent<Animator> ().Play ("MapSelDescLeftToRight");
		else
			GameObject.Find ("P2Wall").GetComponent<Animator> ().Play ("MapSelDescLeftToRight");

		if (PlayerPrefs.GetInt ("GameMode") == GameMode.Target.GetHashCode ())
			GameObject.Find ("P2WallSprite").GetComponent<SpriteRenderer>().sprite = GameModeSprites[1];
		else if (PlayerPrefs.GetInt ("GameMode") == GameMode.Catch.GetHashCode ())
			GameObject.Find ("P2WallSprite").GetComponent<SpriteRenderer>().sprite = GameModeSprites[2];
		else if (PlayerPrefs.GetInt ("GameMode") == GameMode.Breakout.GetHashCode ())
			GameObject.Find ("P2WallSprite").GetComponent<SpriteRenderer>().sprite = GameModeSprites[3];

		if (PlayerPrefs.GetInt ("Opponent") != Opponent.Player.GetHashCode ())
		{
			_p2BannerPlayerName.transform.GetChild (1).transform.Rotate(new Vector3(0.0f, 0.0f, 180.0f));
			_p2BannerPlayerName.transform.GetChild (1).GetComponent<UnityEngine.UI.Text>().alignment = TextAnchor.MiddleRight;
		}

		Invoke ("LoadGameScene", 2.5f);
	}

	private int CheckCharacter (int player, int characterNumber, int p1Character = -1)
	{
		if (characterNumber == 0)
		{
			int tmpCharacterNumber = Random.Range (1, 7);

			if (p1Character != -1 && tmpCharacterNumber == p1Character)
				tmpCharacterNumber = tmpCharacterNumber + 1 > 6 ? 1 : tmpCharacterNumber + 1;
			PlayerPrefs.SetInt ("P" + player.ToString() + "Character", tmpCharacterNumber);
			return tmpCharacterNumber;
		}
		else
			return characterNumber;
	}

	private int CheckMap (int mapNumber)
	{
		if (mapNumber == 0)
		{
			int tmpMapNumber = Random.Range (1, MapTemplates.Length);
			PlayerPrefs.SetInt ("SelectedMap", tmpMapNumber);
			return tmpMapNumber;
		}
		else
			return mapNumber;
	}

	private void LoadGameScene()
	{
		var map = PlayerPrefs.GetInt ("SelectedMap");
		if (map == -1)
			SceneManager.LoadScene("HowToPlayMenu");
		else
			SceneManager.LoadScene("Map"+map.ToString("D2"));
	}
}
