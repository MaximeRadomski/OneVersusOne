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
		BoxCollider2D[] tmpHitBoxes;
		foreach (var button in _listButtons)
		{
            if ((tmpHitBoxes = button != null ? button.GetComponents<BoxCollider2D>() : null) != null)
            {
                foreach (var hitbox in tmpHitBoxes)
                {
                    hitbox.enabled = state;
                }
            }
		}
	}

	void OnDestroy()
	{
		EnableDisableHitBoxes (true);
	}
}
