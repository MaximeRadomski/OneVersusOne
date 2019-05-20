using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PunchlinesData
{
	public static List<List<string>> Punchlines = new List<List<string>>
	{
		new List<string> (), new List<string> (), new List<string> (), new List<string> (), new List<string> (), new List<string> ()
	};

	public static List<List<string>> Intros = new List<List<string>>
	{
		new List<string> (), new List<string> (), new List<string> (), new List<string> (), new List<string> (), new List<string> ()
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
		Punchlines [0].AddRange (new string[] {"BYE DEAR", "U WOT M8", "IN YOUR TEETH"});
		Intros [0].AddRange (new string[] {"BRITISH", "SCONES"});
	}

	private static void PopulateFR()
	{
		Punchlines [1].AddRange (new string[] {"SACREBLEU", "HON HON HON", "BON APPETIT"});
		Intros [1].AddRange (new string[] {"FRENCH", "BAGUETTE"});
	}

	private static void PopulateDE()
	{
		Punchlines [2].AddRange (new string[] {"JAWOHL", "UNBEGRENZTE MACHT", "DAS BEER BOOT"});
		Intros [2].AddRange (new string[] {"GERMAN", "SAUSAGE"});
	}

	private static void PopulateCA()
	{
		Punchlines [3].AddRange (new string[] {"THANKS MATE", "BE POLITE. BE EFFICIENT", "STAY COOL"});
		Intros [3].AddRange (new string[] {"CANADIAN", "BEAVER"});
	}

	private static void PopulateUS()
	{
		Punchlines [4].AddRange (new string[] {"USA! USA! USA!", "TAKE SOME FREEDOM", "BOOM HEADSHOT"});
		Intros [4].AddRange (new string[] {"AMERICAN", "BURGER"});
	}

	private static void PopulateJA()
	{
		Punchlines [5].AddRange (new string[] {"MUDADA", "YARE YARE DAZE", "RYU GA WAGA TEKI WO KURAU"});
		Intros [5].AddRange (new string[] {"JAPANESE", "SUSHIS"});
	}
}
