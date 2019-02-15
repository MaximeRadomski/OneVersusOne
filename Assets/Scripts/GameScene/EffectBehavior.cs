using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBehavior : MonoBehaviour
{
	public SpriteRenderer SpriteRenderer;
    public GameObject ObjectToFollow;
	public float DelayBeforeHide;
	public float DelayBeforeDestroy;
	public EffectType Effect;

	private GameManagerBehavior _gameManagerBehavior;

	void Start ()
	{
		_gameManagerBehavior = GameObject.Find ("$GameManager").GetComponent<GameManagerBehavior>();

		if (Effect != EffectType.None)
			PlayEffectSound ();
		Invoke ("HideAfterDelay", DelayBeforeHide);
		Invoke ("DestroyAfterDelay", DelayBeforeDestroy);
	}

    void Update()
    {
        if (ObjectToFollow != null)
        {
            var tmpDistance = 0.25f;
            if (ObjectToFollow.GetComponent<PlayerBehavior>().Direction == Direction.Left)
                tmpDistance = -tmpDistance;
            transform.position = ObjectToFollow.transform.position + new Vector3(tmpDistance, 0.0f, 0.0f);
        }
    }

	private void PlayEffectSound()
	{
		switch (Effect)
		{
		case EffectType.CastSP:
			AndroidNativeAudio.play(_gameManagerBehavior.CastSPAudioFileID);
			break;
		case EffectType.Catch:
			AndroidNativeAudio.play(_gameManagerBehavior.CatchAudioFileID);
			break;
		case EffectType.Dash:
			AndroidNativeAudio.play(_gameManagerBehavior.DashAudioFileID);
			break;
		case EffectType.Goal:
			AndroidNativeAudio.play(_gameManagerBehavior.GoalAudioFileID);
			break;
		case EffectType.Lift:
			AndroidNativeAudio.play(_gameManagerBehavior.LiftAudioFileID);
			break;
		case EffectType.QuickEffect:
			AndroidNativeAudio.play(_gameManagerBehavior.QuickEffectAudioFileID);
			break;
		case EffectType.WallHit:
			AndroidNativeAudio.play(_gameManagerBehavior.WallHitAudioFileID);
			break;
		}
	}

	private void HideAfterDelay()
	{
		if (SpriteRenderer != null)
			SpriteRenderer.enabled = false;	
	}

    private void DestroyAfterDelay()
	{
		Destroy (gameObject);
	}

	public enum EffectType
	{
		Catch,
		Dash,
		Goal,
		Lift,
		QuickEffect,
		CastSP,
		WallHit,
		None
	}
}
