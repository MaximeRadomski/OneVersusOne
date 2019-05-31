using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandHeightToScreen : MonoBehaviour
{
	
	void Start ()
	{
		Invoke ("AdjustToCamera", 0.0f);
	}

	public void AdjustToCamera()
	{
		//float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
		//GetComponent<SpriteRenderer>().transform.localScale = new Vector3(transform.localScale.x, worldScreenHeight, 1);
	}
}
