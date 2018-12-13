using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControlBehavior : MonoBehaviour
{
    public FocusedPlayer Player;
    public GenericTapAction Action;

    private GameObject _player;
    private GameObject _ball;

    void Start ()
    {
        _player = GameObject.Find(GetFocusedPayerName());
        _ball = GameObject.Find("Ball");
    }

    public void DoAction()
    {
        switch (Action)
        {
            case GenericTapAction.Left:
                if (_player.transform.position.x > -1.5f)
                    _player.transform.position += new Vector3(-0.05f, 0.0f, 0.0f);
                break;
            case GenericTapAction.Right:
                if (_player.transform.position.x < 1.5f)
                    _player.transform.position += new Vector3(0.05f, 0.0f, 0.0f);
                break;
            case GenericTapAction.Special:
                break;
            case GenericTapAction.Throw:
                Throw();
                break;
        }
    }

    private void Throw()
    {
        if (Player == FocusedPlayer.PlayerOne)
        {
            if (_ball.GetComponent<BallBehavior>().IsLinkedToPlayerOne)
            {
                _ball.GetComponent<BallBehavior>().IsLinkedToPlayerOne = false;
                _ball.GetComponent<BallBehavior>().Throw(Vector2.up);
            }
        }
        else
        {
            if (_ball.GetComponent<BallBehavior>().IsLinkedToPlayerTwo)
            {
                _ball.GetComponent<BallBehavior>().IsLinkedToPlayerTwo = false;
                _ball.GetComponent<BallBehavior>().Throw(Vector2.down);

            }
        }
    }

    private string GetFocusedPayerName()
    {
        if (Player == FocusedPlayer.PlayerOne)
            return "PlayerOne";
        return "PlayerTwo";
    }

    public enum GenericTapAction
    {
        Left,
        Right,
        Throw,
        Special
    }

    public enum FocusedPlayer
    {
        PlayerOne,
        PlayerTwo
    }
}
