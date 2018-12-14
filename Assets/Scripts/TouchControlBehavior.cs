using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControlBehavior : MonoBehaviour
{
    public CurrentPlayer Player;
    public GenericTapAction Action;

    private GameObject _player;
    private GameObject _ball;
    private float _distanceWall;
    private float _distanceDash;

    void Start ()
    {
        _player = GameObject.Find(GetFocusedPayerName());
        _ball = GameObject.Find("Ball");
        _distanceWall = 1.68f;
        _distanceDash = 0.5f;
    }

    public void DoAction()
    {
        switch (Action)
        {
            case GenericTapAction.Left:
                if (_player.GetComponent<PlayerBehavior>().HasTheDisc == false)
                    Move(-0.05f, true);
                else
                    Move(-0.01f, true);
                break;
            case GenericTapAction.Right:
                if (_player.GetComponent<PlayerBehavior>().HasTheDisc == false)
                    Move(0.05f, true);
                else
                    Move(0.01f, true);
                break;
        }
    }

    public void BeganAction()
    {
        switch (Action)
        {
            case GenericTapAction.Left:
                if (_player.GetComponent<PlayerBehavior>().HasTheDisc == true)
                    _player.GetComponent<PlayerBehavior>().DecrementAngle();
                break;
            case GenericTapAction.Right:
                if (_player.GetComponent<PlayerBehavior>().HasTheDisc == true)
                    _player.GetComponent<PlayerBehavior>().IncrementAngle();
                break;
            case GenericTapAction.Throw:
                if (_player.GetComponent<PlayerBehavior>().HasTheDisc)
                    Throw();
                else
                    Dash();
                break;
        }
    }

    public void EndAction()
    {
        switch (Action)
        {
            case GenericTapAction.Left:
                _player.GetComponent<PlayerBehavior>().IsGoingLeft = false;
                _player.GetComponent<PlayerBehavior>().IsGoingRight = false;
                break;
            case GenericTapAction.Right:
                _player.GetComponent<PlayerBehavior>().IsGoingLeft = false;
                _player.GetComponent<PlayerBehavior>().IsGoingRight = false;
                break;
        }
    }

    private void Move(float distance, bool isLeft)
    {
        _player.transform.position += new Vector3(distance, 0.0f, 0.0f);
        _player.GetComponent<PlayerBehavior>().IsGoingLeft = isLeft;
        _player.GetComponent<PlayerBehavior>().IsGoingRight = !isLeft;
        if (_player.transform.position.x < -1.0f * _distanceWall)
            _player.transform.position = new Vector3(-1.0f * _distanceWall, _player.transform.position.y, 0.0f);
        if (_player.transform.position.x > _distanceWall)
            _player.transform.position = new Vector3(_distanceWall, _player.transform.position.y, 0.0f);
    }

    private void Throw()
    {
        if (_ball == null)
            _ball = GetBall();
        if (_player.GetComponent<PlayerBehavior>().HasTheDisc)
        {
            _ball.GetComponent<BallBehavior>().Throw(_player.GetComponent<PlayerBehavior>().DirectionalVector + 
                                                     new Vector2(_player.GetComponent<PlayerBehavior>().ThrowAngle, 0.0f));
            _player.GetComponent<PlayerBehavior>().HasTheDisc = false;
        }
    }

    private void Dash()
    {
        if (_player.GetComponent<PlayerBehavior>().CanDash == false)
            return;
        if (_player.GetComponent<PlayerBehavior>().IsGoingLeft)
        {
            Move(-1.0f * _distanceDash, true);
        }
        else if (_player.GetComponent<PlayerBehavior>().IsGoingRight)
        {
            Move(_distanceDash, false);
        }
        _player.GetComponent<PlayerBehavior>().Dash();
    }

    private GameObject GetBall()
    {
        return GameObject.Find("Ball");
    }

    private string GetFocusedPayerName()
    {
        if (Player == CurrentPlayer.PlayerOne)
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
}
