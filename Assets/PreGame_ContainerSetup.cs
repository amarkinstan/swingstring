using UnityEngine;
using System.Collections;

public class PreGame_ContainerSetup : MonoBehaviour
{

		private Quaternion angle;

		// Use this for initialization
		void Start ()
		{
	
				angle = Quaternion.AngleAxis (Random.Range (-20f, 20f), Vector3.back);
				transform.localRotation = angle;
		
	
		}
	

}
