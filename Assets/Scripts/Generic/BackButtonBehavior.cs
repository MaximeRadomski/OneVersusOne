using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonBehavior : MonoBehaviour
{
	void Start ()
	{
		if (PlayerPrefs.GetInt ("ShowBack", 1) == 1) {
			Enable ();
		} else {
			Disable ();
		}
	}

	public void Enable()
	{
		this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		this.gameObject.GetComponent<BoxCollider2D> ().enabled = true;
	}

	public void Disable()
	{
		this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
		this.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
	}
}
