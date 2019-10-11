using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupNumbersBehavior : MonoBehaviour
{
	public GameObject[] Buttons;
	public GameObject ButtonDel;
	public int Number;

	private string _numberToString;
	private UnityEngine.UI.Text _text;
	private bool _hasInputBar;
	private float _inputBarDelay;

	void Start ()
	{
		_numberToString = "";
		_hasInputBar = true;
		_inputBarDelay = 0.4f;
		_text = GameObject.Find ("PopupText").GetComponent<UnityEngine.UI.Text> ();
		ActualizeNumber ();

		Buttons[0].GetComponent<GenericMenuButtonBehavior>().buttonDelegate = AddZero;
		Buttons[1].GetComponent<GenericMenuButtonBehavior>().buttonDelegate = AddOne;
		Buttons[2].GetComponent<GenericMenuButtonBehavior>().buttonDelegate = AddTwo;
		Buttons[3].GetComponent<GenericMenuButtonBehavior>().buttonDelegate = AddThree;
		Buttons[4].GetComponent<GenericMenuButtonBehavior>().buttonDelegate = AddFour;
		Buttons[5].GetComponent<GenericMenuButtonBehavior>().buttonDelegate = AddFive;
		Buttons[6].GetComponent<GenericMenuButtonBehavior>().buttonDelegate = AddSix;
		Buttons[7].GetComponent<GenericMenuButtonBehavior>().buttonDelegate = AddSeven;
		Buttons[8].GetComponent<GenericMenuButtonBehavior>().buttonDelegate = AddEight;
		Buttons[9].GetComponent<GenericMenuButtonBehavior>().buttonDelegate = AddNine;
		ButtonDel.GetComponent<GenericMenuButtonBehavior>().buttonDelegate = Del;

		Invoke ("InputBar", _inputBarDelay);
	}

    private void InputBar()
	{
		_hasInputBar = !_hasInputBar;
        if (_hasInputBar)
            _text.transform.position = new Vector3(0.0555f, _text.transform.position.y, 0.0f);
        else
            _text.transform.position = new Vector3(0.0f, _text.transform.position.y, 0.0f);
        AddRemoveInputBar ();
		Invoke ("InputBar", _inputBarDelay);
	}

	private void AddRemoveInputBar ()
	{
		if (_hasInputBar)
			_text.text = _text.text + "|";
		else if (_text.text.Length > 0 && _text.text [_text.text.Length - 1] == '|')
			_text.text = _text.text.Substring(0, _text.text.Length - 1);
	}

	private void ActualizeNumber ()
	{
		if (_numberToString.Length > 2)
			_numberToString = _numberToString.Substring(0, _numberToString.Length - 1);
		_text.text = _numberToString;
		AddRemoveInputBar ();
		if (_numberToString != null && _numberToString != "")
			Number = int.Parse (_numberToString);
		else
			Number = 0;
	}

	private void AddZero()
	{
		_numberToString = _numberToString + "0";
		ActualizeNumber ();
	}

	private void AddOne()
	{
		_numberToString = _numberToString + "1";
		ActualizeNumber ();
	}

	private void AddTwo()
	{
		_numberToString = _numberToString + "2";
		ActualizeNumber ();
	}

	private void AddThree()
	{
		_numberToString = _numberToString + "3";
		ActualizeNumber ();
	}

	private void AddFour()
	{
		_numberToString = _numberToString + "4";
		ActualizeNumber ();
	}

	private void AddFive()
	{
		_numberToString = _numberToString + "5";
		ActualizeNumber ();
	}

	private void AddSix()
	{
		_numberToString = _numberToString + "6";
		ActualizeNumber ();
	}

	private void AddSeven()
	{
		_numberToString = _numberToString + "7";
		ActualizeNumber ();
	}

	private void AddEight()
	{
		_numberToString = _numberToString + "8";
		ActualizeNumber ();
	}

	private void AddNine()
	{
		_numberToString = _numberToString + "9";
		ActualizeNumber ();
	}

	private void Del()
	{
		if (_numberToString.Length > 0)
		{
			_numberToString = _numberToString.Substring(0, _numberToString.Length - 1);
			ActualizeNumber ();
		}
	}
}
