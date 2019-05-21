using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CharSelManagerBehavior : MonoBehaviour
{
	public Sprite[] CharactersSprites;

	private GameObject _p1BannerPlayerName, _p1LightCharacters, _p1MediumCharacters, _p1HeavyCharacters;
	private GameObject _p1CharacterImage, _p1BannerCharacterName, _p1Skill, _p1ConfirmButton;

	private GameObject _p2BannerPlayerName, _p2LightCharacters, _p2MediumCharacters, _p2HeavyCharacters;
	private GameObject _p2CharacterImage, _p2BannerCharacterName, _p2Skill, _p2ConfirmButton;

	private GameObject _p1CharacterName, _p1CharacterSkill, _p1CharacterSprite;
	private GameObject _p2CharacterName, _p2CharacterSkill, _p2CharacterSprite;

	private bool _p1Confirm;
	private bool _p2Confirm;
	private int[] _lastCharacter = {-1,-1};
	private bool _isAgainstAI;

	void Start ()
	{
		//Change "PLAYERTWO" Orientation if Opponent is an AI
		if (PlayerPrefs.GetInt ("Opponent") == Opponent.AI.GetHashCode ())
		{
			_isAgainstAI = true;
			var tmpPlayerTwo = GameObject.Find ("PLAYER TWO");
			tmpPlayerTwo.transform.Rotate(new Vector3(0.0f, 0.0f, 180.0f));
			tmpPlayerTwo.transform.position = new Vector3 (0.0f, 3.618f, 0.0f);
			//Change "Player 2" to "AI"
			tmpPlayerTwo.transform.GetChild(0).transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = "AI";
			//Remove the "Confirm" button for AI
			tmpPlayerTwo.transform.GetChild(7).transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
			GameObject.Find ("MenuSeparator").transform.position = new Vector2(0.0f, -0.05f);
		}
		//Remove "PLAYERTWO" if Opponent is a WALL
		else if (PlayerPrefs.GetInt ("Opponent") == Opponent.Wall.GetHashCode ())
		{
			var playerTwoContainer = GameObject.Find ("PLAYER TWO");
			var childCountLessOne = playerTwoContainer.transform.childCount - 1;
			for (int i = 0; i < childCountLessOne; ++i)
				playerTwoContainer.transform.GetChild (i).gameObject.SetActive (false);
			//Up lines replace this
			//GameObject.Find ("PLAYER TWO").SetActive(false);
			GameObject.Find ("P2Wall").GetComponent<Animator> ().Play ("MapSelDescLeftToRight");
		}

		_p1BannerPlayerName = GameObject.Find ("P1BannerPlayerName");
		_p1LightCharacters = GameObject.Find ("P1LightCharacters");
		_p1MediumCharacters = GameObject.Find ("P1MediumCharacters");
		_p1HeavyCharacters = GameObject.Find ("P1HeavyCharacters");
		_p1CharacterImage = GameObject.Find ("P1CharacterImage");
		_p1BannerCharacterName = GameObject.Find ("P1BannerCharacterName");
		_p1Skill = GameObject.Find ("P1Skill");
		_p1ConfirmButton = GameObject.Find ("P1ConfirmButton");

		_p1CharacterName = GameObject.Find ("P1CharacterName");
		_p1CharacterSkill = GameObject.Find ("P1CharacterSkill");
		_p1CharacterSprite = GameObject.Find ("P1CharacterSprite");

		StartCoroutine(InitiateLeft(_p1BannerPlayerName, _p1LightCharacters, _p1MediumCharacters, _p1HeavyCharacters));
		StartCoroutine(InitiateRight(_p1CharacterImage, _p1BannerCharacterName, _p1Skill, _p1ConfirmButton));
		_p1Confirm = false;
		ChangeSelectedCharacter(1, PlayerPrefs.GetInt ("P1Character", 1));

		if (PlayerPrefs.GetInt ("Opponent") != Opponent.Wall.GetHashCode ())
		{
			_p2BannerPlayerName = GameObject.Find ("P2BannerPlayerName");
			_p2LightCharacters = GameObject.Find ("P2LightCharacters");
			_p2MediumCharacters = GameObject.Find ("P2MediumCharacters");
			_p2HeavyCharacters = GameObject.Find ("P2HeavyCharacters");
			_p2CharacterImage = GameObject.Find ("P2CharacterImage");
			_p2BannerCharacterName = GameObject.Find ("P2BannerCharacterName");
			_p2Skill = GameObject.Find ("P2Skill");
			_p2ConfirmButton = GameObject.Find ("P2ConfirmButton");

			_p2CharacterName = GameObject.Find ("P2CharacterName");
			_p2CharacterSkill = GameObject.Find ("P2CharacterSkill");
			_p2CharacterSprite = GameObject.Find ("P2CharacterSprite");

			StartCoroutine(InitiateLeft(_p2BannerPlayerName, _p2LightCharacters, _p2MediumCharacters, _p2HeavyCharacters));
			StartCoroutine(InitiateRight(_p2CharacterImage, _p2BannerCharacterName, _p2Skill, _p2ConfirmButton));
			_p2Confirm = false;
			ChangeSelectedCharacter(2, PlayerPrefs.GetInt ("P2Character", 1));
		}
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

	private IEnumerator ChangeCharacterInfo(CurrentPlayer player, int character)
	{
		yield return new WaitForSeconds(0.1f);
		if (player == CurrentPlayer.PlayerOne)
		{
			_p1CharacterName.GetComponent<UnityEngine.UI.Text> ().text = CharactersData.Characters [character].Name;
			_p1CharacterSkill.GetComponent<UnityEngine.UI.Text> ().text = CharactersData.Characters [character].Skill;
			_p1CharacterSprite.GetComponent<SpriteRenderer> ().sprite = CharactersSprites[character];
		}
		else
		{
			_p2CharacterName.GetComponent<UnityEngine.UI.Text> ().text = CharactersData.Characters [character].Name;
			_p2CharacterSkill.GetComponent<UnityEngine.UI.Text> ().text = CharactersData.Characters [character].Skill;
			_p2CharacterSprite.GetComponent<SpriteRenderer> ().sprite = CharactersSprites[character];
		}
	}

	public void ChangeSelectedCharacter(int player, int character)
	{
		//_lastCharacter[player-1] = PlayerPrefs.GetInt ("P" + player.ToString () + "Character");
		if (_lastCharacter[player-1] == character)
			character = 0;
		PlayerPrefs.SetInt ("P" + player.ToString() + "Character", character);
		if (_lastCharacter [player - 1] != character)
		{
			if (player == 1)
			{
				_p1Confirm = false;
				StartCoroutine(InitiateRight(_p1CharacterImage, _p1BannerCharacterName, _p1Skill, _p1ConfirmButton));
				StartCoroutine(ChangeCharacterInfo(CurrentPlayer.PlayerOne, character));
			}
			else
			{
				_p2Confirm = false;
				StartCoroutine(InitiateRight(_p2CharacterImage, _p2BannerCharacterName, _p2Skill, _p2ConfirmButton));
				StartCoroutine(ChangeCharacterInfo(CurrentPlayer.PlayerTwo, character));
			}
			var tmpConfirm = GameObject.Find ("P" + player + "-Confirm");
			tmpConfirm.GetComponent<CharSelButtonBehavior> ().SpriteRenderer.sprite = tmpConfirm.GetComponent<CharSelButtonBehavior> ().SpriteOff;
			if (_lastCharacter [player - 1] > 0)
			{
				var tmpLastButton = GameObject.Find ("P" + player + "-" + _lastCharacter [player - 1]);
				tmpLastButton.GetComponent<CharSelButtonBehavior> ().SpriteRenderer.sprite = tmpLastButton.GetComponent<CharSelButtonBehavior> ().SpriteOff;
			}
			if (character != 0)
			{
				var tmpButton = GameObject.Find ("P" + player + "-" + character);
				tmpButton.GetComponent<CharSelButtonBehavior> ().SpriteRenderer.sprite = tmpButton.GetComponent<CharSelButtonBehavior> ().SpriteOn;
			}
			_lastCharacter [player - 1] = character;
		}
	}

	public void Confirm(int player)
	{
		if (player == 1) {
			_p1Confirm = !_p1Confirm;
			if (_p1Confirm)
				Intro (1);
		} else {
			_p2Confirm = !_p2Confirm;
			if (_p2Confirm)
				Intro (2);
		}

		if (_p1Confirm && _p2Confirm ||
			(PlayerPrefs.GetInt ("Opponent") != Opponent.Player.GetHashCode () && _p1Confirm))
		{
			var tmpButtons = GameObject.FindGameObjectsWithTag("Button");
			foreach (var button in tmpButtons)
			{
				button.GetComponent<BoxCollider2D> ().enabled = false;
			}
			Invoke ("LoadGameScene", 0.5f);
		}
	}

	private void Intro(int player)
	{
		var introModel = Resources.Load<GameObject> ("Prefabs/Punchline");
		var parentObject = GameObject.Find ("P" + player + "CharacterName");
		var introInstance = Instantiate (introModel, parentObject.transform.position, parentObject.transform.rotation);
		introInstance.transform.SetParent (GameObject.Find("Canvas").transform);
		introInstance.transform.position = parentObject.transform.position + new Vector3(
			Random.Range(player == 1 ? -0.5f : 0.5f, player == 1 ? -1.0f : 1.0f),
			Random.Range(0.0f, player == 1 ? 1.0f : -1.0f),
			0.0f);
		if (_isAgainstAI && player == 2)
			introInstance.transform.GetChild(0).transform.Rotate(0.0f, 0.0f, 180.0f);
		var currentCharacter = PlayerPrefs.GetInt ("P" + player.ToString() + "Character");
		var introsCount = PunchlinesData.Intros [currentCharacter].Count;
		introInstance.transform.GetChild(0).GetComponent<PunchlineBehavior> ().Text = PunchlinesData.Intros[currentCharacter][Random.Range(0, introsCount)];
	}

	private void LoadGameScene()
	{
		SceneManager.LoadScene("MapSelScene");
	}
}
