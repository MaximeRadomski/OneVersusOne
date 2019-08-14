using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBehavior : MonoBehaviour
{
	public Sprite SpriteIdle;
	public Sprite SpriteTouched;
	public Sprite SpriteDeath;
	public int HP;

	private int _touchedCount;
	private bool _isDead;

	void Start()
	{
		this.gameObject.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Text> ().text = HP.ToString ();
		_touchedCount = 0;
		_isDead = false;
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Disc")
		{
			++_touchedCount;
			if (col.gameObject.GetComponent<BallBehavior> ().QuickDisk)
				HP -= 2;
			else
				--HP;
			this.gameObject.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Text> ().text = HP >= 0 ? HP.ToString () : "0";
			StretchOnCollision ();
			if (HP > 0) {
				this.gameObject.GetComponent<SpriteRenderer> ().sprite = SpriteTouched;
				Invoke ("ResetSprite", 0.25f);
			} else {
				DestroyAfterCollision ();
			}
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
		if (_touchedCount == 0 && !_isDead)
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = SpriteIdle;
	}

	private void DestroyAfterCollision()
	{
		_isDead = true;
		this.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
		this.gameObject.GetComponent<SpriteRenderer> ().sprite = SpriteDeath;
		this.gameObject.GetComponent<Animator> ().Play ("BrickFade");
		Invoke ("DestroyAfterDelay", 1.5f);
	}

	private void DestroyAfterDelay()
	{
		if (GameObject.Find ("Breakout").transform.childCount == 1) {
			GameObject.Find ("$GameManager").GetComponent<GameManagerBehavior> ().NewBreakout ();
			var ball = GameObject.Find ("Ball");
			if (ball.GetComponent<BallBehavior> ().CurrentPlayer != CurrentPlayer.None)
				ball.GetComponent<BallBehavior> ().ResetSpeed ();
			else
				Destroy (ball);
		}
		Destroy (this.gameObject);
	}
}
