using UnityEngine;
using System.Collections;

public class Death : MonoBehaviour {

    private GameObject player;

	// Use this for initialization
	void Start () {

        player = GameObject.Find("Player");
	
	}
	
	// Update is called once per frame
	void Update () {

       
	
	}
    void OnTriggerEnter(Collider who)
    {

        if (who == player.collider)
        {

            EventManager.TriggerPlayerDeath();

        }

    }
}
