using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {

    private GameObject Player;

	// Use this for initialization
	void Start () {

        Player = GameObject.Find("Player");
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = new Vector3(Player.transform.position.x, transform.position.y, transform.position.z);
	
	}
}
