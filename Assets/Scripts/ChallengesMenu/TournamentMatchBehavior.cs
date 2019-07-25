using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TournamentMatchBehavior : MonoBehaviour
{
	public Sprite[] FLags;
	public Sprite[] Characters;

	private List<int> _charactersBag;
	private List<int> _mapsBag;
	private List<int> _opponents;
	private List<int> _maps;

	void Start ()
	{
		var tournamentOpponent = PlayerPrefs.GetInt ("TournamentOpponent", 1);
		if (tournamentOpponent == 1)
			SetTournament (tournamentOpponent);
		Invoke ("ContinueTournament", 4.0f);
	}

	private void SetTournament (int tournamentOpponent)
	{
		var playerCharacterId = PlayerPrefs.GetInt ("P1Character");
		_mapsBag = new List<int> (){ 1, 2, 3, 4, 5, 6 };
		_charactersBag = new List<int> (){ 1, 2, 3, 4, 5, 6 };
		_charactersBag.RemoveAt (playerCharacterId - 1);
		int nbOpponents = 3;
		if (PlayerPrefs.GetInt ("CurrentChallengeDifficulty") == 2)
			nbOpponents = 4;
		else if (PlayerPrefs.GetInt ("CurrentChallengeDifficulty") >= 3)
			nbOpponents = 5;
		_opponents = new List<int> ();
		for (int i = 0; i < nbOpponents; ++i) {
			var randomId = Random.Range (0, _charactersBag.Count);
			_opponents.Add (_charactersBag[randomId]);
			_charactersBag.RemoveAt (randomId);
		}
		_maps = new List<int> ();
		for (int i = 0; i < nbOpponents; ++i) {
			var randomId = Random.Range (0, _mapsBag.Count);
			_maps.Add (_mapsBag[randomId]);
			_mapsBag.RemoveAt (randomId);
		}
		GameObject.Find ("P1CharacterSprite").GetComponent<SpriteRenderer>().sprite = Characters[playerCharacterId];
		GameObject.Find ("P1Character").GetComponent<Animator> ().Play ("CharSelLeftToRight");
		var versusListItemModel = Resources.Load<GameObject> ("Prefabs/VersusListItem");
		for (int i = 0; i < nbOpponents; ++i) {
			var versusListItemInstance = Instantiate (versusListItemModel, new Vector3(0.0f, 1.5f - i * 0.5f, 0.0f), versusListItemModel.transform.rotation);
			if (i == tournamentOpponent - 1) {
				versusListItemInstance.transform.GetChild (1).GetComponent<SpriteRenderer> ().sprite = FLags [playerCharacterId - 1];
				GameObject.Find ("P2CharacterSprite").GetComponent<SpriteRenderer> ().sprite = Characters [_opponents [i]];
				GameObject.Find ("P2Character").GetComponent<Animator> ().Play ("CharSelRightToLeft");
			} else {
				versusListItemInstance.transform.GetChild (0).gameObject.SetActive (false);
				versusListItemInstance.transform.GetChild (1).gameObject.SetActive (false);
			}
			versusListItemInstance.transform.GetChild (2).GetComponent<SpriteRenderer>().sprite = FLags[_opponents[i] - 1];
			versusListItemInstance.transform.SetParent (GameObject.Find ("Canvas").transform);
		}
		var opponentsString = "";
		foreach (int id in _opponents) {
			opponentsString += id;
		}
		var mapsString = "";
		foreach (int id in _maps) {
			mapsString += id;
		}
		PlayerPrefs.SetString ("TournamentOpponents", opponentsString);
		PlayerPrefs.SetString ("TournamentMaps", mapsString);
	}

	private void ContinueTournament ()
	{
		var opponent = PlayerPrefs.GetInt ("TournamentOpponent");
		var opponents = PlayerPrefs.GetString ("TournamentOpponents");
		var maps = PlayerPrefs.GetString ("TournamentMaps");

		Debug.Log ("OPPONENTS : " + opponents);
		Debug.Log ("MAPS : " + maps);

		PlayerPrefs.SetInt ("P2Character", int.Parse(opponents.Substring(opponent - 1, 1)));
		SceneManager.LoadScene("Map"+int.Parse(maps.Substring(opponent - 1, 1)).ToString("D2"));
	}
}
