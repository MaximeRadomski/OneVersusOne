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
		if (gameObject.tag == "Disc")
		{
			this.gameObject.GetComponent<Animator> ().Play ("TargetFade");
			Invoke ("DestroyAfterDelay", 1.5f);
		}
	}

	private void DestroyAfterDelay()
	{
		Destroy (this.gameObject);
	}
}
