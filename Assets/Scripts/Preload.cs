using UnityEngine;
using System.Collections;

public class Preload : MonoBehaviour {

   

	// Use this for initialization
	void Start () {

        StartCoroutine(Wait(0.02f));


	
	}

    IEnumerator Wait(float amount)
    {
        
        yield return new WaitForSeconds(amount);
        gameObject.SetActive(false);
        
    }
	
	
}
