﻿using System.Collections;
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
        GameObject tmpGenericMenuManager = null;
        if (ButtonSound != ButtonSoundType.NoSound && (tmpGenericMenuManager = GameObject.Find("$GenericMenuManager")) != null)
			_genericMenuManager = tmpGenericMenuManager.GetComponent<GenericMenuManagerBehavior>();
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
		StretchOnPress ();
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
            if (_genericMenuManager != null)
			    CustomAudio.PlayEffect(_genericMenuManager.MenuBipGoToAudioFileID);
			break;
		default:
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
		GoTo,
		DoDelegate
	}

	public enum ButtonSoundType
	{
		MenuBipGoTo,
		NoSound
	}
}
