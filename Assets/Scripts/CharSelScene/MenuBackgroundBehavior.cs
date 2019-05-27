using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackgroundBehavior : MonoBehaviour
{
	public float ScrollSpeed;
	public MeshRenderer Renderer;

	private float _originalHeight;

	private void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	void Start()
	{
		_originalHeight = transform.lossyScale.y;
		AdjustToCamera ();

		var menuBackgroundList = GameObject.FindGameObjectsWithTag ("MenuBackground");
		if (menuBackgroundList.Length > 1)
			Destroy(gameObject);
		Renderer.sortingLayerName = "Background";
		if (PlayerPrefs.GetInt ("Music", 1) == 0)
			this.gameObject.GetComponent<AudioSource> ().volume = 0.0f;
	}

	public void AdjustToCamera()
	{
		float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
		transform.localScale = new Vector3(transform.localScale.x, worldScreenHeight, 1);
		Renderer.material.mainTextureScale = new Vector2 (1.0f, transform.lossyScale.y / _originalHeight);
	}

	void Update ()
	{
		float offset = Time.time * ScrollSpeed;
		Renderer.material.mainTextureOffset = new Vector2(offset, offset/2);
	}

	public void PlayAudio()
	{
		this.gameObject.GetComponent<AudioSource> ().Play ();
	}
}
