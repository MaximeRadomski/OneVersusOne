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
		Destroy(GameObject.Find ("$GenericMenuManager"));
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
		_mapTemplateContainer.GetComponent<SpriteRenderer> ().sprite = MapTemplates [map];

		int p1Character = PlayerPrefs.GetInt ("P1Character");
		p1Character = CheckCharacter (1, p1Character);
		_p1CharacterSprite.GetComponent<SpriteRenderer> ().sprite = CharactersSprites[p1Character];

		int p2Character = PlayerPrefs.GetInt ("P2Character");
		p2Character = CheckCharacter (2, p2Character);
		_p2CharacterSprite.GetComponent<SpriteRenderer> ().sprite = CharactersSprites[p2Character];

		_p1LoadingContainer.GetComponent<Animator> ().Play ("MapSelButtons");
		_p2LoadingContainer.GetComponent<Animator> ().Play ("MapSelButtons");

		_p1BannerPlayerName.GetComponent<Animator> ().Play ("CharSelLeftToRight");
		_p2BannerPlayerName.GetComponent<Animator> ().Play ("CharSelLeftToRight");

		_p1PlayerInfo.GetComponent<Animator> ().Play ("MapSelDescLeftToRight");
		if (PlayerPrefs.GetInt ("Opponent") != Opponent.Wall.GetHashCode ())
			_p2PlayerInfo.GetComponent<Animator> ().Play ("MapSelDescLeftToRight");
		else
			GameObject.Find ("P2Wall").GetComponent<Animator> ().Play ("MapSelDescLeftToRight");

		if (PlayerPrefs.GetInt ("Opponent") != Opponent.Player.GetHashCode ())
		{
			_p2BannerPlayerName.transform.GetChild (1).transform.Rotate(new Vector3(0.0f, 0.0f, 180.0f));
			_p2BannerPlayerName.transform.GetChild (1).GetComponent<UnityEngine.UI.Text>().alignment = TextAnchor.MiddleRight;
			_p2BannerPlayerName.transform.GetChild (1).transform.position += new Vector3 (0.0f, 0.02f, 0.0f);
		}

		Invoke ("LoadGameScene", 2.0f);
	}

	private int CheckCharacter (int player, int characterNumber)
	{
		if (characterNumber == 0)
		{
			int tmpCharacterNumber = Random.Range (1, 7);
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
			int tmpMapNumber = Random.Range (1, 3);
			PlayerPrefs.SetInt ("SelectedMap", tmpMapNumber);
			return tmpMapNumber;
		}
		else
			return mapNumber;
	}

	private void LoadGameScene()
	{
		var map = PlayerPrefs.GetInt ("SelectedMap");
		SceneManager.LoadScene("Map"+map.ToString("D2"));
	}
}
