using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Opponent
{
	Player = 0,
	AI = 1,
	Wall = 2,
	Target = 3,
	Catch = 4,
	Breakout = 5
}

public enum Difficulty
{
	Easy = 0,
	Normal = 1,
	Hard = 2
}

public enum Bounce
{
	Normal = 0,
	Random = 1
}

public enum GameMode
{
	Duel = 0,
	Tournament = 1,
	Target = 2,
	Catch = 3,
	Breakout = 4
}