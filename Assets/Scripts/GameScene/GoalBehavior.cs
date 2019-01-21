using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBehavior : MonoBehaviour
{
    public CurrentPlayer Player;
	public Animator Animator;
	public int Points;

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
}
