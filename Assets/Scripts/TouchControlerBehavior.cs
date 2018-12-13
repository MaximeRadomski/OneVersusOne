﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControlerBehavior : MonoBehaviour
{

    private Vector3 _touchPosWorld;
    
    void Start ()
	{
		
	}
	
	void Update ()
	{
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
	                    touchedObject.GetComponent<TouchControlBehavior>().DoAction();
	                }
	            }
	        }
	    }
    }
}
