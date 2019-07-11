using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengesMenuBehavior : MonoBehaviour
{
	private GameObject _tournamentTitle, _targetsTitle, _catchTitle;
	private GameObject _tournamentButtons, _targetsButtons, _catchButtons;

	void Start ()
	{
		_tournamentTitle = GameObject.Find ("Tournament");
		_tournamentButtons = GameObject.Find ("TournamentButtons");
		_targetsTitle = GameObject.Find ("Targets");
		_targetsButtons = GameObject.Find ("TargetsButtons");
		_catchTitle = GameObject.Find ("Catch");
		_catchButtons = GameObject.Find ("CatchButtons");

		StartCoroutine (InitiateLeft());
	}

	private IEnumerator InitiateLeft()
	{
		_tournamentTitle.GetComponent<Animator> ().Play ("CharSelLeftToRight");
		yield return new WaitForSeconds(0.05f);
		_tournamentButtons.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
		yield return new WaitForSeconds(0.05f);
		_targetsTitle.GetComponent<Animator> ().Play ("CharSelLeftToRight");
		yield return new WaitForSeconds(0.05f);
		_targetsButtons.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
		yield return new WaitForSeconds(0.05f);
		_catchTitle.GetComponent<Animator> ().Play ("CharSelLeftToRight");
		yield return new WaitForSeconds(0.05f);
		_catchButtons.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
		yield return new WaitForSeconds(0.05f);
	}
}
