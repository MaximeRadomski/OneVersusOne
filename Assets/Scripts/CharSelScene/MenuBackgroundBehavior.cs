using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackgroundBehavior : MonoBehaviour
{
	public float ScrollSpeed;
	public MeshRenderer Renderer;

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
