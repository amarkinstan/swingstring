using UnityEngine;
using System.Collections;

public class SlowField : MonoBehaviour
{

		private GameObject player;
		public Vector3 savedGravity;
		public float maxSpeed;
		public bool slows;
	
		// Use this for initialization
		void Start ()
		{
		
				player = GameObject.Find ("Player");
		
		}
	
	
		void OnTriggerStay (Collider who)
		{
				if (who == player.collider && slows) {
						player.rigidbody.velocity = player.rigidbody.velocity.normalized * maxSpeed;
				}
		
		}
		void OnTriggerEnter (Collider who)
		{
		
				if (who == player.collider) {
						
						Physics.gravity = Vector3.zero;
			
				}
		
		}
		void OnTriggerExit (Collider who)
		{
				if (who == player.collider) {
						
						Physics.gravity = savedGravity;
			
				}
		
		}
}
