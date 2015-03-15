using UnityEngine;
using System.Collections;

public class Boundary : MonoBehaviour {

    private GameObject Player;

    public bool vertical;

    public bool horizontal;

    
	// Use this for initialization
	void Start () {

        Player = GameObject.Find("Player");
	
	}
	
	// Update is called once per frame
	void Update () {

        if (vertical)
        {
            transform.position = new Vector3(Player.transform.position.x, transform.position.y, transform.position.z);
        }
        if (horizontal)
        {
            transform.position = new Vector3(transform.position.x, Player.transform.position.y, transform.position.z);
        }
	
	}
}
