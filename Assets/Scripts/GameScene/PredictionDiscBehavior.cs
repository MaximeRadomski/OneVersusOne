using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictionDiscBehavior : MonoBehaviour
{
    private bool _canBeDestroyedByPlayer;

    void Start()
    {
        _canBeDestroyedByPlayer = false;
        Invoke("CanBeDestroyedByPlayer", 0.05f);
    }

    private void CanBeDestroyedByPlayer()
    {
        _canBeDestroyedByPlayer = true;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if ((col.gameObject.tag == "Player" && _canBeDestroyedByPlayer)
            || col.gameObject.tag == "Goal"
            || col.gameObject.tag == "TrainingWall"
            || col.gameObject.tag == "FrozenWall"
            || col.gameObject.tag == "Target")
        {
            Debug.Log("Ouch!");
            Destroy(gameObject);
        }
    }
}
