using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ButtonRestart : MonoBehaviour
{

		public Text seedText;

		public void ClickRestart ()
		{
				Physics.gravity = GlobalStuff.Gravity;
				EventManager.TriggerGameRestart ();
				float number;
				if (float.TryParse (seedText.text, out number)) {
						GlobalStore.Seed = float.Parse (seedText.text);
			
				} else {
						GlobalStore.Seed = seedText.text.GetHashCode ();
						
						if (GlobalStore.Seed < 0f) {
								GlobalStore.Seed = -GlobalStore.Seed;
						}
				}
					
				Application.LoadLevel ("ropebase"); 
	
		}
}
