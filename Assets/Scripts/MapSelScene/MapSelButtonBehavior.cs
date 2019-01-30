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

	public ButtonSoundType ButtonSound;

	private MapSelManagerBehavior _mapSelManager;
	private int _player;

	void Start()
	{
		if (Player == CurrentPlayer.PlayerOne)
			_player = 1;
		else
			_player = 2;
		_mapSelManager = GameObject.Find ("$MapSelManager").GetComponent<MapSelManagerBehavior>();
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
		PlayButtonSound ();
	}

	private void PlayButtonSound ()
	{
		switch (ButtonSound)
		{
		case ButtonSoundType.MenuBipSelect:
			AndroidNativeAudio.play (_mapSelManager.MenuBipSelectAudioFileID);
			break;
		case ButtonSoundType.MenuBipConfirm:
			AndroidNativeAudio.play (_mapSelManager.MenuBipConfirmAudioFileID);
			break;
		}
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
