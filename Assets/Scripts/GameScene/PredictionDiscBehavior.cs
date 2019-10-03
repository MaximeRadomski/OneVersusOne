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
            AI ai = null;
            if (col.gameObject.tag == "Player" && (ai = col.gameObject.GetComponent<AI>()) != null &&
                GenericHelpers.FloatEqualsPrecision(Physics2D.gravity.x, 0.0f, 0.1f))
            {
                ai.HasHitPredictionDisc = true;
            }
            Destroy(gameObject);
        }
    }
}
