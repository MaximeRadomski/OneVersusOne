using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSelButtonBehavior : MonoBehaviour
{
	public CurrentPlayer Player;
	public GenericTapAction Action;
	public SpriteRenderer SpriteRenderer;
	public Sprite SpriteOn;
	public Sprite SpriteOff;

	public ButtonSoundType ButtonSound;

	private CharSelManagerBehavior _charSelManager;
	private GenericMenuManagerBehavior _genericMenuManager;
	private int _player;

	void Start()
	{
		if (Player == CurrentPlayer.PlayerOne)
			_player = 1;
		else
			_player = 2;
		_charSelManager = GameObject.Find ("$CharSelManager").GetComponent<CharSelManagerBehavior>();
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
		case GenericTapAction.C1:
			_charSelManager.ChangeSelectedCharacter(_player, 1);
			break;
		case GenericTapAction.C2:
			_charSelManager.ChangeSelectedCharacter(_player, 2);
			break;
		case GenericTapAction.C3:
			_charSelManager.ChangeSelectedCharacter(_player, 3);
			break;
		case GenericTapAction.C4:
			_charSelManager.ChangeSelectedCharacter(_player, 4);
			break;
		case GenericTapAction.C5:
			_charSelManager.ChangeSelectedCharacter(_player, 5);
			break;
		case GenericTapAction.C6:
			_charSelManager.ChangeSelectedCharacter(_player, 6);
			break;
		case GenericTapAction.Confirm:
			_charSelManager.Confirm(_player);
			break;
		}
		PlayButtonSound ();
	}

	private void PlayButtonSound ()
	{
		switch (ButtonSound)
		{
		case ButtonSoundType.MenuBipSelect:
			AndroidNativeAudio.play (_genericMenuManager.MenuBipSelectAudioFileID);
			break;
		case ButtonSoundType.MenuBipConfirm:
			AndroidNativeAudio.play (_genericMenuManager.MenuBipConfirmAudioFileID);
			break;
		}
	}


	public enum GenericTapAction
	{
		C1,
		C2,
		C3,
		C4,
		C5,
		C6,
		Confirm
	}

	public enum ButtonSoundType
	{
		MenuBipSelect,
		MenuBipConfirm
	}
}
