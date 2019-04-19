using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBehavior : MonoBehaviour
{
	public CurrentPlayer Player;

	private GameObject _linkedPlayer;
	private float _y = 0;

	void Start ()
	{
		if (Player == CurrentPlayer.PlayerOne) {
			_linkedPlayer = GameObject.Find ("PlayerOne");
			_y = -0.154f;
		} else {
			_linkedPlayer = GameObject.Find ("PlayerTwo");
			_y = 0.013f;
		}
	}

	void Update ()
	{
		if (_linkedPlayer == null) {
			if (Player == CurrentPlayer.PlayerOne) {
				_linkedPlayer = GameObject.Find ("PlayerOne");
			} else {
				_linkedPlayer = GameObject.Find ("PlayerTwo");
			}
		}	
		transform.position = _linkedPlayer.transform.position +	new Vector3 (0.0278f, _y, 0.0f);
	}
}
