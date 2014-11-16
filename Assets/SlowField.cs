using UnityEngine;
using System.Collections;

public class SlowField : MonoBehaviour
{

		private GameObject player;
		private Vector3 savedGravity;
		public float maxSpeed;
		public bool slows;
		public bool noGrav;
	
		// Use this for initialization
		void Start ()
		{
		
				player = GameObject.Find ("Player");
				savedGravity = Physics.gravity;
		}
	
	
		void OnTriggerStay (Collider who)
		{
				if (who == player.collider && slows) {
						if (player.rigidbody.velocity.magnitude > maxSpeed) {
								player.rigidbody.velocity = player.rigidbody.velocity.normalized * (player.rigidbody.velocity.magnitude * 0.97f);
						}
				}
				if (who == player.collider && noGrav) {
			
						Physics.gravity = Vector3.zero;
			
				}
		
		}
		void OnTriggerEnter (Collider who)
		{
		
				if (who == player.collider && noGrav) {
						
						Physics.gravity = Vector3.zero;
			
				}
		
		}
		void OnTriggerExit (Collider who)
		{
				if (who == player.collider && noGrav) {
						
						Physics.gravity = GlobalStuff.Gravity;
			
				}
		
		}
}
