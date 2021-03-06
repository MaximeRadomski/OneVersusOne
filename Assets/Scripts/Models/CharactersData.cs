﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharactersData
{
	public static List<Character> Characters = new List<Character>
	{
		new Character{Name = "RANDOM", Skill = "???"},
		new Character{Name = "MARY POPPIN", Skill = "SCONES\nAND TEA"},
		new Character{Name = "LISE BAGUET", Skill = "OMELETTE DU FROMAGE"},
		new Character{Name = "HANS GRUBER", Skill = "PANZER-\nSCHRECK"},
		new Character{Name = "SARA BEAVER", Skill = "SORRY FOR BEING SORRY"},
		new Character{Name = "MARK FRIDOM", Skill = "SNIPER\nRIFLE"},
		new Character{Name = "DZAK ITSHAN", Skill = "OMAE WA MOU SHINDEIRU"}
	};

	public class Character
	{
		public string Name;
		public string Skill;
	}
}
