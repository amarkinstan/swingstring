using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;


//Used to hold all the global variables and behaviours in a scene
public class GlobalStuff : MonoBehaviour
{
            

        //score multiplier
        public static float Multi;

        //Score
        public static float Score;

        //time since the pl;ayer last ran into something
        public static float TimeSinceLastCollision;
    
        //Where a player spawns
        public GameObject RespawnPoint;

        //speed of darkness
        public static float DarkSpeed;

		//what gravity normally is
		public static Vector3 Gravity;
		
		//the colour of th elast bloack attachetd to.
		public static Color LastColour;
		
		//Is the game pasued
		public static bool Paused;

        //Is the player Dead
        public static bool isDead;
		
		//The valocity of the player at pause
		public static Vector3 savedVelocity;
		
		//WHere the seed is shown
		public Text seedText;

        //WHere the Score is shown
        public Text scoreText;

        //WHere the muli is shown
        public Text multiText;

        //staring menu color
        public Color startingColour;

        //how many frames should we get a movign average over? (more looks smoother)
        public int framesToAverage;
		
		CanvasGroup menu;
		GameObject player;
		TrailRenderer trail;

        private int BoomCOUNT = 0;

        //average speed of the player of last x frames
        public static float AveragePlayerSpeed;


        //list of player speeds for a moving average;
        private List<float> speeds = new List<float>();
		
		// Use this for initialization
		void Start ()
		{

                player = GameObject.Find("Player");

                TimeSinceLastCollision = 0f;
                
                EventManager.GamePause += GamePause;
                EventManager.PlayerDeath += PlayerDeath;
				EventManager.GameResume += GameResume;
				
				//when there is no last colour set it to be black
				LastColour = startingColour;
		
				//more seed related stuff int he ButtonREstart Script
				seedText.text = GlobalStore.Seed.ToString ();
				
				//if we don't have a seed make a random one
				if (GlobalStore.Seed == 0f) {
						GlobalStore.Seed = (float)((int)(Random.value * 1000000f)) / 10f;
						
						seedText.text = GlobalStore.Seed.ToString ();
						
				} 
				
				
				// dont start paused
				Paused = false;

                //don't start dead
                isDead = false;
				
				//set inital gravity
				Gravity = new Vector3 (0f, -6f, 0f);
				Physics.gravity = Gravity;
				
				//menu shoudl start hidden
				menu = GameObject.Find ("Menu").GetComponent<CanvasGroup> ();
				menu.alpha = 0;
				menu.interactable = false;
				
	
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

        void PlayerDeath()
        {

            isDead = true;

                player.rigidbody.isKinematic = true;

                TrailRenderer trail = player.GetComponent<TrailRenderer>();
                trail.time = Mathf.Infinity;

        }
		
        void Update ()
        {
            if (Paused == false && isDead == false)
            {
                speeds.Add(player.rigidbody.velocity.magnitude);

                TimeSinceLastCollision += Time.deltaTime;

                //remove the speed at teh end if the list is full
                if (speeds.Count > framesToAverage)
                {

                    speeds.RemoveAt(0);

                }

                AveragePlayerSpeed = speeds.Average();

                scoreText.text = Score.ToString("0");

                multiText.text = "× " + Multi.ToString("0.00");
            }

            if (Time.unscaledDeltaTime > 0.017f)
            {
                BoomCOUNT++;
                print("BOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOM "+BoomCOUNT);

            }

        }
}
