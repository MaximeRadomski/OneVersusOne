using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelButtonBehavior : MonoBehaviour
{
	public CurrentPlayer Player;
	public GenericTapAction Action;
	public SpriteRenderer SpriteRenderer;
	public Sprite SpriteOn;
	public Sprite SpriteOff;
	public bool KeepState;

	public ButtonSoundType ButtonSound;

	private MapSelManagerBehavior _mapSelManager;
	private GenericMenuManagerBehavior _genericMenuManager;
	private int _player;

	void Start()
	{
		if (Player == CurrentPlayer.PlayerOne)
			_player = 1;
		else
			_player = 2;
		_mapSelManager = GameObject.Find ("$MapSelManager").GetComponent<MapSelManagerBehavior>();
		_genericMenuManager = GameObject.Find ("$GenericMenuManager").GetComponent<GenericMenuManagerBehavior>();
	}

	public void SwitchSprite()
	{
		if (SpriteRenderer.sprite == SpriteOn)
			SpriteRenderer.sprite = SpriteOff;
		else
			SpriteRenderer.sprite = SpriteOn;
	}

	public void DoAction()
	{
		switch (Action)
		{
		case GenericTapAction.Left:
			_mapSelManager.ChangeSelectedMap(Direction.Left);
			break;
		case GenericTapAction.Right:
			_mapSelManager.ChangeSelectedMap(Direction.Right);
			break;
		case GenericTapAction.Confirm:
			_mapSelManager.Confirm(_player);
			break;
		}
		StretchOnPress ();
		PlayButtonSound ();
	}

	private void PlayButtonSound ()
	{
		switch (ButtonSound)
		{
		case ButtonSoundType.MenuBipSelect:
			CustomAudio.PlayEffect (_genericMenuManager.MenuBipSelectAudioFileID);
			break;
		case ButtonSoundType.MenuBipConfirm:
			CustomAudio.PlayEffect (_genericMenuManager.MenuBipConfirmAudioFileID);
			break;
		}
	}

	private void StretchOnPress()
	{
		transform.localScale = new Vector3 (1.05f, 0.9f, 1.0f);
		Invoke ("ResetStretch", 0.1f);
	}

	private void ResetStretch()
	{
		transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
	}

	public enum GenericTapAction
	{
		Left,
		Right,
		Confirm
	}

	public enum ButtonSoundType
	{
		MenuBipSelect,
		MenuBipConfirm
	}
}
