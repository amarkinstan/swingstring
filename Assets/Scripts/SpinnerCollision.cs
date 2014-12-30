using UnityEngine;
using System.Collections;

//If the player collides withthe spinner stop them fast.
public class SpinnerCollision : MonoBehaviour
{

		private GameObject player;
		
		//how much to slow by
		public float slowAmount;
	
		// Use this for initialization
		void Start ()
		{
		
				player = GameObject.Find ("Player");
		
		}
	
	
		void OnCollisionEnter ()
		{
		
				player.rigidbody.AddForce (-(slowAmount) * player.rigidbody.velocity, ForceMode.VelocityChange);
		
		}
}
