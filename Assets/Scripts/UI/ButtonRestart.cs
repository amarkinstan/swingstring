using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ButtonRestart : MonoBehaviour
{

		public Text seedText;

		//Called On click
		public void ClickRestart ()
		{
				//make sure no gravity effects are happening
				Physics.gravity = GlobalStuff.Gravity;
				
				//if the seedtext is a number, set the global seed as that
				float number;
				if (float.TryParse (seedText.text, out number)) {
						GlobalStore.Seed = float.Parse (seedText.text);
			
				} else {
						GlobalStore.Seed = seedText.text.GetHashCode ();
						
						if (GlobalStore.Seed < 0f) {
								GlobalStore.Seed = -GlobalStore.Seed;
						}
				}
				
				//clear statics that need to be cleared, then load game
				EventManager.TriggerGameRestart ();
				StartCoroutine (LevelLoad ("ropebase"));
				
	
		}
		
		//wait 1 second then load level
		IEnumerator LevelLoad (string name)
		{
				
				yield return new WaitForSeconds (1f);
				
				Application.LoadLevel (name);
		}
}
