using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchlineBehavior : MonoBehaviour
{
	public string Text;

	private float _delay;

	void Start ()
	{
		this.gameObject.GetComponent<UnityEngine.UI.Text> ().text = Text;
		_delay = 1.0f;
		Invoke ("DestroyAfterDelay", _delay);
	}

	private void DestroyAfterDelay()
	{
		Destroy (this.gameObject);
	}
}
