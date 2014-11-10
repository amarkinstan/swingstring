using UnityEngine;
using System.Collections;

public class GlobalStuff : MonoBehaviour
{

		public static float Seed;
		// Use this for initialization
		void Start ()
		{
		
				Seed = Random.value * 10000;
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
				//print (GameObject.FindObjectsOfType<GameObject> ().Length);
				//print ((Mathf.PerlinNoise (Random.Range (0f, 10f), Random.Range (0f, 10f)) * 100f));
	
		}
}
