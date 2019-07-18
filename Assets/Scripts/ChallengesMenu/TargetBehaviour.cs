using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehaviour : MonoBehaviour
{
	void Start ()
	{
		
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Disc")
		{
			this.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
			GameObject.Find ("$GameManager").GetComponent<GameManagerBehavior> ().NewTarget ();
			this.gameObject.GetComponent<Animator> ().Play ("TargetFade");
			Invoke ("DestroyAfterDelay", 1.5f);
		}
	}

	private void DestroyAfterDelay()
	{
		Destroy (this.gameObject);
	}
}
