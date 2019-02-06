using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapsData
{
	public static List<Map> Maps = new List<Map>
	{
		new Map {
			Name = "OPEN AIR BEACH",
			Country = "FRANCE",
			City = "BISCARROSSE",
			Effect = "NONE",
			WallCollisionX = 1.472f,
			GoalCollisionY = 1.833f},
		new Map {
			Name = "X-WIND",
			Country = "UNITED KINGDOM",
			City = "BIRMINGHAM",
			Effect = "FASTER DISC",
			WallCollisionX = 1.278f,
			GoalCollisionY = 1.917f}
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
