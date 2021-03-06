﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSelTouchControlerBehavior : MonoBehaviour
{
	private Vector3 _touchPosWorld;
	//private GameObject _currentButton = null;

	void Update ()
	{
		#if UNITY_ANDROID
		if (Input.touchCount > 0)
		{
			for (int i = 0; i < Input.touchCount; i++)
			{
				//We transform the touch position into word space from screen space and store it.
				_touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);

				Vector2 touchPosWorld2D = new Vector2(_touchPosWorld.x, _touchPosWorld.y);

				//We now raycast with this information. If we have hit something we can process it.
				RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

				if (hitInformation.collider != null)
				{
					//We should have hit something with a 2D Physics collider!
					GameObject touchedObject = hitInformation.transform.gameObject;
					//touchedObject should be the object someone touched.
					//Debug.Log("Touched " + touchedObject.transform.name);
					if (touchedObject.tag == "Button")
					{
						if (Input.GetTouch(i).phase == TouchPhase.Began)
						{
							touchedObject.GetComponent<CharSelButtonBehavior>().SwitchSprite();
							touchedObject.GetComponent<CharSelButtonBehavior>().DoAction();
							//_currentButton = touchedObject;
						}
					}
					else if (touchedObject.tag == "BackButton")
					{
						if (Input.GetTouch(i).phase == TouchPhase.Began)
						{
							touchedObject.GetComponent<GenericMenuButtonBehavior>().DoAction();
						}
					}
				}
				/*if (Input.GetTouch(i).phase == TouchPhase.Ended && _currentButton != null)
				{
					_currentButton.GetComponent<CharSelButtonBehavior>().DoAction();
					_currentButton = null;
				}*/
			}
		}
		#else
		if (Input.GetMouseButtonDown (0))
		{
			//We transform the touch position into word space from screen space and store it.
			_touchPosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			Vector2 touchPosWorld2D = new Vector2(_touchPosWorld.x, _touchPosWorld.y);

			//We now raycast with this information. If we have hit something we can process it.
			RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

			if (hitInformation.collider != null)
			{
				//We should have hit something with a 2D Physics collider!
				GameObject touchedObject = hitInformation.transform.gameObject;
				//touchedObject should be the object someone touched.
				//Debug.Log("Touched " + touchedObject.transform.name);
				if (touchedObject.tag == "Button" || touchedObject.tag == "BackButton")
				{
					touchedObject.GetComponent<CharSelButtonBehavior>().DoAction();
				}
			}
		}
		#endif
	}
}
