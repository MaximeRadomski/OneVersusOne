using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackgroundBehavior : MonoBehaviour
{
	public float ScrollSpeed;
	public MeshRenderer Renderer;

	private void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	void Start()
	{
		Renderer.sortingLayerName = "Background";
	}

	void Update ()
	{
		float offset = Time.time * ScrollSpeed;
		Renderer.material.mainTextureOffset = new Vector2(offset, offset/2);
	}
}
