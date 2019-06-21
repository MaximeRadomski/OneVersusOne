using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsData : MonoBehaviour
{
	public static List<GameAd> GameAds = new List<GameAd>
	{
		new GameAd {
			Name = "Windjammers",
			Description = "The game that started it all!\nIf you don't know it yet, my game is inspired by Windjammers.\nCheck it out, it's an awesome game.\n(More infos in the \"About\" section)",
			UrlId = "http://www.dotemu.com/game/windjammers/",
			IsMobileGame = false},
		new GameAd {
			Name = "Teturss",
			Description = "My Tetris fan-game.\nI made it in order to have an ad-free Tetris game that I can play everywhere, and with the rules I like the most in Tetris games.\nYou even have sexy Bolsheviks coming with it!",
			UrlId = "https://abject.itch.io/teturss",
			IsMobileGame = false},
		new GameAd {
			Name = "Horizon Chase",
			Description = "You like old arcade racing games ?\nThen this one is made for you !\nEasy to learn/hard to master gameplay, Gorgeous musics and a real challenge await you!",
			UrlId = "com.aquiris.horizonchase"},
		new GameAd {
			Name = "Downwell",
			Description = "A tough roguelike with a simple yet effective gameplay.\nRequires some advanced skills to complete it.\nHas a good replayability and a fine pixel art.",
			UrlId = "com.devolver.downwell"},
		new GameAd {
			Name = "Pixel Dungeon",
			Description = "An homage to old roguelike games.\nVery difficult and a little luck-based.\nAs its creator says : \"Anyway, you will die often. You are warned!\"",
			UrlId = "com.watabou.pixeldungeon"},
		new GameAd {
			Name = "Underhand",
			Description = "A card game where you create your own cult.\nAn unique concept, an unique artistic direction, in short a real breeze of fresh air in mobile games!",
			UrlId = "edu.cornell.gdiac.underhand"},
		new GameAd {
			Name = "Hoppenhelm",
			Description = "Little and simple game with good vibes.\nArcade runner with one handed controls.\nPerfect for short runs from times to times.",
			UrlId = "com.tobiasornberg.knightgame"},
		new GameAd {
			Name = "Terra Battle",
			Description = "One of the best tactical RPG on mobile.\nThe gameplay is a nice mix between skill and strategy.\nIt has a beautiful character design and sublime artworks.",
			UrlId = "com.mistwalkercorp.guardians"}
	};

	public class GameAd
	{
		public string Name;
		public string Description;
		public string UrlId;
		public bool IsMobileGame = true;
	}
}
