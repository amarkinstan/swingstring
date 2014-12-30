using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//Set the rotation of buttons in the menu if they can't be set directly becuase of animention
public class SetupContainer : MonoBehaviour
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
