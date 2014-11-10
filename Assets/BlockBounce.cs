using UnityEngine;
using System.Collections;

public class BlockBounce : MonoBehaviour
{

		public Transform player;
		public float bounceAmount;

		// Use this for initialization
		void Start ()
		{
	
		}
	
	
		void OnCollisionExit ()
		{
	
				player.rigidbody.AddForce ((bounceAmount) * player.rigidbody.velocity, ForceMode.VelocityChange);
	
		}
}
