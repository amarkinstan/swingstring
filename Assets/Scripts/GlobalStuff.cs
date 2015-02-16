using UnityEngine;
using System.Collections;
using UnityEngine.UI;


//Used to hold all the global variables and behaviours in a scene
public class GlobalStuff : MonoBehaviour
{

		//what gravity normally is
		public static Vector3 Gravity;
		
		//the colour of th elast bloack attachetd to.
		public static Color LastColour;
		
		//Is the game pasued
		public static bool Paused;
		
		//The valocity of the player at pause
		public static Vector3 savedVelocity;
		
		//WHere the seed is shown
		public Text seedText;
		
		CanvasGroup menu;
		GameObject player;
		TrailRenderer trail;

        private int BoomCOUNT = 0;
		
		// Use this for initialization
		void Start ()
		{		
				EventManager.GamePause += GamePause;
				EventManager.GameResume += GameResume;
				
				//when there is no last colour set it to be black
				LastColour = Color.black;
		
				//more seed related stuff int he ButtonREstart Script
				seedText.text = GlobalStore.Seed.ToString ();
				
				//if we don't have a seed make a random one
				if (GlobalStore.Seed == 0f) {
						GlobalStore.Seed = (float)((int)(Random.value * 1000000f)) / 10f;
						
						seedText.text = GlobalStore.Seed.ToString ();
						
				} 
				
				
				// dont start paused
				Paused = false;
				
				//set inital gravity
				Gravity = new Vector3 (0f, -6f, 0f);
				Physics.gravity = Gravity;
				
				//menu shoudl start hidden
				menu = GameObject.Find ("Menu").GetComponent<CanvasGroup> ();
				menu.alpha = 0;
				menu.interactable = false;
				player = GameObject.Find ("Player");
	
		}
	
		//get block denisty
		public static float getDensity (float xCoord, float yCoord, float seed)
		{
		
				return Mathf.PerlinNoise (xCoord / 1000f + seed, yCoord / 1000f + seed);
		
		}
		//get block size
		public static float getSize (float xCoord, float yCoord, float seed)
		{
		
				return Mathf.PerlinNoise (xCoord / 1000f + (seed * 2f), yCoord / 1000f + (seed * 2f));
		
		}
		
		//what happens on resume
		void GameResume ()
		{
		
				Paused = false;
		
		
				player.rigidbody.isKinematic = false;
				player.rigidbody.AddForce (GlobalStuff.savedVelocity, ForceMode.VelocityChange);
		
				TrailRenderer trail = player.GetComponent<TrailRenderer> ();
				trail.time = 10f;
		
				menu.alpha = 0;
				menu.interactable = false;
		
		}
		//what happens on pause? We save the players velocity, set a flag, make the trail hang around forever, and turn on the menu
		void GamePause ()
		{
				Paused = true;
		
				GlobalStuff.savedVelocity = player.rigidbody.velocity;
				player.rigidbody.isKinematic = true;
		
		
				TrailRenderer trail = player.GetComponent<TrailRenderer> ();
				trail.time = Mathf.Infinity;
		
				menu.alpha = 1;
				menu.interactable = true;
		}
		
        void Update ()
        {


            if (Time.unscaledDeltaTime > 0.017f)
            {
                BoomCOUNT++;
                print("BOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOM "+BoomCOUNT);

            }

        }
}
