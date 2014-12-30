using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//Set the colour and rotation of buttons in the menu. The colour is based on last touched block
public class SetupButton : MonoBehaviour
{
		
		public bool isLight;
		private Color tint;
		private Quaternion angle;
		private Image picture;
		
		void Start ()
		{		
		
				EventManager.GamePause += GamePause;
				//EventManagers.GameResume += GameResume;
		
		}
				
		// Use this for initialization
		void GamePause ()
		{
				picture = GetComponent<Image> ();
				RectTransform thisRect = GetComponent<RectTransform> ();
		
				tint = GlobalStuff.LastColour;
				
				
						
				
				if (isLight) {
				
						picture.color = Color.Lerp (tint, Color.white, 0.3f);
						picture.color = new Color (picture.color.r, picture.color.g, picture.color.b, 1f);
				
				} else {
				
						picture.color = Color.Lerp (tint, Color.black, 0.3f);
						picture.color = new Color (picture.color.r, picture.color.g, picture.color.b, 1f);
				}
				
				
	
		}
	
		
}
