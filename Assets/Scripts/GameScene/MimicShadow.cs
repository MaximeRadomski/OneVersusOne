using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicShadow : MonoBehaviour
{
	public GameObject LinkedGameObject;
	public bool IsDoingAction;

	private SpriteRenderer _linkedSpriteRenderer;
	private SpriteRenderer _currentSpriteRenderer;
	private Direction _currentDirection;
	private Direction _oldDirection;
	private Color _transparent;
	private Color _fullColor;

	void Start ()
	{
		_linkedSpriteRenderer = LinkedGameObject.GetComponent<SpriteRenderer> ();
		_currentSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer> ();
		_currentDirection = Direction.Standby;
		_transparent = new Color (0.0f, 0.0f, 0.0f, 0.0f);
		_fullColor = new Color (0.0f, 0.0f, 0.0f, 0.35f);
	}

	void Update ()
	{
		_currentSpriteRenderer.sprite = _linkedSpriteRenderer.sprite;
		transform.position = LinkedGameObject.transform.position + new Vector3 (0.056f, -0.056f, 0.0f);
		_oldDirection = _currentDirection;
		_currentDirection = LinkedGameObject.GetComponent<PlayerBehavior>().Direction;
		transform.rotation = LinkedGameObject.transform.rotation;
		if (_oldDirection != _currentDirection || IsDoingAction) {
			_currentSpriteRenderer.color = _transparent;
			IsDoingAction = false;
		}
		else
			_currentSpriteRenderer.color = _fullColor;
	}
}
