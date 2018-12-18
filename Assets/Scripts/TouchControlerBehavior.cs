using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControlerBehavior : MonoBehaviour
{

    private Vector3 _touchPosWorld;

	private GameObject _leftP1;
	private GameObject _rightP1;
	private GameObject _specialP1;
	private GameObject _throwP1;
	/*private GameObject _leftP2;
	private GameObject _rightP2;
	private GameObject _specialP2;
	private GameObject _throwP2;*/
    
    void Start ()
	{
		_leftP1 = GameObject.Find ("LeftP1");
		_rightP1 = GameObject.Find ("RightP1");
		_specialP1 = GameObject.Find ("SpecialP1");
		_throwP1 = GameObject.Find ("ThrowP1");
		/*_leftP2 = GameObject.Find ("LeftP2");
		_rightP2 = GameObject.Find ("RightP2");
		_specialP2 = GameObject.Find ("SpecialP2");
		_throwP2 = GameObject.Find ("ThrowP2");*/
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
	                    if (Input.GetTouch(i).phase == TouchPhase.Ended)
	                        touchedObject.GetComponent<TouchControlBehavior>().EndAction();
	                    else if (Input.GetTouch(i).phase == TouchPhase.Began)
	                        touchedObject.GetComponent<TouchControlBehavior>().BeganAction();
                        else
                            touchedObject.GetComponent<TouchControlBehavior>().DoAction();
                        
	                }
	            }
	        }
	    }


		if (Input.GetButtonDown("LeftP1"))
			_leftP1.GetComponent<TouchControlBehavior> ().BeganAction ();
		else if (Input.GetButtonDown("RightP1"))
			_rightP1.GetComponent<TouchControlBehavior> ().BeganAction ();
		else if (Input.GetButton("LeftP1"))
			_leftP1.GetComponent<TouchControlBehavior> ().DoAction ();
		else if (Input.GetButton("RightP1"))
			_rightP1.GetComponent<TouchControlBehavior> ().DoAction ();
		else if (Input.GetButtonUp("LeftP1"))
			_leftP1.GetComponent<TouchControlBehavior> ().EndAction ();
		else if (Input.GetButtonUp("RightP1"))
			_rightP1.GetComponent<TouchControlBehavior> ().EndAction ();

		if (Input.GetButtonDown("ThrowP1"))
			_throwP1.GetComponent<TouchControlBehavior> ().BeganAction ();

		if (Input.GetButtonDown("SpecialP1"))
			_specialP1.GetComponent<TouchControlBehavior> ().EndAction ();
    }
}
