using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicShadow : MonoBehaviour
{
	public GameObject LinkedGameObject;

	private SpriteRenderer _linkedSpriteRenderer;
	private SpriteRenderer _currentSpriteRenderer;

	void Start ()
	{
		_linkedSpriteRenderer = LinkedGameObject.GetComponent<SpriteRenderer> ();
		_currentSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer> ();
	}

	void Update ()
	{
		transform.position = LinkedGameObject.transform.position + new Vector3 (0.028f, -0.028f, 0.0f);
		transform.rotation = LinkedGameObject.transform.rotation;
		_currentSpriteRenderer.sprite = _linkedSpriteRenderer.sprite;
	}
}
