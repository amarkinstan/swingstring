using UnityEngine;
using System.Collections;

public class GlobalStuff : MonoBehaviour
{

		public static float Seed;
		public static Vector3 Gravity;
		// Use this for initialization
		void Start ()
		{
		
				Seed = Random.value * 10000;
				Gravity = new Vector3 (0f, -6f, 0f);
	
		}
	
		// Update is called once per frame
		
}
