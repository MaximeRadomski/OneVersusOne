using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackgroundBehavior : MonoBehaviour
{
	public float ScrollSpeed;
	public MeshRenderer Renderer;
	public bool IsAdSceneBackground;

	private float _originalHeight;
    private float _originalWidth;

    private void Awake()
	{
		if (!IsAdSceneBackground)
			DontDestroyOnLoad(transform.gameObject);
	}

	void Start()
	{
		_originalHeight = transform.lossyScale.y;
        _originalWidth = transform.lossyScale.x;
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
        float worldScreenWidth = worldScreenHeight * Camera.main.aspect;
        transform.localScale = new Vector3(worldScreenWidth, worldScreenHeight, 1);
        Renderer.material.mainTextureScale = new Vector2 (transform.lossyScale.x / _originalWidth, transform.lossyScale.y / _originalHeight);
	}

    public void AdjustHorizontalToCamera()
    {
        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight * Camera.main.aspect;
        transform.localScale = new Vector3(worldScreenWidth, transform.localScale.y, 1);
        Renderer.material.mainTextureScale = new Vector2(transform.lossyScale.x / _originalWidth, 1.0f);
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
