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
		var tournamentOpponent = PlayerPrefs.GetInt ("TournamentOpponent", 0);
        if (tournamentOpponent == 0)
        {
            ++tournamentOpponent;
            SetTournament();
        }
		DisplayTournament (tournamentOpponent);
		Invoke ("ContinueTournament", 3.0f);
	}

	private void SetTournament ()
	{
        PlayerPrefs.SetInt("TournamentOpponent", 1);
        var playerCharacterId = PlayerPrefs.GetInt ("P1Character");
        if (playerCharacterId == 0)
        {
            playerCharacterId = Random.Range(1,7);
            PlayerPrefs.SetInt("P1Character", playerCharacterId);
        }
		_mapsBag = new List<int> (){ 1, 2, 3, 4, 5, 6 };
		_charactersBag = new List<int> (){ 1, 2, 3, 4, 5, 6 };
		_charactersBag.RemoveAt (playerCharacterId - 1);
		int nbOpponents = PlayerPrefs.GetInt ("NbOpponents");
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

		//Debug.Log ("OPPONENTS : " + opponentsString);
		//Debug.Log ("MAPS : " + mapsString);
	}

	private void DisplayTournament (int tournamentOpponent)
	{
		var playerCharacterId = PlayerPrefs.GetInt ("P1Character");
		int nbOpponents = PlayerPrefs.GetInt ("NbOpponents");
		var opponents = PlayerPrefs.GetString ("TournamentOpponents");
		GameObject.Find ("P1CharacterSprite").GetComponent<SpriteRenderer>().sprite = Characters[playerCharacterId];
		GameObject.Find ("P1Character").GetComponent<Animator> ().Play ("CharSelLeftToRight");
		var versusListItemModel = Resources.Load<GameObject> ("Prefabs/VersusListItem");
		for (int i = 0; i < nbOpponents; ++i) {
			var versusListItemInstance = Instantiate (versusListItemModel, new Vector3(0.0f, 0.3f * nbOpponents - i * 0.5f, 0.0f), versusListItemModel.transform.rotation);
			if (i == tournamentOpponent - 1) {
				versusListItemInstance.transform.GetChild (1).GetComponent<SpriteRenderer> ().sprite = FLags [playerCharacterId - 1];
				GameObject.Find ("P2CharacterSprite").GetComponent<SpriteRenderer> ().sprite = Characters [int.Parse(opponents.Substring(i, 1))];
				GameObject.Find ("P2Character").GetComponent<Animator> ().Play ("CharSelRightToLeft");
			} else {
				versusListItemInstance.transform.GetChild (0).gameObject.SetActive (false);
				versusListItemInstance.transform.GetChild (1).gameObject.SetActive (false);
				if (tournamentOpponent - 1 > i)
					versusListItemInstance.transform.GetChild (2).GetComponent<SpriteRenderer> ().color = new Color (0.6f, 0.6f, 0.6f, 1.0f);
			}
			versusListItemInstance.transform.GetChild (2).GetComponent<SpriteRenderer>().sprite = FLags[int.Parse(opponents.Substring(i, 1)) - 1];
			versusListItemInstance.transform.SetParent (GameObject.Find ("Canvas").transform);
		}
	}

	private void ContinueTournament ()
	{
		var opponent = PlayerPrefs.GetInt ("TournamentOpponent");
		var opponents = PlayerPrefs.GetString ("TournamentOpponents");
		var maps = PlayerPrefs.GetString ("TournamentMaps");
		var difficulties = PlayerPrefs.GetString ("TournamentDifficulties");

		PlayerPrefs.SetInt ("P2Character", int.Parse(opponents.Substring(opponent - 1, 1)));
		PlayerPrefs.SetInt ("SelectedMap", int.Parse(maps.Substring(opponent - 1, 1)));
		PlayerPrefs.SetInt ("Difficulty", int.Parse(difficulties.Substring(opponent - 1, 1)));
		SceneManager.LoadScene("GameLoadingScene");
	}
}
