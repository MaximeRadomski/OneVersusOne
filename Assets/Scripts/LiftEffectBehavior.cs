using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftEffectBehavior : MonoBehaviour
{
	public SpriteRenderer SpriteRenderer;
	public float DelayBeforeDestroy;

	void Start ()
	{
		Invoke ("DestroyAfterDelay", DelayBeforeDestroy);
	}

	private void DestroyAfterDelay()
	{
		SpriteRenderer.enabled = false;
		Destroy (gameObject);
	}
}
