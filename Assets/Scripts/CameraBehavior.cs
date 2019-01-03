using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public Camera Camera;

	private int _nbReset;

    private float _initialCameraSize;
	private Vector3 _initialCameraPosition;
	private float _goalDirectionMultiplier;

    void Start()
    {
		_nbReset = 0;

        _initialCameraSize = Camera.orthographicSize;
		_initialCameraPosition = Camera.transform.position;
		_goalDirectionMultiplier = 0;
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
		_goalDirectionMultiplier = 0;
    }

	public void GoalHit(float goalYAxe)
    {
		if (goalYAxe > 0)
			_goalDirectionMultiplier = 1.0f;
		else
			_goalDirectionMultiplier = -1.0f;
		transform.position += new Vector3 (0.0f, 0.2f * _goalDirectionMultiplier, 0.0f);

		//Camera.orthographicSize -= 0.2f;
		++_nbReset;
		Invoke("ResetCamera", 0.6f);
    }
}
