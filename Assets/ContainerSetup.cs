using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ContainerSetup : MonoBehaviour
{

		private Quaternion angle;
		
		void Start ()
		{
		
				EventManager.GamePause += GamePause;
		}

		// Use this for initialization
		void GamePause ()
		{
	
				angle = Quaternion.AngleAxis (Random.Range (-20f, 20f), Vector3.back);
				transform.localRotation = angle;
	
		}
	

}
