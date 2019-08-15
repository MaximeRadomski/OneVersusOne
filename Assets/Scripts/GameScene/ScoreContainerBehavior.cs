using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreContainerBehavior : MonoBehaviour {

	public void Stretch()
	{
		transform.localScale = new Vector3 (0.95f, 1.1f, 1.0f);
		Invoke ("ResetStretch", 0.1f);
	}

	private void ResetStretch()
	{
		transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
	}
}
