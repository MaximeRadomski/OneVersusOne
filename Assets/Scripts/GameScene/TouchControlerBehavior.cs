using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControlerBehavior : MonoBehaviour
{

    private Vector3 _touchPosWorld;

	private GameObject _leftP1;
	private GameObject _rightP1;
	private GameObject _liftP1;
	private GameObject _throwP1;
	private GameObject _superP1;
	private GameObject _leftP2;
	/*private GameObject _rightP2;
	private GameObject _liftP2;
	private GameObject _throwP2;*/
	private GameObject _aiP1;
	private GameObject _aiP2;

	private GameObject _currentButton;

	private GameObject _playerOne, _playerTwo;
    
    void Start ()
	{
		_leftP1 = GameObject.Find ("LeftP1");
		_rightP1 = GameObject.Find ("RightP1");
		_liftP1 = GameObject.Find ("LiftP1");
		_throwP1 = GameObject.Find ("ThrowP1");
		_superP1 = GameObject.Find ("SuperP1");
		_leftP2 = GameObject.Find ("LeftP2");
		/*_rightP2 = GameObject.Find ("RightP2");
		_liftP2 = GameObject.Find ("LiftP2");
		_throwP2 = GameObject.Find ("ThrowP2");*/
		_aiP1 = GameObject.Find ("AiP1");
		_aiP2 = GameObject.Find ("AiP2");

		_currentButton = null;

		//_playerOne = GameObject.Find ("PlayerOne");
		//_playerTwo = GameObject.Find ("PlayerTwo");
	}
	
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
					if (touchedObject.tag == "Button" && touchedObject.GetComponent<BoxCollider2D>().enabled)
	                {
	                    if (Input.GetTouch(i).phase == TouchPhase.Ended)
	                        touchedObject.GetComponent<TouchControlBehavior>().EndAction();
	                    else if (Input.GetTouch(i).phase == TouchPhase.Began)
	                        touchedObject.GetComponent<TouchControlBehavior>().BeganAction();
                        else
                            touchedObject.GetComponent<TouchControlBehavior>().DoAction();
	                }
					else if (touchedObject.tag == "ButtonPopup")
					{
						if (Input.GetTouch(i).phase == TouchPhase.Began)
						{
							touchedObject.GetComponent<GenericMenuButtonBehavior>().PressSprite();
							/*if (_currentButton != null)
								_currentButton.GetComponent<GenericMenuButtonBehavior>().SwitchSprite();*/
							_currentButton = touchedObject;
						}
					}
					else if (touchedObject.tag == "Player" && touchedObject.GetComponent<PlayerBehavior>().Player == CurrentPlayer.PlayerTwo)
					{
						touchedObject.GetComponent<TouchControlBehavior>().EndAction();
					}
	            }
				else if (hitInformation.collider == null && Input.GetTouch(i).phase != TouchPhase.Began && Input.GetTouch(i).phase != TouchPhase.Ended)
				{
					if (touchPosWorld2D.y < 0)
						_leftP1.GetComponent<TouchControlBehavior>().EndAction();
					else
						_leftP2.GetComponent<TouchControlBehavior>().EndAction();
				}
				if (Input.GetTouch(i).phase == TouchPhase.Ended && _currentButton != null && _currentButton.tag == "ButtonPopup")
				{
					_currentButton.GetComponent<GenericMenuButtonBehavior>().DoAction();
					ResetCurrentButton();
					_currentButton = null;
				}
	        }
	    }
		/*else {
			if (_playerOne.GetComponent<PlayerBehavior>().IsMoving)
				_leftP1.GetComponent<TouchControlBehavior>().EndAction();
			if (_playerTwo.GetComponent<PlayerBehavior>().IsMoving)
				_leftP2.GetComponent<TouchControlBehavior>().EndAction();
		}*/

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

		if (Input.GetButtonDown("ThrowP1") && _throwP1.GetComponent<SpriteRenderer>().enabled == true)
			_throwP1.GetComponent<TouchControlBehavior> ().BeganAction ();
		else if (Input.GetButtonDown("ThrowP1") && _throwP1.GetComponent<SpriteRenderer>().enabled == false)
			_superP1.GetComponent<TouchControlBehavior> ().BeganAction ();

		if (Input.GetButtonDown("LiftP1") && _throwP1.GetComponent<SpriteRenderer>().enabled == true)
			_liftP1.GetComponent<TouchControlBehavior> ().BeganAction ();
		else if (Input.GetButtonDown("LiftP1") && _throwP1.GetComponent<SpriteRenderer>().enabled == false)
			_superP1.GetComponent<TouchControlBehavior> ().BeganAction ();

		if (Input.GetButtonUp("AiP1"))
			_aiP1.GetComponent<TouchControlBehavior> ().EndAction ();
		if (Input.GetButtonUp("AiP2"))
			_aiP2.GetComponent<TouchControlBehavior> ().EndAction ();


		//#else
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
				if (touchedObject.tag == "ButtonPopup")
				{
					touchedObject.GetComponent<GenericMenuButtonBehavior>().SwitchSprite();
					if (_currentButton != null)
						_currentButton.GetComponent<GenericMenuButtonBehavior>().SwitchSprite();
					_currentButton = touchedObject;
					_currentButton.GetComponent<GenericMenuButtonBehavior>().DoAction();
					ResetCurrentButton();
					_currentButton = null;
				}
			}
		}
		#endif
    }

	private void ResetCurrentButton()
	{
		if (_currentButton.GetComponent<GenericMenuButtonBehavior>().KeepState == false)
			_currentButton.GetComponent<GenericMenuButtonBehavior>().SwitchSprite();
	}
}
