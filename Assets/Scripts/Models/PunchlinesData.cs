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
		new List<string> (), new List<string> (), new List<string> (), new List<string> (), new List<string> (), new List<string> (), new List<string> ()
	};

	static PunchlinesData()
	{
		PopulateRandom ();
		PopulateUK ();
		PopulateFR ();
		PopulateDE ();
		PopulateCA ();
		PopulateUS ();
		PopulateJA ();
	}

	private static void PopulateRandom()
	{
		Intros [0].AddRange (new string[] {"WOW", "MUCH RISK", "SUCH SUSPENSE", "VERY BRAVE"});
	}

	private static void PopulateUK()
	{
		Punchlines [0].AddRange (new string[] {"BYE DEAR", "U WOT M8", "IN YOUR TEETH"});
		Intros [1].AddRange (new string[] {"FOR THE QUEEN", "HOLD MY BEER", "A CUP OF TEA"});
	}

	private static void PopulateFR()
	{
		Punchlines [1].AddRange (new string[] {"SACREBLEU", "HON HON HON", "BON APPETIT"});
		Intros [2].AddRange (new string[] {"C'EST PARTI", "MILLE SABORDS", "EN VOITURE SIMONE"});
	}

	private static void PopulateDE()
	{
		Punchlines [2].AddRange (new string[] {"JAWOHL", "UNBEGRENZTE MACHT", "DAS BEER BOOT"});
		Intros [3].AddRange (new string[] {"ICH WILL", "DU HAST MICH GEFRAGT", "WIR HOREN EUCH"});
	}

	private static void PopulateCA()
	{
		Punchlines [3].AddRange (new string[] {"THANKS MATE", "BE POLITE. BE EFFICIENT", "STAY COOL"});
		Intros [4].AddRange (new string[] {"PARDON MY FRENCH", "SORRY I'LL WIN", "FIGHTING ALLOWED"});
	}

	private static void PopulateUS()
	{
		Punchlines [4].AddRange (new string[] {"USA! USA! USA!", "TAKE SOME FREEDOM", "BOOM HEADSHOT"});
		Intros [5].AddRange (new string[] {"LET'S DO THIS", "READY FOR DUTY", "CHECK THOSE GUNS"});
	}

	private static void PopulateJA()
	{
		Punchlines [5].AddRange (new string[] {"MUDADA", "YARE YARE DAZE", "RYU GA WAGA TEKI WO KURAU"});
		Intros [6].AddRange (new string[] {"NANI?", "YAMERO BAKA", "NARUHODO"});
	}
}
