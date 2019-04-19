using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TutorialData
{
	public static List<Tutorial> Tutorials = new List<Tutorial>
	{
		new Tutorial {
			Title = "BASIC MOVEMENTS",
			Content = "MOVE:\tARROWS\nDASH:\tARROWS + DASH"
		},
		new Tutorial {
			Title = "AFTER CATCHING",
			Content = "YOU CANNOT MOVE AFTER CATCHING THE DISC"
		}
	};

	public class Tutorial
	{
		public string Title;
		public string Content;
	}
}
