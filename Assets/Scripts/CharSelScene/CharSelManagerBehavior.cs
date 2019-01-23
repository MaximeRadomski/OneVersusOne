using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CharSelManagerBehavior : MonoBehaviour
{
	private GameObject _p1BannerPlayerName, _p1LightCharacters, _p1MediumCharacters, _p1HeavyCharacters;
	private GameObject _p1CharacterImage, _p1BannerCharacterName, _p1Skill, _p1ConfirmButton;

	private GameObject _p2BannerPlayerName, _p2LightCharacters, _p2MediumCharacters, _p2HeavyCharacters;
	private GameObject _p2CharacterImage, _p2BannerCharacterName, _p2Skill, _p2ConfirmButton;

	private bool _p1Confirm;
	private bool _p2Confirm;

	void Start ()
	{
		_p1BannerPlayerName = GameObject.Find ("P1BannerPlayerName");
		_p1LightCharacters = GameObject.Find ("P1LightCharacters");
		_p1MediumCharacters = GameObject.Find ("P1MediumCharacters");
		_p1HeavyCharacters = GameObject.Find ("P1HeavyCharacters");
		_p1CharacterImage = GameObject.Find ("P1CharacterImage");
		_p1BannerCharacterName = GameObject.Find ("P1BannerCharacterName");
		_p1Skill = GameObject.Find ("P1Skill");
		_p1ConfirmButton = GameObject.Find ("P1ConfirmButton");

		StartCoroutine(InitiateLeft(_p1BannerPlayerName, _p1LightCharacters, _p1MediumCharacters, _p1HeavyCharacters));
		StartCoroutine(InitiateRight(_p1CharacterImage, _p1BannerCharacterName, _p1Skill, _p1ConfirmButton));

		_p2BannerPlayerName = GameObject.Find ("P2BannerPlayerName");
		_p2LightCharacters = GameObject.Find ("P2LightCharacters");
		_p2MediumCharacters = GameObject.Find ("P2MediumCharacters");
		_p2HeavyCharacters = GameObject.Find ("P2HeavyCharacters");
		_p2CharacterImage = GameObject.Find ("P2CharacterImage");
		_p2BannerCharacterName = GameObject.Find ("P2BannerCharacterName");
		_p2Skill = GameObject.Find ("P2Skill");
		_p2ConfirmButton = GameObject.Find ("P2ConfirmButton");

		StartCoroutine(InitiateLeft(_p2BannerPlayerName, _p2LightCharacters, _p2MediumCharacters, _p2HeavyCharacters));
		StartCoroutine(InitiateRight(_p2CharacterImage, _p2BannerCharacterName, _p2Skill, _p2ConfirmButton));

		_p1Confirm = false;
		_p2Confirm = false;

		PlayerPrefs.SetInt ("P1Character", 0);
		PlayerPrefs.SetInt ("P2Character", 0);
		ChangeSelectedCharacter(1, 1);
		ChangeSelectedCharacter(2, 1);
	}

	private IEnumerator InitiateLeft(GameObject A, GameObject B, GameObject C, GameObject D)
	{
		A.GetComponent<Animator> ().Play ("CharSelLeftToRight");
		yield return new WaitForSeconds(0.1f);
		B.GetComponent<Animator> ().Play ("CharSelLeftToRight");
		yield return new WaitForSeconds(0.1f);
		C.GetComponent<Animator> ().Play ("CharSelLeftToRight");
		yield return new WaitForSeconds(0.1f);
		D.GetComponent<Animator> ().Play ("CharSelLeftToRight");
	}

	private IEnumerator InitiateRight(GameObject A, GameObject B, GameObject C, GameObject D)
	{
		A.GetComponent<Animator> ().Play ("Empty");
		B.GetComponent<Animator> ().Play ("Empty");
		C.GetComponent<Animator> ().Play ("Empty");
		D.GetComponent<Animator> ().Play ("Empty");
		yield return new WaitForSeconds(0.1f);
	}

	public void ChangeSelectedCharacter(int player, int character)
	{
		int lastCharacter = PlayerPrefs.GetInt ("P" + player.ToString () + "Character");
		PlayerPrefs.SetInt ("P" + player.ToString() + "Character", character);
		if (lastCharacter != character)
		{
			if (player == 1)
			{
				_p1Confirm = false;
				StartCoroutine(InitiateRight(_p1CharacterImage, _p1BannerCharacterName, _p1Skill, _p1ConfirmButton));
			}
			else
			{
				_p2Confirm = false;
				StartCoroutine(InitiateRight(_p2CharacterImage, _p2BannerCharacterName, _p2Skill, _p2ConfirmButton));
			}
			var tmpConfirm = GameObject.Find ("P" + player + "-Confirm");
			tmpConfirm.GetComponent<CharSelButtonBehavior> ().SpriteRenderer.sprite = tmpConfirm.GetComponent<CharSelButtonBehavior> ().SpriteOff;
			if (lastCharacter != 0)
			{
				var tmpLastButton = GameObject.Find ("P" + player + "-" + lastCharacter);
				tmpLastButton.GetComponent<CharSelButtonBehavior> ().SpriteRenderer.sprite = tmpLastButton.GetComponent<CharSelButtonBehavior> ().SpriteOff;
			}
			var tmpButton = GameObject.Find ("P" + player + "-" + character);
			tmpButton.GetComponent<CharSelButtonBehavior> ().SpriteRenderer.sprite = tmpButton.GetComponent<CharSelButtonBehavior> ().SpriteOn;
		}
	}

	public void Confirm(int player)
	{
		if (player == 1)
			_p1Confirm = !_p1Confirm;
		else
			_p2Confirm = !_p2Confirm;
		GameObject.Find ("P" + player + "-Confirm").GetComponent<CharSelButtonBehavior>().SwitchSprite();

		if (_p1Confirm && _p2Confirm)
		{
			var tmpButtons = GameObject.FindGameObjectsWithTag("Button");
			foreach (var button in tmpButtons)
			{
				button.GetComponent<BoxCollider2D> ().enabled = false;
			}
			Invoke ("LoadGameScene", 0.25f);
		}
	}

	private void LoadGameScene()
	{
		SceneManager.LoadScene("GameScene");
	}
}
