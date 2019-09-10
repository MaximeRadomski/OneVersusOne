using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingButtonBehavior : MonoBehaviour
{
	public SpriteRenderer SpriteRenderer;
	public Sprite HasTheDisc, DoesntHaveTheDisc, EmptyButton;
	public CurrentPlayer Player;

	public bool IsThrowLift;
    public bool IsLeftRight;

	private PlayerBehavior _playerbehavior;
	private GameManagerBehavior _gameManagerBehavior;
	private bool _hasTheDisc;

	void Start()
	{
		_hasTheDisc = true;
        if (PlayerPrefs.GetInt("GameMode") == GameMode.Breakout.GetHashCode() && SpriteRenderer != null && IsThrowLift && EmptyButton != null)
        {
            transform.position = new Vector3(3.0f, 0.0f, 0.0f);
            GameObject.Find("P1SPCoolDown").transform.position = new Vector3(3.0f, 0.0f, 0.0f);
        }
    }

	private void GetPlayer()
	{
		_playerbehavior = GameObject.Find (Player.ToString()).GetComponent<PlayerBehavior>();
		_gameManagerBehavior = GameObject.Find ("$GameManager").GetComponent<GameManagerBehavior>();
	}

	void Update ()
	{
		if (IsThrowLift)
			ThrowLiftButton ();
        else if (IsLeftRight)
            LeftRightButton();
        else
			SuperButton ();
	}

	private void ThrowLiftButton()
	{
		if (_playerbehavior == null)
			GetPlayer ();
		if (_playerbehavior == null)
			return;
		
		if (_playerbehavior.IsDoingSP || _gameManagerBehavior.IsPaused)
			DisableCurrentButton ();
		else
			EnableCurrentButton ();

		if (_playerbehavior.HasTheDisc && !_hasTheDisc)
		{
			_hasTheDisc = true;
			SpriteRenderer.sprite = HasTheDisc;
		}
		else if (!_playerbehavior.HasTheDisc && _hasTheDisc)
		{
			_hasTheDisc = false;
			if (EmptyButton == null || _playerbehavior.SPCooldown == 0)
				SpriteRenderer.sprite = DoesntHaveTheDisc;
			else
				SpriteRenderer.sprite = EmptyButton;
		}
	}

    private void LeftRightButton()
    {
        if (_playerbehavior == null)
            GetPlayer();
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

    private void SuperButton()
	{
		if (_playerbehavior == null)
			GetPlayer ();
		if (_playerbehavior == null)
			return;
		if (_playerbehavior.IsDoingSP)
			EnableCurrentButton ();
		else
			DisableCurrentButton ();
	}

	public void EnableCurrentButton()
	{
		this.GetComponent<SpriteRenderer> ().enabled = true;
		this.GetComponent<BoxCollider2D> ().enabled = true;
	}

	public void DisableCurrentButton()
	{
		this.GetComponent<BoxCollider2D> ().enabled = false;
		if (_gameManagerBehavior.IsPaused)
			return;
		this.GetComponent<SpriteRenderer> ().enabled = false;

	}
}
