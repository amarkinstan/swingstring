using UnityEngine;
using System.Collections;

public class ScoreCalculator : MonoBehaviour {

    GameObject player;
    float score;
    float multi;

    public float factor;


	// Use this for initialization
	void Start () {

        player = GameObject.Find("Player");
	
	}
	
	// Update is called once per frame
	void Update () {

        if (GlobalStuff.Paused == false && GlobalStuff.isDead == false)
        {

            score = GlobalStuff.Score;


            //multiplier is the average speed combined with the time since last block touched
            //(time in seconds since last block touch)/10f * (Globalstuff.averagePlayerSpeed * factor)
            multi = Mathf.Clamp(GlobalStuff.TimeSinceLastCollision / 10f, 0f, 1f) * (GlobalStuff.AveragePlayerSpeed * factor);

            GlobalStuff.Multi = multi;

            score += multi * Time.deltaTime * 10f;

            GlobalStuff.Score = score;
        }
	
	}
}
