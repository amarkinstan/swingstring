using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetTextColourToBg : MonoBehaviour {

    private Text label;
    private GameObject mainCamera;

	// Use this for initialization
	void Start () {

        label = GetComponent<Text>();
        mainCamera = GameObject.Find("Main Camera");
        label.color = mainCamera.camera.backgroundColor;
	}
	

}
