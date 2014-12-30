using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//Set the colour and rotation of buttons in the pregame menu
public class PreGame_ButtonSetup : MonoBehaviour
{

		//Flag to set if the colour is more white than black
		public bool isLight;
		private Color tint;
		private Quaternion angle;
		private Image picture;
		

		// Use this for initialization
		void Start ()
		{
	
				
				
				picture = GetComponent<Image> ();
				RectTransform thisRect = GetComponent<RectTransform> ();
		
				tint = PreGame_ColoursToPick.shade;
				
		
		
		
				if (isLight) {
			
						picture.color = Color.Lerp (tint, Color.white, 0.3f);
						picture.color = new Color (picture.color.r, picture.color.g, picture.color.b, 1f);
			
				} else {
			
						picture.color = Color.Lerp (tint, Color.black, 0.3f);
						picture.color = new Color (picture.color.r, picture.color.g, picture.color.b, 1f);
				}
				angle = Quaternion.AngleAxis (Random.Range (-20f, 20f), Vector3.back);
				transform.localRotation = angle;
	
		}
	

}
