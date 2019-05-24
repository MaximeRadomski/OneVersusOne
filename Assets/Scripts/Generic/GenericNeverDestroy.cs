using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericNeverDestroy : MonoBehaviour
{
	private void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}
}
