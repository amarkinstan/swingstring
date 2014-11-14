using UnityEngine;
using System.Collections;

public class SpinnerCollision : MonoBehaviour
{

		private GameObject player;
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
