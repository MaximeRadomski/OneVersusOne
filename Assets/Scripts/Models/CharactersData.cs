using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersData
{
	public List<Character> Characters;

	public class Character
	{
		public string Name;
		public string Super;
	}

	public CharactersData()
	{
		Characters = new List<Character>
		{
			new Character{Name = "MARY POPPIN", Super = "GOLDEN GATE"},
			new Character{Name = "HUGUETTE BAGUETTE", Super = "OMELETTE DU FROMAGE"},
			new Character{Name = "GUNTHER VON MARMELADE", Super = "BLITZKRIEG"},
			new Character{Name = "CODY BEAVER", Super = "SORRY"},
			new Character{Name = "JOHN DUFF", Super = "SNIPER RIFLE"},
			new Character{Name = "DZAK ITSAN", Super = "OMAE WA MOU SHINDEIRU"}
		};
	}
}
