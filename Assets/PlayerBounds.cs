using UnityEngine;
using System.Collections;

public class PlayerBounds : MonoBehaviour
{

		public float xBound;
		public float yBound;

		// Use this for initialization
		void Start ()
		{
				
		}
	
		// Update is called once per frame
		void Update ()
		{
				
	
				if (transform.position.y < -yBound) {
						
						transform.position = new Vector3 (transform.position.x, yBound, 0f);
						
						
						
				}
				if (transform.position.x < -xBound) {
						transform.position = new Vector3 (xBound, transform.position.y, 0f);
	
				}
				if (transform.position.x > xBound) {
						transform.position = new Vector3 (-xBound, transform.position.y, 0f);
			
				}
		}
}
