using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBackgroundBehavior : MonoBehaviour
{
	public SpriteRenderer SpriteRenderer;
	public Sprite SpriteWin;
	public Sprite SpriteLoose;

	public void Win()
	{
		SpriteRenderer.sprite = SpriteWin;
	}

	public void Loose()
	{
		SpriteRenderer.sprite = SpriteLoose;
	}
}
