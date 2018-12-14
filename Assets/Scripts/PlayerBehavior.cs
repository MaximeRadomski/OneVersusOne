using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public CurrentPlayer Player;
    public bool IsGoingLeft, IsGoingRight;
    public bool HasTheDisc;
    public Vector2 DirectionalVector;
    public bool CanDash = true;
    public float DashCooldown;
    public float ThrowAngle;

	void Start ()
	{
		if (Player == CurrentPlayer.PlayerOne)
            DirectionalVector = Vector2.up;
        else
            DirectionalVector = Vector2.down;
	}

    public void IncrementAngle()
    {
        var tmpThrowAngle = ThrowAngle + 0.5f;
        if (tmpThrowAngle >= -1.5f && tmpThrowAngle <= 1.5f)
            ThrowAngle = tmpThrowAngle;
    }

    public void DecrementAngle()
    {
        var tmpThrowAngle = ThrowAngle - 0.5f;
        if (tmpThrowAngle >= -1.5f && tmpThrowAngle <= 1.5f)
            ThrowAngle = tmpThrowAngle;
    }

    public void Dash()
    {
        CanDash = false;
        Invoke("ResetDash", DashCooldown);
    }

    private void ResetDash()
    {
        CanDash = true;
    }

}
