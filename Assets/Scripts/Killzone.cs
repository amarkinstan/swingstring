using UnityEngine;
using System.Collections;

public class Killzone : MonoBehaviour {

    public float factor;

    public float acceleration;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        GlobalStuff.DarkSpeed = 0f;

        if (GlobalStuff.isDead == false)
        {

            float scaling = Time.deltaTime * factor;

            scaling = scaling + (acceleration * Time.timeSinceLevelLoad * Time.deltaTime);

            if (scaling * (1 / Time.deltaTime) > 61f)
            {

                scaling = 61f / (1 / Time.deltaTime);

            }

            GlobalStuff.DarkSpeed = scaling * (1 / Time.deltaTime);

            //print("increase per second =" + GlobalStuff.DarkSpeed);

            transform.localScale = new Vector3(transform.localScale.x + scaling, 10f, transform.localScale.z + scaling);
        }

        
	
	}
}
