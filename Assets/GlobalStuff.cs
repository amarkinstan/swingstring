using UnityEngine;
using System.Collections;

public class GlobalStuff : MonoBehaviour
{

		public static float Seed;
		public static Vector3 Gravity;
		public static Color LastColour;
		
		// Use this for initialization
		void Start ()
		{
		
				Seed = Random.value * 10000;
				Gravity = new Vector3 (0f, -6f, 0f);
	
		}
	
		public static float getDensity (float xCoord, float yCoord, float seed)
		{
		
				return Mathf.PerlinNoise (xCoord / 1000f + seed, yCoord / 1000f + seed);
		
		}
		
		public static float getSize (float xCoord, float yCoord, float seed)
		{
		
				return Mathf.PerlinNoise (xCoord / 1000f + (seed * 2f), yCoord / 1000f + (seed * 2f));
		
		}
		
}
