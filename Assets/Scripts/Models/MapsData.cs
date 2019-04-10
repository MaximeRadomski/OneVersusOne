using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapsData
{
	public static List<Map> Maps = new List<Map>
	{
		new Map {
			Name = "RANDOM MAP",
			Country = "???",
			City = "???",
			Effect = "???",
			WallCollisionX = 0.0f,
			GoalCollisionY = 0.0f},
		new Map {
			Name = "OPEN AIR BEACH",
			Country = "FRANCE",
			City = "BISCARROSSE",
			Effect = "NET POLES",
			WallCollisionX = 1.472f,
			GoalCollisionY = 1.833f},
		new Map {
			Name = "X-WIND",
			Country = "UNITED KINGDOM",
			City = "MANCHESTER",
			Effect = "FASTER DISC",
			WallCollisionX = 1.278f,
			GoalCollisionY = 1.917f},
		new Map {
			Name = "EARTH WIND N RIVER",
			Country = "JAPAN",
			City = "KYOTO",
			Effect = "ZEN",
			WallCollisionX = 1.472f,
			GoalCollisionY = 1.833f}
	};

	public class Map
	{
		public string Name;
		public string Country;
		public string City;
		public string Effect;
		public float WallCollisionX;
		public float GoalCollisionY;
	}
}
