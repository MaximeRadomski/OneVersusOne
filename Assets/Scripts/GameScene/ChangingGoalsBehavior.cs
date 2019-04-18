using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingGoalsBehavior : MonoBehaviour
{
	public Sprite Bot3, Bot5;
	public Sprite Top3, Top5;

	private Sprite _looserSprite, _winnerSprite;

	public void SomeoneScored(CurrentPlayer looser)
	{
		string looserSide, winnerSide;
		if (looser == CurrentPlayer.PlayerOne) {
			looserSide = "Bot";
			winnerSide = "Top";
			_looserSprite = Bot3;
			_winnerSprite = Top5;
		} else {
			looserSide = "Top";
			winnerSide = "Bot";
			_looserSprite = Top3;
			_winnerSprite = Bot5;
		}
		ChangeLooser (looserSide);
		ChangeWinner (winnerSide);
	}

	private void ChangeLooser(string side)
	{
		GameObject.Find ("Goal" + side + "01").GetComponent<GoalBehavior> ().Points = 3;
		GameObject.Find ("Goal" + side + "01").GetComponent<GoalBehavior> ().NormalStateSprite = _looserSprite;
		GameObject.Find ("Goal" + side + "01").GetComponent<GoalBehavior> ().Actualize ();
		GameObject.Find ("Goal" + side + "02").GetComponent<GoalBehavior> ().Points = 3;
		GameObject.Find ("Goal" + side + "02").GetComponent<GoalBehavior> ().NormalStateSprite = _looserSprite;
		GameObject.Find ("Goal" + side + "02").GetComponent<GoalBehavior> ().Actualize ();
	}

	private void ChangeWinner(string side)
	{
		int goalId;
		if (GameObject.Find ("Goal" + side + "01").GetComponent<GoalBehavior> ().Points == 3 &&
			GameObject.Find ("Goal" + side + "02").GetComponent<GoalBehavior> ().Points == 3)
			goalId = Random.Range (1, 3);
		else if (GameObject.Find ("Goal" + side + "01").GetComponent<GoalBehavior> ().Points == 5)
			goalId = 2;
		else
			goalId = 1;

		GameObject.Find ("Goal" + side + goalId.ToString("D2")).GetComponent<GoalBehavior> ().Points = 5;
		GameObject.Find ("Goal" + side + goalId.ToString("D2")).GetComponent<GoalBehavior> ().NormalStateSprite = _winnerSprite;
		GameObject.Find ("Goal" + side + goalId.ToString ("D2")).GetComponent<GoalBehavior> ().Actualize ();
	}
}
