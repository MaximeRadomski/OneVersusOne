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
			StretchOnCollision();
			Invoke ("ResetSprite", 0.25f);
		}
	}

	private void StretchOnCollision()
	{
		transform.localScale = new Vector3 (1.1f, 0.9f, 1.0f);
		Invoke ("ResetStretch", 0.1f);
	}

	private void ResetStretch()
	{
		transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
	}

	private void ResetSprite()
	{
		--_touchedCount;
		if (_touchedCount == 0)
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = SpriteIdle;
	}
}
