using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapsData
{
	public List<Map> Maps;

	public class Map
	{
		public string Name;
		public string Country;
		public string City;
		public string Effect;
	}

	public MapsData()
	{
		Maps = new List<Map>
		{
			new Map {
				Name = "RENÉ COTY STADIUM",
				Country = "FRANCE",
				City = "BISCARROSSE",
				Effect = "NONE"},
			new Map {
				Name = "X-WIND",
				Country = "UNITED KINGDOM",
				City = "BIRMINGHAM",
				Effect = "FASTER DISC"}
		};
	}
}
