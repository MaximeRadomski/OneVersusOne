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
			CustomAudio.PlayEffect(_gameManagerBehavior.CastSPAudioFileID);
			break;
		case EffectType.Catch:
			CustomAudio.PlayEffect(_gameManagerBehavior.CatchAudioFileID);
			break;
		case EffectType.Dash:
			CustomAudio.PlayEffect(_gameManagerBehavior.DashAudioFileID);
			break;
		case EffectType.Goal:
			CustomAudio.PlayEffect(_gameManagerBehavior.GoalAudioFileID);
			break;
		case EffectType.Lift:
			CustomAudio.PlayEffect(_gameManagerBehavior.LiftAudioFileID);
			break;
		case EffectType.QuickEffect:
		case EffectType.Super01:
			CustomAudio.PlayEffect(_gameManagerBehavior.QuickEffectAudioFileID);
			break;
		case EffectType.WallHit:
			CustomAudio.PlayEffect(_gameManagerBehavior.WallHitAudioFileID);
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
		None,
		Super01
	}
}
