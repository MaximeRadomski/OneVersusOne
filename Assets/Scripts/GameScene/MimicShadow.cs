using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicShadow : MonoBehaviour
{
	public GameObject LinkedGameObject;
	public bool IsDoingAction;

	private SpriteRenderer _linkedSpriteRenderer;
	private SpriteRenderer _currentSpriteRenderer;
	private PlayerBehavior _currentPlayerBehavior;
	private Direction _currentDirection;
	private Direction _oldDirection;
	private Color _transparent;
	private Color _fullColor;
    private bool _isDisplayingOverridenSprite;

	void Start ()
	{
		_linkedSpriteRenderer = LinkedGameObject.GetComponent<SpriteRenderer> ();
		_currentSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer> ();
		_currentPlayerBehavior = LinkedGameObject.GetComponent<PlayerBehavior> ();
		_currentDirection = Direction.Standby;
		_transparent = new Color (0.0f, 0.0f, 0.0f, 0.0f);
		_fullColor = new Color (0.0f, 0.0f, 0.0f, 0.35f);
        _isDisplayingOverridenSprite = false;
	}

	void Update ()
	{
        if (_isDisplayingOverridenSprite)
            _isDisplayingOverridenSprite = false;
        else
		    _currentSpriteRenderer.sprite = _linkedSpriteRenderer.sprite;
		_currentSpriteRenderer.enabled = _linkedSpriteRenderer.enabled;
		_currentSpriteRenderer.flipX = _linkedSpriteRenderer.flipX;
		_currentSpriteRenderer.flipY = _linkedSpriteRenderer.flipY;
		transform.localScale = LinkedGameObject.transform.localScale;
		transform.position = LinkedGameObject.transform.position + new Vector3 (0.056f, -0.056f, 0.0f);
		_oldDirection = _currentDirection;
		if (_currentPlayerBehavior != null)
			_currentDirection = _currentPlayerBehavior.Direction;
		transform.rotation = LinkedGameObject.transform.rotation;
		if (_oldDirection != _currentDirection || IsDoingAction) {
			_currentSpriteRenderer.color = _transparent;
			IsDoingAction = false;
		}
		else
			_currentSpriteRenderer.color = _fullColor;
	}

    public void ForceSprite(Sprite overrideSprite)
    {
        _isDisplayingOverridenSprite = true;
        _currentSpriteRenderer.sprite = overrideSprite;
    }
}
