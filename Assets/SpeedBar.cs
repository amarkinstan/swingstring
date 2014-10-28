using UnityEngine;
using System.Collections;

public class SpeedBar : MonoBehaviour
{

		public GameObject player;
		private Color colour;

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
		
				transform.localScale = new Vector3 (Mathf.Lerp (transform.localScale.x, (player.rigidbody.velocity.magnitude / 60f), 0.5f), transform.localScale.y, 1f);
				colour = player.renderer.material.color;
				colour = new Color (colour.r, colour.g, colour.b, 1f);
				colour = Color.Lerp (colour, Color.black, 0.6f);
				guiTexture.color = colour;
			
		}
}
