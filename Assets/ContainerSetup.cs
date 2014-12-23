using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ContainerSetup : MonoBehaviour
{

		private Quaternion angle;

		// Use this for initialization
		void OnEnable ()
		{
	
				angle = Quaternion.AngleAxis ((Random.value * 2f * 30f) - 30f, Vector3.back);
				transform.localRotation = angle;
	
		}
	

}
