using UnityEngine;
using System.Collections;

//make player bounce off block with more energy than was input
public class BlockBounce : MonoBehaviour
{

		private GameObject player;
		
		//how much bounce do we need?
		public float bounceAmount;

		// Use this for initialization
		void Start ()
		{
	
				player = GameObject.Find ("Player");
		
		}
	
	
		void OnCollisionExit ()
		{
	
				player.rigidbody.AddForce ((bounceAmount) * player.transform.rigidbody.velocity, ForceMode.VelocityChange);
	
		}
}
