using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {

    private Text text;
    private Color textColor;

    //colour to lerp to
    public Color shade;

    //how much to lerp - 0 disables
    public float lerpAmount;

	// Use this for initialization
	void Start () {

        text = GetComponent<Text>();
	
	}
	
	// Update is called once per frame
	void Update () {

        

        textColor = new Color(GlobalStuff.LastColour.r, GlobalStuff.LastColour.g, GlobalStuff.LastColour.b, 1.0f);

        text.color = Color.Lerp(textColor, shade, lerpAmount);
	
	}
}
