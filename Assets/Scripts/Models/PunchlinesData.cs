using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PunchlinesData
{
	public static List<List<string>> Punchlines = new List<List<string>>
	{
		new List<string> (),
		new List<string> (),
		new List<string> (),
		new List<string> (),
		new List<string> (),
		new List<string> ()
	};

	static PunchlinesData()
	{
		PopulateUK ();
		PopulateFR ();
		PopulateDE ();
		PopulateCA ();
		PopulateUS ();
		PopulateJA ();
	}

	private static void PopulateUK()
	{
		Punchlines [0].Add ("BYE LOVE");
	}

	private static void PopulateFR()
	{
		Punchlines [1].Add ("SACREBLEU");
	}

	private static void PopulateDE()
	{
		Punchlines [2].Add ("JAWOHL");
	}

	private static void PopulateCA()
	{
		Punchlines [3].Add ("THANKS MATE");
	}

	private static void PopulateUS()
	{
		Punchlines [4].Add ("F*CK YEAH");
	}

	private static void PopulateJA()
	{
		Punchlines [5].Add ("MUDADA");
	}
}
