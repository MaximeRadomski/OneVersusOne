using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPCooldownBehavior : MonoBehaviour
{
	public CurrentPlayer Player;

	private PlayerBehavior _playerbehavior;
	private bool _hasTheDisc;

	void Start()
	{
		_hasTheDisc = true;
	}

	private void GetPlayer()
	{
		_playerbehavior = GameObject.Find (Player.ToString()).GetComponent<PlayerBehavior>();
	}

	void Update ()
	{
		if (_playerbehavior == null)
			GetPlayer ();
		if (_playerbehavior == null)
			return;

		if (_playerbehavior.HasTheDisc && !_hasTheDisc)
		{
			_hasTheDisc = true;
			DisableCurrentText ();
		}
		else if (!_playerbehavior.HasTheDisc && _hasTheDisc)
		{
			_hasTheDisc = false;
			if (_playerbehavior.SPCooldown == 0)
				DisableCurrentText ();
			else
				EnableCurrentText ();
		}
	}

	public void EnableCurrentText()
	{
		this.GetComponent<UnityEngine.UI.Text> ().enabled = true;
		this.GetComponent<UnityEngine.UI.Text> ().text = _playerbehavior.SPCooldown.ToString ();
	}

	public void DisableCurrentText()
	{
		this.GetComponent<UnityEngine.UI.Text> ().enabled = false;
	}
}
