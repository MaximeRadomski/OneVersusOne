using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetWallBehaviour : MonoBehaviour
{
	public Sprite SpriteIdle;
	public Sprite SpriteTouched;

	private int _touchedCount;

	void Start()
	{
		_touchedCount = 0;
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Disc")
		{
			++_touchedCount;
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = SpriteTouched;
			Invoke ("ResetSprite", 0.25f);
		}
	}

	private void ResetSprite()
	{
		--_touchedCount;
		if (_touchedCount == 0)
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = SpriteIdle;
	}
}
