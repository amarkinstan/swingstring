using UnityEngine;
using System.Collections;

public class SpeedBar : MonoBehaviour
{

		public GameObject player;
		private Color colour;
		public bool Bot;
		public bool Top;
		public bool Left;
		public bool Right;

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
		
				if (Bot) {
						transform.localScale = new Vector3 (Mathf.Lerp (transform.localScale.x, (player.rigidbody.velocity.magnitude / (9.8f)), 0.5f), transform.localScale.y, 1f);
				}
				if (Top) {
						float size = (player.rigidbody.velocity.magnitude / (9.8f)) - 3.125f;
						if (size < 0f) {
								size = 0f;
						}
						transform.localScale = new Vector3 (Mathf.Lerp (transform.localScale.x, size, 0.5f), transform.localScale.y, 1f);
				}
				if (Right) {
						float size = (player.rigidbody.velocity.magnitude / (5.4f)) - 3.623f;
				
						if (size < 0f) {
								size = 0f;
						}
				 
						transform.localScale = new Vector3 (transform.localScale.x, Mathf.Lerp (transform.localScale.y, size, 0.5f), 1f);
				}
				if (Left) {
						float size = ((player.rigidbody.velocity.magnitude / (5.4f)) - 9.375f) * 1.1428f;
			
						if (size < 0f) {
								size = 0f;
						}
			
						transform.localScale = new Vector3 (transform.localScale.x, Mathf.Lerp (transform.localScale.y, size, 0.5f), 1f);
				}
				
				colour = player.renderer.material.color;
				colour = new Color (colour.r, colour.g, colour.b, 1f);
				colour = Color.Lerp (colour, Color.black, 0.6f);
				guiTexture.color = colour;
			
		}
}
