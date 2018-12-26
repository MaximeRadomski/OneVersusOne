using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControlBehavior : MonoBehaviour
{
    public CurrentPlayer Player;
    public GenericTapAction Action;

    private GameObject _player;

    void Start ()
    {
        _player = GameObject.Find(GetFocusedPayerName());
    }

    public void DoAction()
    {
        switch (Action)
        {
		case GenericTapAction.Left:
		        _player.GetComponent<PlayerBehavior>().Move(Direction.Left);
                break;
            case GenericTapAction.Right:
                _player.GetComponent<PlayerBehavior>().Move(Direction.Right);
                break;
        }
    }

    public void BeganAction()
    {
        switch (Action)
        {
            case GenericTapAction.Left:
                _player.GetComponent<PlayerBehavior>().DecrementAngle();
                break;
            case GenericTapAction.Right:
                _player.GetComponent<PlayerBehavior>().IncrementAngle();
                break;
            case GenericTapAction.Throw:
                _player.GetComponent<PlayerBehavior>().Throw();
                break;
			case GenericTapAction.Lift:
				_player.GetComponent<PlayerBehavior> ().Lift ();
				break;
        }
    }

    public void EndAction()
    {
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
        }
    }

    private string GetFocusedPayerName()
    {
        if (Player == CurrentPlayer.PlayerOne)
            return "PlayerOne";
        return "PlayerTwo";
    }

	private void ActivateAI()
	{
		if (GameObject.Find (GetFocusedPayerName()).GetComponent<AI> ().enabled == false)
			GameObject.Find (GetFocusedPayerName()).GetComponent<AI> ().enabled = true;
		else
			GameObject.Find (GetFocusedPayerName()).GetComponent<AI> ().enabled = false;
	}

    public enum GenericTapAction
    {
        Left,
        Right,
        Throw,
        Lift,
		PlayerAI
    }
}
