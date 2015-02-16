using UnityEngine;
using System.Collections;

public class Killzone : MonoBehaviour {

    public float factor;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        float scaling = 1f + (Time.deltaTime * factor * (1f / (1f+(Time.timeSinceLevelLoad*0.1f))));

        transform.localScale = new Vector3(transform.localScale.x * scaling, 5f, transform.localScale.z * scaling);
	
	}
}
