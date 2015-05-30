using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.IO;


//Used to hold all the global variables and behaviours in a scene
public class GlobalStuff : MonoBehaviour
{
    //Variables that can be changed in the editor 
   
    //are when on the titel screen?
    public bool onTitle;
       
    //average speed of the player of last x frames
    public static float AveragePlayerSpeed;

    //WHere the seed is shown
    public Text seedText;

    //Where the Score is shown
    public Text scoreText;

    //Where the muli is shown
    public Text multiText;

    //staring menu color
    public Color startingColour;

    //how many frames should we get a movign average over? (more looks smoother)
    public int framesToAverage;

    //GLobal static vars

    //score multiplier
    public static float Multi;

    //Score for escape level
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
    
    //Internal vars

    //The escape menu
    private CanvasGroup menu;

    //The player
    private GameObject player;

    //player trailrendere
    private TrailRenderer trail;

    //list of player speeds for a moving average;
    private List<float> speeds = new List<float>();

    //Debug vars

    //How many times the game has stuttered since launch 
    private int BoomCOUNT = 0;

    public static string MakeSeed(int stringLength)
    {

        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new System.Random();
        var result = new string(
            Enumerable.Repeat(chars, stringLength)
                      .Select(s => s[random.Next(s.Length)])
                      .ToArray());

        return result;
    }

    public static T DeepClone<T>(T a)
    {
        using (var ms = new MemoryStream())
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            xs.Serialize(ms, a);
            ms.Position = 0;

            return (T)xs.Deserialize(ms);
        }
    }


    // Use this for initialization
    void Start()
    {
        // find the player
        player = GameObject.Find("Player");

        //set time since last hit to 0 when we start
        TimeSinceLastCollision = 0f;

        //reset score and mulktiper
        Score = 0f;
        Multi = 0f;

        //add our methods to the event manager
        EventManager.GamePause += GamePause;
        EventManager.PlayerDeath += PlayerDeath;
        EventManager.GameResume += GameResume;
        EventManager.GameRestart += GameRestart;

        //when there is no last colour set it to be what we the user said
        LastColour = startingColour;

        //seed from textbox
        seedText.text = GlobalStore.SeedText;

        //if we don't have a seed make a random one
        if (GlobalStore.Seed == 0f && GlobalStore.SeedText == "" && !onTitle)
        {
            string newSeed = MakeSeed(7);
            //GlobalStore.Seed = (float)((int)(Random.value * 1000000f)) / 10f;

            seedText.text = newSeed;

            GlobalStore.Seed = ((float)seedText.text.GetHashCode() * 10f) / 100000f;

            if (GlobalStore.Seed < 0f)
            {
                GlobalStore.Seed = -GlobalStore.Seed;
            }

            GlobalStore.SeedText = seedText.text;

            print("CHEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE" + GlobalStore.Seed);

        }


        // dont start paused
        Paused = false;

        //don't start dead
        isDead = false;

        //set inital gravity
        Gravity = new Vector3(0f, -6f, 0f);
        Physics.gravity = Gravity;

        //menu should start hidden
        if (!onTitle)
        {
            menu = GameObject.Find("Menu").GetComponent<CanvasGroup>();
            menu.alpha = 0;
            menu.interactable = false;
        }


    }

    public static void setLastColor(Color colorHit,GameObject player)
    {

        LastColour = colorHit;
        player.renderer.material.color = colorHit;
        Color trail = Color.Lerp(LastColour, Color.white, 0.5f);
        player.GetComponent<TrailRenderer>().material.SetColor("_Color", trail);

    }

    //get block denisty
    public static float getDensity(float xCoord, float yCoord, float seed)
    {

        return Mathf.PerlinNoise(xCoord / 1000f + seed, yCoord / 1000f + seed);

    }
    //get block size
    public static float getSize(float xCoord, float yCoord, float seed)
    {

        return Mathf.PerlinNoise(xCoord / 1000f + (seed * 2f), yCoord / 1000f + (seed * 2f));

    }

    //what happens when restart is pressed
    void GameRestart()
    {
        //make sure no gravity effects are happening
        Physics.gravity = Gravity;



        GlobalStore.Seed = ((float)seedText.text.GetHashCode() * 10f) / 100000f;

            if (GlobalStore.Seed < 0f)
            {
                GlobalStore.Seed = -GlobalStore.Seed;
            }

        GlobalStore.SeedText = seedText.text;
        
        print("CHEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE"+GlobalStore.Seed);
    }

    //what happens on resume
    void GameResume()
    {

        Paused = false;

        player.rigidbody.isKinematic = false;
        player.rigidbody.AddForce(GlobalStuff.savedVelocity, ForceMode.VelocityChange);

        TrailRenderer trail = player.GetComponent<TrailRenderer>();
        trail.time = 10f;

        menu.alpha = 0;
        menu.interactable = false;

    }
    //what happens on pause? We save the players velocity, set a flag, make the trail hang around forever, and turn on the menu
    void GamePause()
    {
        Paused = true;

        GlobalStuff.savedVelocity = player.rigidbody.velocity;
        player.rigidbody.isKinematic = true;


        TrailRenderer trail = player.GetComponent<TrailRenderer>();
        trail.time = Mathf.Infinity;

        menu.alpha = 1;
        menu.interactable = true;
    }

    //what happens when the player dies?
    void PlayerDeath()
    {

        isDead = true;

        player.rigidbody.isKinematic = true;

        TrailRenderer trail = player.GetComponent<TrailRenderer>();
        trail.time = Mathf.Infinity;

    }

    void Update()
    {
        //if the game is runnign
        if (Paused == false && isDead == false && !onTitle)
        {
            //record player speed for moving average
            speeds.Add(player.rigidbody.velocity.magnitude);

            //increment the time sine last collision
            TimeSinceLastCollision += Time.deltaTime;

            //remove the speed at the end if the list is full
            if (speeds.Count > framesToAverage)
            {
                speeds.RemoveAt(0);
            }

            //get the current average speed
            AveragePlayerSpeed = speeds.Average();

            //set the score UI element with the correct string format
            scoreText.text = Score.ToString("0");

            //set the multiplier UI element with the correct string format
            multiText.text = "× " + Multi.ToString("0.00");
        }

        // find stutters
        if (Time.unscaledDeltaTime > 0.02f)
        {
            BoomCOUNT++;
            print("BOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOM " + BoomCOUNT);

        }

    }
}
