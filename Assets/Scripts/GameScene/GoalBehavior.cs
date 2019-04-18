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

	void Start()
	{
		IsFrozen = false;
	}

	public void GoalHit()
	{
		if (Player == CurrentPlayer.PlayerOne)
			Animator.Play ("GoalBot");
		else
			Animator.Play ("GoalTop");
		Invoke ("StopAnimation", 0.5f);
	}

	public void StopAnimation()
	{
		Animator.Play ("Idle");
	}

	public void Freeze()
	{
		IsFrozen = true;
		gameObject.tag = "FrozenWall";
		this.GetComponent<SpriteRenderer> ().sprite = FrozenStateSprite;
	}

	public void Unfreeze()
	{
		IsFrozen = false;
		gameObject.tag = "Goal";
		this.GetComponent<SpriteRenderer> ().sprite = NormalStateSprite;
	}

	public void Actualize()
	{
		if (IsFrozen)
			this.GetComponent<SpriteRenderer> ().sprite = FrozenStateSprite;
		else
			this.GetComponent<SpriteRenderer> ().sprite = NormalStateSprite;
	}
}
