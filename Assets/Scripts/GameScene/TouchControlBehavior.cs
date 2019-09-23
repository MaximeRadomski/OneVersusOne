﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControlBehavior : MonoBehaviour
{
    public CurrentPlayer Player;
    public GenericTapAction Action;

    private GameObject _player;
    private int _directionKeptHolded;
    private int _directionKeptHoldedStartValue;
    private float _directionKeptHoldedDelay;

    void Start ()
    {
        _player = GameObject.Find(GetFocusedPayerName());
        _directionKeptHolded = 0;
        _directionKeptHoldedDelay = 0.25f;
        _directionKeptHoldedStartValue = -10;
    }

    public void DoAction()
    {
		if (_player.GetComponent<PlayerBehavior> ().IsControlledByAI)
			return;
        switch (Action)
        {
		case GenericTapAction.Left:
	            _player.GetComponent<PlayerBehavior>().Move(Direction.Left);
                ++_directionKeptHolded;
                break;
        case GenericTapAction.Right:
                _player.GetComponent<PlayerBehavior>().Move(Direction.Right);
                ++_directionKeptHolded;
                break;
        }
    }

    public void BeganAction()
    {
		if (_player == null)
			_player = GameObject.Find(GetFocusedPayerName());
		if (_player.GetComponent<PlayerBehavior> ().IsControlledByAI)
			return;
        switch (Action)
        {
            case GenericTapAction.Left:
                _player.GetComponent<PlayerBehavior>().DecrementAngle();
                _directionKeptHolded = _directionKeptHoldedStartValue;
                Invoke("CheckIfDirectionKeptHolded", _directionKeptHoldedDelay);
                break;
            case GenericTapAction.Right:
                _player.GetComponent<PlayerBehavior>().IncrementAngle();
                _directionKeptHolded = _directionKeptHoldedStartValue;
                Invoke("CheckIfDirectionKeptHolded", _directionKeptHoldedDelay);
                break;
            case GenericTapAction.Throw:
                _player.GetComponent<PlayerBehavior>().Throw();
                break;
			case GenericTapAction.Lift:
				_player.GetComponent<PlayerBehavior> ().Lift ();
				break;
			case GenericTapAction.Super:
				_player.GetComponent<PlayerBehavior>().Super();
				break;
        }
		StretchOnPress ();
    }

    public void EndAction()
    {
        _directionKeptHolded = _directionKeptHoldedStartValue;
        if (_player == null || _player.GetComponent<PlayerBehavior> ().IsControlledByAI && Action != GenericTapAction.PlayerAI)
			return;
        switch (Action)
        {
			case GenericTapAction.Left:
			    _player.GetComponent<PlayerBehavior>().Standby();
                break;
            case GenericTapAction.Right:
                _player.GetComponent<PlayerBehavior>().Standby();
                break;
			case GenericTapAction.PlayerAI:
				ActivateAI ();
				break;
			case GenericTapAction.Field:
				GameObject.Find("$GameManager").GetComponent<GameManagerBehavior>().OnBackButtonPressed();
				break;
        }
    }

    private string GetFocusedPayerName()
    {
        if (Player == CurrentPlayer.PlayerOne)
            return "PlayerOne";
        return "PlayerTwo";
    }

    private void CheckIfDirectionKeptHolded()
    {
        if (_directionKeptHolded > 0)
        {
            switch (Action)
            {
                case GenericTapAction.Left:
                    _player.GetComponent<PlayerBehavior>().DecrementAngle();
                    break;
                case GenericTapAction.Right:
                    _player.GetComponent<PlayerBehavior>().IncrementAngle();
                    break;
            }
            Invoke("CheckIfDirectionKeptHolded", _directionKeptHoldedDelay);
        }
    }


    private void ActivateAI()
	{
	    if (GameObject.Find(GetFocusedPayerName()).GetComponent<AI>().enabled == false)
	    {
	        GameObject.Find(GetFocusedPayerName()).GetComponent<AI>().enabled = true;
	        gameObject.GetComponent<SpriteRenderer>().enabled = true;
	    }
	    else
	    {
	        GameObject.Find(GetFocusedPayerName()).GetComponent<AI>().enabled = false;
	        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
	}

	private void StretchOnPress()
	{
		transform.localScale = new Vector3 (1.1f, 0.9f, 1.0f);
		Invoke ("ResetStretch", 0.1f);
	}

	private void ResetStretch()
	{
		transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
	}

    public enum GenericTapAction
    {
        Left,
        Right,
        Throw,
        Lift,
		PlayerAI,
		Super,
		Field
    }

}
