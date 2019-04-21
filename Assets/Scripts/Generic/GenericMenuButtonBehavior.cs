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

	public delegate void ButtonDelegate();
	public ButtonDelegate buttonDelegate;

	private GenericMenuManagerBehavior _genericMenuManager;

	void Start()
	{
		if (ButtonSound != ButtonSoundType.NoSound)
			_genericMenuManager = GameObject.Find ("$GenericMenuManager").GetComponent<GenericMenuManagerBehavior>();
	}

	public void SwitchSprite()
	{
		if (SpriteRenderer == null)
			return;
		SpriteRenderer.color = new Color (1.0f, 1.0f, 1.0f);
		if (SpriteRenderer.sprite == SpriteOn)
			SpriteRenderer.sprite = SpriteOff;
		else
			SpriteRenderer.sprite = SpriteOn;
	}

	public void PressSprite()
	{
		if (SpriteRenderer == null)
			return;
		SpriteRenderer.color = new Color (0.8f, 0.8f, 0.8f);
	}

	public void SetSpriteOn()
	{
		if (SpriteRenderer == null)
			return;
		SpriteRenderer.color = new Color (1.0f, 1.0f, 1.0f);
		SpriteRenderer.sprite = SpriteOn;
	}

	public void SetSpriteOff()
	{
		if (SpriteRenderer == null)
			return;
		SpriteRenderer.color = new Color (1.0f, 1.0f, 1.0f);
		SpriteRenderer.sprite = SpriteOff;
	}

	public void DoAction()
	{
		if (SpriteRenderer != null)
			SpriteRenderer.color = new Color (1.0f, 1.0f, 1.0f);
		switch (Action)
		{
		case GenericTapAction.GoTo:
			SceneManager.LoadScene(NextSceneName);
			break;
		case GenericTapAction.DoDelegate:
			buttonDelegate ();
			break;
		}
		PlayButtonSound ();
	}

	private void PlayButtonSound ()
	{
		switch (ButtonSound)
		{
		case ButtonSoundType.MenuBipGoTo:
			CustomAudio.PlayEffect(_genericMenuManager.MenuBipGoToAudioFileID);
			break;
		default:
			break;
		}
	}


	public enum GenericTapAction
	{
		GoTo,
		DoDelegate
	}

	public enum ButtonSoundType
	{
		MenuBipGoTo,
		NoSound
	}
}
