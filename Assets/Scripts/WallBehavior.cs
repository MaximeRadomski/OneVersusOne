using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehavior : MonoBehaviour
{
	public Side WallSide;
	public Animator Animator;

	public void WallHit()
	{
		if (WallSide == Side.Left)
			Animator.Play ("WallLeft");
		else
			Animator.Play ("WallRight");
		Invoke ("StopAnimation", 0.5f);
	}

	public void StopAnimation()
	{
		Animator.Play ("Idle");
	}

	public enum Side
	{
		Left,
		Right
	}
}
