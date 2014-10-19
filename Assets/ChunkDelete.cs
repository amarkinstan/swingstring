using UnityEngine;
using System.Collections;

public class ChunkDelete : MonoBehaviour
{

		private Transform player;
		public float maxDistance;

		//If this chunk is more than X distance from player, delete it
	
		// Use this for initialization
		void Start ()
		{
				player = GameObject.Find ("Player").transform;
		}
	
		// Update is called once per frame
		void Update ()
		{
	
				if (Vector3.Distance (player.position, transform.position) > maxDistance) {
		
						Object.Destroy (gameObject);
		
				}
	
		}
}
