using UnityEngine;
using System.Collections;

public class setupField : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{
	
				transform.rotation = Quaternion.AngleAxis (Random.Range (0f, 360f), Vector3.back);
	
		}
	
	
}
