using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManagerBehavior : MonoBehaviour
{
	private GameObject _duelButton, _challengesButton, _howToPlayButton, _dashesText, _optionsButton, _aboutButton;

	void Start ()
	{
		_duelButton = GameObject.Find ("DuelButton");
		_challengesButton = GameObject.Find ("ChallengesButton");
		_howToPlayButton = GameObject.Find ("HowToPlayButton");
		_dashesText = GameObject.Find ("DashesText");
		_optionsButton = GameObject.Find ("OptionsButton");
		_aboutButton = GameObject.Find ("AboutButton");

		StartCoroutine (InitiateLeft());
	}

	private IEnumerator InitiateLeft()
	{
		_duelButton.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
		yield return new WaitForSeconds(0.05f);
		_challengesButton.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
		yield return new WaitForSeconds(0.05f);
		_howToPlayButton.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
		yield return new WaitForSeconds(0.05f);
		_dashesText.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
		yield return new WaitForSeconds(0.05f);
		_optionsButton.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
		yield return new WaitForSeconds(0.05f);
		_aboutButton.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
	}
}
