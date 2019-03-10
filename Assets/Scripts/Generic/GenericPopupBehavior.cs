using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPopupBehavior : MonoBehaviour
{
	private GameObject[] _listButtons;

	void Start ()
	{
		_listButtons = GameObject.FindGameObjectsWithTag ("Button");

		EnableDisableHitBoxes (false);
	}

	private void EnableDisableHitBoxes(bool state)
	{
		BoxCollider2D tmpHitBox;
		foreach (var button in _listButtons)
		{
			if ((tmpHitBox = button != null ? button.GetComponent<BoxCollider2D> () : null) != null)
				tmpHitBox.enabled = state;
		}
	}

	void OnDestroy()
	{
		EnableDisableHitBoxes (true);
	}
}
