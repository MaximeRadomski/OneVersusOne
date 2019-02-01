using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingButtonBehavior : MonoBehaviour
{
	public SpriteRenderer SpriteRenderer;
	public Sprite HasTheDisc, DoesntHaveTheDisc;
	public CurrentPlayer Player;

	private PlayerBehavior _playerbehavior;
	private bool _hasTheDisc;

	void Start()
	{
		_hasTheDisc = true;
	}

	private void GetPlayer()
	{
		_playerbehavior = GameObject.Find (Player.ToString()).GetComponent<PlayerBehavior>();
	}

	void Update ()
	{
		if (_playerbehavior == null)
			GetPlayer ();
		if (_playerbehavior == null)
			return;
		if (_playerbehavior.HasTheDisc && !_hasTheDisc)
		{
			_hasTheDisc = true;
			SpriteRenderer.sprite = HasTheDisc;
		}
		else if (!_playerbehavior.HasTheDisc && _hasTheDisc)
		{
			_hasTheDisc = false;
			SpriteRenderer.sprite = DoesntHaveTheDisc;
		}
	}
}
