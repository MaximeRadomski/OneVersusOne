using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayMenuManager : MonoBehaviour
{
	private GameObject _tutorialNext, _tutorialPrevious;
	private UnityEngine.UI.Text _tutorialTitle, _tutorialContent;
	private int _tutorialIteration;

	private Color _transparentColor;
	private Color _fullColor;

	void Start ()
	{
		_tutorialTitle = GameObject.Find ("TutorialTitle").GetComponent<UnityEngine.UI.Text> ();
		_tutorialContent = GameObject.Find ("TutorialContent").GetComponent<UnityEngine.UI.Text> ();
		_tutorialNext = GameObject.Find ("TutorialNext");
		_tutorialPrevious = GameObject.Find ("TutorialPrevious");
		_tutorialIteration = 0;
		_transparentColor = new Color (1.0f, 1.0f, 1.0f, 0.0f);
		_fullColor = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		_tutorialNext.GetComponent<GenericMenuButtonBehavior>().buttonDelegate = NextTutorialIteration;
		_tutorialPrevious.GetComponent<GenericMenuButtonBehavior>().buttonDelegate = PreviousTutorialIteration;
		ChangeTutorialIteration ();
	}

	private void NextTutorialIteration()
	{
		if (_tutorialIteration + 1 >= TutorialData.Tutorials.Count)
			return;
		++_tutorialIteration;
		ChangeTutorialIteration ();
	}

	private void PreviousTutorialIteration()
	{
		if (_tutorialIteration - 1 < 0)
			return;
		--_tutorialIteration;
		ChangeTutorialIteration ();
	}

	private void ChangeTutorialIteration ()
	{
		if (_tutorialIteration == 0)
			_tutorialPrevious.GetComponent<UnityEngine.UI.Text> ().color = _transparentColor;
		else
			_tutorialPrevious.GetComponent<UnityEngine.UI.Text> ().color = _fullColor;

		if (_tutorialIteration == TutorialData.Tutorials.Count - 1)
			_tutorialNext.GetComponent<UnityEngine.UI.Text> ().color = _transparentColor;
		else
			_tutorialNext.GetComponent<UnityEngine.UI.Text> ().color = _fullColor;

		_tutorialTitle.text = TutorialData.Tutorials [_tutorialIteration].Title;
		_tutorialContent.text = TutorialData.Tutorials [_tutorialIteration].Content;

		var ball = GameObject.Find ("Ball");
		if (_tutorialIteration >= 2 && _tutorialIteration <= TutorialData.Tutorials.Count - 1) {
			if (ball == null)
				GameObject.Find ("$GameManager").GetComponent<GameManagerBehavior> ().PlaceBall ();
		}
	}
}
