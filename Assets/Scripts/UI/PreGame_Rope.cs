using UnityEngine;
using System.Collections;

//Make a  rope for the pre-game menu
public class PreGame_Rope : MonoBehaviour
{

		GameObject ball;
		LineRenderer line;
		// Use this for initialization
		void Start ()
		{
	
				ball = GameObject.Find ("Ball");
				line = GetComponent<LineRenderer> ();
				ball.renderer.material.color = PreGame_ColoursToPick.shade;
				line.renderer.material.color = PreGame_ColoursToPick.shade;
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
				line.SetPosition (1, ball.transform.position);
	
	
		}
}
