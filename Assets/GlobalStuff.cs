using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GlobalStuff : MonoBehaviour
{

		
		public static Vector3 Gravity;
		public static Color LastColour;
		public static bool Paused;
		public static Vector3 savedVelocity;
		public Text seedText;
		
		Canvas canvas;
		GameObject player;
		TrailRenderer trail;
		
		// Use this for initialization
		void Start ()
		{		
				LastColour = Color.black;
		
				//more seed related stuff int he ButtonREstart Script
				if (GlobalStore.Seed == 0f) {
						GlobalStore.Seed = (float)((int)(Random.value * 1000000f)) / 10f;
						seedText.text = (GlobalStore.Seed * 10f).ToString ();
				} 
				
				seedText.text = GlobalStore.Seed.ToString ();
				
				Paused = false;
				
				
				Gravity = new Vector3 (0f, -6f, 0f);
				EventManager.GamePause += GamePause;
				EventManager.GameResume += GameResume;
				
				canvas = GameObject.Find ("Canvas").GetComponent<Canvas> ();	
				player = GameObject.Find ("Player");
	
		}
	
		public static float getDensity (float xCoord, float yCoord, float seed)
		{
		
				return Mathf.PerlinNoise (xCoord / 1000f + seed, yCoord / 1000f + seed);
		
		}
		
		public static float getSize (float xCoord, float yCoord, float seed)
		{
		
				return Mathf.PerlinNoise (xCoord / 1000f + (seed * 2f), yCoord / 1000f + (seed * 2f));
		
		}
		
		void GameResume ()
		{
		
				Paused = false;
		
		
				player.rigidbody.isKinematic = false;
				player.rigidbody.AddForce (GlobalStuff.savedVelocity, ForceMode.VelocityChange);
		
				TrailRenderer trail = player.GetComponent<TrailRenderer> ();
				trail.time = 10f;
		
				canvas.enabled = false;
		
		}
		
		void GamePause ()
		{
				Paused = true;
		
				GlobalStuff.savedVelocity = player.rigidbody.velocity;
				player.rigidbody.isKinematic = true;
		
		
				TrailRenderer trail = player.GetComponent<TrailRenderer> ();
				trail.time = Mathf.Infinity;
		
				canvas.enabled = true;
		}
		
}
