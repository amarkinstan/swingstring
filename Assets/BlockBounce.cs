using UnityEngine;
using System.Collections;

public class BlockBounce : MonoBehaviour
{

		private GameObject player;
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
