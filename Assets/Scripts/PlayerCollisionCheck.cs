using UnityEngine;
using System.Collections;

public class PlayerCollisionCheck : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


	
	}

    void OnCollisionEnter(Collision who)
    {

        if (who.collider.isTrigger == false && who.transform.tag != "NoScoreReset")
        {

            GlobalStuff.TimeSinceLastCollision = 0f;

        }

    }
}
