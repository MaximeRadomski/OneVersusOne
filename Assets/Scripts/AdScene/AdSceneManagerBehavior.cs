using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdSceneManagerBehavior : MonoBehaviour
{
	private GameObject _leaveButton;

	void Start ()
	{
		_leaveButton = GameObject.Find ("LeaveButton");

		StartCoroutine (InitiateLeft());
	}

	private IEnumerator InitiateLeft()
	{
		yield return new WaitForSeconds(1.0f);
		_leaveButton.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
	}
}
