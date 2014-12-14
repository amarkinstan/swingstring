using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpeedBar : MonoBehaviour
{

		public GameObject player;
		private Color colour;
		public bool Bot;
		public bool Top;
		public bool Left;
		public bool Right;
		public int framesToAverage;
		
		private List<float> speeds = new List<float> ();
		
		private float average;

		// Use this for initialization
		void Start ()
		{
	
			
	
		}
	
		// Update is called once per frame
		void Update ()
		{
				
				
				
				speeds.Add (player.rigidbody.velocity.magnitude);
				if (speeds.Count > framesToAverage) {
		
						speeds.RemoveAt (0);
		
				}
				
				
				average = speeds.Average ();
				
		
				if (Bot) {
						transform.localScale = new Vector3 (Mathf.Lerp (transform.localScale.x, (average / (9.8f)), 0.5f), transform.localScale.y, 1f);
				}
				if (Top) {
						float size = (average / (9.8f)) - 3.125f;
						if (size < 0f) {
								size = 0f;
						}
						transform.localScale = new Vector3 (Mathf.Lerp (transform.localScale.x, size, 0.5f), transform.localScale.y, 1f);
				}
				if (Right) {
						float size = (average / (5.4f)) - 3.623f;
				
						if (size < 0f) {
								size = 0f;
						}
				 
						transform.localScale = new Vector3 (transform.localScale.x, Mathf.Lerp (transform.localScale.y, size, 0.5f), 1f);
				}
				if (Left) {
						float size = ((average / (5.4f)) - 9.375f) * 1.1428f;
			
						if (size < 0f) {
								size = 0f;
						}
			
						transform.localScale = new Vector3 (transform.localScale.x, Mathf.Lerp (transform.localScale.y, size, 0.5f), 1f);
				}
				
				colour = GlobalStuff.LastColour;
				colour = new Color (colour.r, colour.g, colour.b, 1f);
				colour = Color.Lerp (colour, Color.black, 0.6f);
				guiTexture.color = colour;
			
		}
}
