using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsecutiveHitBehavior : MonoBehaviour
{
	public int Number;

	private float _delay;

	void Start ()
	{
		this.gameObject.GetComponent<UnityEngine.UI.Text> ().text = Number.ToString ();
		_delay = 1.0f;
		Invoke ("DestroyAfterDelay", _delay);
	}

	private void DestroyAfterDelay()
	{
		Destroy (this.gameObject);
	}
}
