using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControlBehavior : MonoBehaviour
{
    public CurrentPlayer Player;
    public GenericTapAction Action;
    public int DirectionKeptHolded;

    private GameObject _player;
    private int _directionKeptHoldedStartValue;
    private float _directionKeptHoldedDelay;
    private TouchControlBehavior _otherArrowBehavior;

    void Start ()
    {
        _player = GameObject.Find(GetFocusedPayerName());
        _directionKeptHoldedDelay = 0.25f;
        _directionKeptHoldedStartValue = -10;
        DirectionKeptHolded = _directionKeptHoldedStartValue;
        if (Action == GenericTapAction.Left)
            _otherArrowBehavior = GameObject.Find("RightP" + (Player == CurrentPlayer.PlayerOne ? "2" : "1")).GetComponent<TouchControlBehavior>();
        else if (Action == GenericTapAction.Right)
            _otherArrowBehavior = GameObject.Find("LeftP" + (Player == CurrentPlayer.PlayerOne ? "2" : "1")).GetComponent<TouchControlBehavior>();
    }

    public void DoAction()
    {
		if (_player.GetComponent<PlayerBehavior> ().IsControlledByAI)
			return;
        switch (Action)
        {
		case GenericTapAction.Left:
	            _player.GetComponent<PlayerBehavior>().Move(Direction.Left);
                ++DirectionKeptHolded;
                break;
        case GenericTapAction.Right:
                _player.GetComponent<PlayerBehavior>().Move(Direction.Right);
                ++DirectionKeptHolded;
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
                ResetDirectionHolded();
                Invoke("CheckIfDirectionKeptHolded", _directionKeptHoldedDelay);
                break;
            case GenericTapAction.Right:
                _player.GetComponent<PlayerBehavior>().IncrementAngle();
                ResetDirectionHolded();
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
        ResetDirectionHolded();
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
        if (DirectionKeptHolded > 0)
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

    private void ResetDirectionHolded()
    {
        DirectionKeptHolded = _directionKeptHoldedStartValue;
        if (_otherArrowBehavior != null)
            _otherArrowBehavior.DirectionKeptHolded = _directionKeptHoldedStartValue;
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
