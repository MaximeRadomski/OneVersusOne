using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public Camera Camera;
	public float sceneWidth = 4;

	private int _nbReset;

    private float _initialCameraSize;
	private Vector3 _initialCameraPosition;

    void Start()
    {
        //if (Screen.currentResolution.)
		float unitsPerPixel = sceneWidth / Screen.width;
		float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;
        if (desiredHalfHeight > 3.50f)
		    Camera.orthographicSize = desiredHalfHeight;

		_nbReset = 0;
		_initialCameraSize = Camera.orthographicSize;
		_initialCameraPosition = Camera.transform.position;
    }

    public void WallHit()
    {
        Camera.orthographicSize -= 0.040f;
		++_nbReset;
        Invoke("ResetCamera", 0.1f);
    }

    private void ResetCamera()
    {
		_nbReset--;
		if (_nbReset > 0)
			return;
        Camera.orthographicSize = _initialCameraSize;
		Camera.transform.position = _initialCameraPosition;
    }

	public void GoalHit(float goalYAxe)
    {
		_nbReset = 5;
		Camera.orthographicSize -= 0.040f;
		ShakyCam ();
    }

	private void ShakyCam()
	{
		transform.position = new Vector3 (Random.Range (-0.1f, 0.1f), Random.Range (-0.1f, 0.1f), 0.0f);
		if (--_nbReset == 0)
			Invoke ("ResetCamera", 0.1f);
		else
			Invoke ("ShakyCam", 0.1f);
	}
}
