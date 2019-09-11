using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutMenuManagerBehavior : MonoBehaviour
{
	private GameObject _who, _what, _why;
	private GameObject _whoContent, _whatContent, _whyContent;

	void Start () {
		_who = GameObject.Find ("Who");
		_what = GameObject.Find ("What");
		_why = GameObject.Find ("Why");
		_whoContent = GameObject.Find ("WhoContent");
		_whatContent = GameObject.Find ("WhatContent");
		_whyContent = GameObject.Find ("WhyContent");

		StartCoroutine (InitiateLeft());
	}

	private IEnumerator InitiateLeft()
	{
		_who.GetComponent<Animator> ().Play ("CharSelLeftToRight");
		yield return new WaitForSeconds(0.05f);
		_whoContent.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
		yield return new WaitForSeconds(0.05f);
		_what.GetComponent<Animator> ().Play ("CharSelLeftToRight");
		yield return new WaitForSeconds(0.05f);
		_whatContent.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
		yield return new WaitForSeconds(0.05f);
		_why.GetComponent<Animator> ().Play ("CharSelLeftToRight");
		yield return new WaitForSeconds(0.05f);
		_whyContent.GetComponent<Animator> ().Play ("LeftOut-RightMiddle");
	}

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //We transform the touch position into word space from screen space and store it.
            var touchPosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (touchPosWorld.y > 2.3f)
                Application.OpenURL("https://twitter.com/Abject_Sama");
        }
    }
}
