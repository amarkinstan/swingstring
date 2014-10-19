using UnityEngine;
using System.Collections;

public class GlobalStuff : MonoBehaviour
{

		public static float Seed;
		// Use this for initialization
		void Start ()
		{
		
				Seed = Random.value * 1000;
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
				//print (GameObject.FindObjectsOfType<GameObject> ().Length);
				//print ((Mathf.PerlinNoise (Random.value, Random.value) * 100f));
	
		}
}
