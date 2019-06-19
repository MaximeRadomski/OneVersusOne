using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsData : MonoBehaviour
{
	public static List<GameAd> GameAds = new List<GameAd>
	{
		new GameAd {
			Name = "Horizon Chase",
			Description = "You like old arcade racing games ?\nThen this one is made for you !\nEasy to learn/hard to master gameplay, Gorgeous musics and a real challenge await you with horizon chase",
			UrlId = "com.aquiris.horizonchase"},
		new GameAd {
			Name = "Downwell",
			Description = "A tough roguelike with a simple yet effective gameplay.\nRequires some advanced skills to complete it.\nHas a good replayability and a fine pixel art.",
			UrlId = "com.devolver.downwell"},
		new GameAd {
			Name = "Pixel Dungeon",
			Description = "An homage to old roguelike games.\nVery difficult and a little luck-based.\nAs its creator says : \"Anyway, you will die often. You are warned!\"",
			UrlId = "com.watabou.pixeldungeon"}
	};

	public class GameAd
	{
		public string Name;
		public string Description;
		public string UrlId;
	}
}
