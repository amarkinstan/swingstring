using UnityEngine;
using System.Collections;

public class SlowField : MonoBehaviour
{

		private GameObject player;
		
		//the speed at which the player will no longer be slowed
		public float maxSpeed;
		
		//slow field?
		public bool slows;
		
		//noGrav field?
		public bool noGrav;
	
		// Use this for initialization
		void Start ()
		{
		
				player = GameObject.Find ("Player");
				
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
