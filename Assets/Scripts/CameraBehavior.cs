using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public Camera Camera;

    private float _initialCameraSize;
    void Start()
    {
        _initialCameraSize = Camera.orthographicSize;
    }

    public void WallHit()
    {
        Camera.orthographicSize -= 0.025f;
        Invoke("ResetWallHit", 0.1f);
    }

    private void ResetWallHit()
    {
        Camera.orthographicSize = _initialCameraSize;
    }

    public void GoalHit()
    {

    }
}
