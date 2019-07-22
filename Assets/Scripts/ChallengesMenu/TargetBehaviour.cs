using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehaviour : MonoBehaviour
{
	private GameObject _healthBar;
	private GameObject _healthBarBackground;
	private bool _touched;

	void Start()
	{
		_touched = false;
		_healthBar = this.gameObject.transform.GetChild (0).gameObject;
		_healthBarBackground = this.gameObject.transform.GetChild (1).gameObject;
		_healthBar.GetComponent<Animator> ().Play ("TargetHealthBar");
		Invoke ("HealthBarAtZero", 3.5f);
	}

	private void HealthBarAtZero()
	{
		if (!_touched)
			DestroyAfterCollisionOrDelay (true);
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Disc")
		{
			_touched = true;
			DestroyAfterCollisionOrDelay ();
		}
	}

	private void DestroyAfterCollisionOrDelay(bool lose = false)
	{
		_healthBar.SetActive (false);
		_healthBarBackground.SetActive (false);
		this.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
		if (!lose)
			GameObject.Find ("$GameManager").GetComponent<GameManagerBehavior> ().NewTarget ();
		else
			GameObject.Find ("$GameManager").GetComponent<GameManagerBehavior> ().NewBallChallenge (true);
		this.gameObject.GetComponent<Animator> ().Play ("TargetFade");
		Invoke ("DestroyAfterDelay", 1.5f);
	}

	private void DestroyAfterDelay()
	{
		Destroy (this.gameObject);
	}
}
