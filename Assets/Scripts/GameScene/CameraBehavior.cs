using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public Camera Camera;

	private int _nbReset;

    private float _initialCameraSize;
	private Vector3 _initialCameraPosition;

    void Start()
    {
		_nbReset = 0;

        _initialCameraSize = Camera.orthographicSize;
		_initialCameraPosition = Camera.transform.position;
    }

    public void WallHit()
    {
        Camera.orthographicSize -= 0.025f;
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
		Camera.orthographicSize -= 0.025f;
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
