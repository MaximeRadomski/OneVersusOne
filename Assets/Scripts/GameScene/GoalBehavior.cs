using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBehavior : MonoBehaviour
{
    public CurrentPlayer Player;
	public Animator Animator;
	public int Points;

	public bool IsFrozen;

	public Sprite NormalStateSprite;
	public Sprite FrozenStateSprite;

	private SpriteRenderer _spriteRenderer;
	private Shader _shaderGUItext;
	private Shader _shaderSpritesDefault;
    private bool _isHowToPlay;

	void Start()
	{
		IsFrozen = false;
		_spriteRenderer = this.GetComponent<SpriteRenderer> ();
		_shaderGUItext = Shader.Find("GUI/Text Shader");
		_shaderSpritesDefault = Shader.Find("Sprites/Default");
        _isHowToPlay = GameObject.Find("$GameManager").GetComponent<GameManagerBehavior>().IsHowToPlay;
	}

	public void GoalHit()
	{
		if (Player == CurrentPlayer.PlayerOne)
			Animator.Play ("GoalBot");
		else
			Animator.Play ("GoalTop");
		StretchOnGoal ();
        if (!_isHowToPlay)
		    TiltOnGoal ();
		Invoke ("StopAnimation", 0.5f);
	}

	private void StretchOnGoal()
	{
		transform.localScale = new Vector3 (0.8f, 1.2f, 1.0f);
		Invoke ("ResetStretch", 0.3f);
	}

	private void ResetStretch()
	{
		transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
	}

	private void TiltOnGoal()
	{
		_spriteRenderer.material.shader = _shaderGUItext;
		_spriteRenderer.color = new Color (1.0f,1.0f,1.0f,0.5f);
		Invoke ("ResetTilt", 0.1f);
	}

	private void ResetTilt()
	{
		_spriteRenderer.material.shader = _shaderSpritesDefault;
		_spriteRenderer.color = Color.white;
	}

	public void StopAnimation()
	{
		Animator.Play ("Idle");
	}

	public void Freeze()
	{
		IsFrozen = true;
		gameObject.tag = "FrozenWall";
		_spriteRenderer.sprite = FrozenStateSprite;
	}

	public void Unfreeze()
	{
		IsFrozen = false;
		gameObject.tag = "Goal";
		_spriteRenderer.sprite = NormalStateSprite;
	}

	public void Actualize()
	{
		if (IsFrozen)
			_spriteRenderer.sprite = FrozenStateSprite;
		else
			_spriteRenderer.sprite = NormalStateSprite;
	}
}
