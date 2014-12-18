using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleSetup : MonoBehaviour
{
		
		// Use this for initialization
		void Start ()
		{
				Image picture = GetComponent<Image> ();
				transform.localRotation = Quaternion.AngleAxis ((Random.value * 2f * 30f) - 30f, Vector3.back);
				picture.color = new Color (Random.value, Random.value, Random.value);
				
	
		}
	
		
}
