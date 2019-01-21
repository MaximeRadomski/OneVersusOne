using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBehavior : MonoBehaviour
{
	public SpriteRenderer SpriteRenderer;
    public GameObject ObjectToFollow;
	public float DelayBeforeHide;
	public float DelayBeforeDestroy;

	void Start ()
	{
		Invoke ("HideAfterDelay", DelayBeforeHide);
		Invoke ("DestroyAfterDelay", DelayBeforeDestroy);
	}

    void Update()
    {
        if (ObjectToFollow != null)
        {
            var tmpDistance = 0.25f;
            if (ObjectToFollow.GetComponent<PlayerBehavior>().Direction == Direction.Left)
                tmpDistance = -tmpDistance;
            transform.position = ObjectToFollow.transform.position + new Vector3(tmpDistance, 0.0f, 0.0f);
        }
    }

	private void HideAfterDelay()
	{
		if (SpriteRenderer != null)
			SpriteRenderer.enabled = false;	
	}

    private void DestroyAfterDelay()
	{
		Destroy (gameObject);
	}
}
