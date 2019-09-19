using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TutorialData
{
	public static List<Tutorial> Tutorials = new List<Tutorial>
	{
		new Tutorial {
			Title = "BASIC MOVEMENTS",
			Content = "MOVE WITH ARROWS"
		},
        new Tutorial {
            Title = "DASHING",
            Content = "VERTICAL DASH:\n" +
                      "DASH BUTTON"
        },
        new Tutorial {
            Title = "DASHING",
            Content = "HORIZONTAL DASH:\n" +
                      "HOLD ARROW + DASH BUTTON"
        },
        new Tutorial {
            Title = "DASHING",
            Content = "DIAGONAL DASH:\n" +
                      "DASH BUTTON THEN ARROW"
        },
        new Tutorial {
			Title = "AFTER CATCHING",
			Content = "YOU CANNOT MOVE\n" +
					  "AFTER CATCHING THE DISC"
		},
		new Tutorial {
			Title = "DISC CONTROLS",
			Content = "THROW DIRECTION:\tARROWS\n" +
					  "LIFT:\tFIRST BUTTON\n" +
					  "BASIC THROW:\tSECOND BUTTON"
		},
		new Tutorial {
			Title = "BASIC LIFT",
			Content = "CHOOSE A DIRECTION,\n" +
					  "THEN LIFT"
		},
		new Tutorial {
			Title = "ADVANCED LIFT",
			Content = "CHOOSE A DIRECTION,\n" +
					  "LIFT AND IMMEDIATELY\n" +
					  "CHOOSE THE COUNTER DIRECTION"
		},
		new Tutorial {
			Title = "QUICK THROW",
			Content = "DO A BASIC THROW JUST AFTER\n" +
					  "CATCHING THE DISC.\n" +
					  "NOT POSSIBLE AFTER A DASH"
		},
		new Tutorial {
			Title = "SUPER THROW",
			Content = "EACH CHARACTER HAS A UNIQUE\n" +
					  "SUPER THROW,\n" +
					  "AND ITS COOLDOWN"
		},
		new Tutorial {
			Title = "SUPER THROW",
			Content = "THE COOLDOWN DECREASES EACH\n" +
					  "TIME YOU CATCH THE DISC"
		},
		new Tutorial {
			Title = "SUPER THROW",
			Content = "YOU ARE IMMOBILIZED FOR\n" +
					  "ONE SECOND AFTER CASTING\n" +
					  "YOUR SUPER THROW"
		},
		new Tutorial {
			Title = "SUPER THROW",
			Content = "CATCH THE DISC DURING\n" +
					  "THIS PHASE TO DO YOUR\n" +
					  "SUPER THROW"
		},
        new Tutorial {
            Title = "SUPER THROW",
            Content = "DO A QUICK THROW AFTER\n" +
                      "CATCHING THE OPPONENT'S SUPER\n" +
                      "TO THROW IT BACK TO HIM/HER"
        },
        new Tutorial {
			Title = "GOALS",
			Content = "SCORE POINTS HITTING GOALS,\n" +
				      "RED MEANS FIVE POINTS, ANY\n" +
				      "OTHER COLOR MEANS THREE"
		},
        new Tutorial {
            Title = "PAUSE",
            Content = "YOU CAN PAUSE THE GAME\n" +
                "BY TOUCHING THE FIELD"
        },
        new Tutorial {
			Title = "THAT'S ALL FOLKS",
			Content = "GOOD LUCK"
		}
	};

	public class Tutorial
	{
		public string Title;
		public string Content;
	}
}
