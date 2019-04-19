using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayMenuManager : MonoBehaviour
{
	private UnityEngine.UI.Text _tutorialTitle;
	private UnityEngine.UI.Text _tutorialContent;

	void Start ()
	{
		_tutorialTitle = GameObject.Find ("TutorialTitle").GetComponent<UnityEngine.UI.Text> ();
		_tutorialContent = GameObject.Find ("TutorialContent").GetComponent<UnityEngine.UI.Text> ();
		ChangeTutorialIteration (0);
	}

	private void ChangeTutorialIteration (int id)
	{
		_tutorialTitle.text = TutorialData.Tutorials [id].Title;
		_tutorialContent.text = TutorialData.Tutorials [id].Content;
	}
}
