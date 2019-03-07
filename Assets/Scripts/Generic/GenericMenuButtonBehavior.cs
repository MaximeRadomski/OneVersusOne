using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenericMenuButtonBehavior : MonoBehaviour
{
	public GenericTapAction Action;
	public SpriteRenderer SpriteRenderer;
	public Sprite SpriteOn;
	public Sprite SpriteOff;
	public string NextSceneName;
	public bool KeepState;

	public ButtonSoundType ButtonSound;

	private GenericMenuManagerBehavior _genericMenuManager;

	void Start()
	{
		_genericMenuManager = GameObject.Find ("$GenericMenuManager").GetComponent<GenericMenuManagerBehavior>();
	}

	public void SwitchSprite()
	{
		if (SpriteRenderer.sprite == SpriteOn)
			SpriteRenderer.sprite = SpriteOff;
		else
			SpriteRenderer.sprite = SpriteOn;
	}

	public void SetSpriteOn()
	{
		SpriteRenderer.sprite = SpriteOn;
	}

	public void SetSpriteOff()
	{
		SpriteRenderer.sprite = SpriteOff;
	}

	public void DoAction()
	{
		switch (Action)
		{
		case GenericTapAction.GoTo:
			SceneManager.LoadScene(NextSceneName);
			break;
		}
		PlayButtonSound ();
	}

	private void PlayButtonSound ()
	{
		switch (ButtonSound)
		{
		case ButtonSoundType.MenuBipGoTo:
			AndroidNativeAudio.play (_genericMenuManager.MenuBipGoToAudioFileID);
			break;
		}
	}


	public enum GenericTapAction
	{
		GoTo
	}

	public enum ButtonSoundType
	{
		MenuBipGoTo
	}
}
