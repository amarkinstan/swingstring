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
				
				float number;
				if (float.TryParse (seedText.text, out number)) {
						GlobalStore.Seed = float.Parse (seedText.text);
			
				} else {
						GlobalStore.Seed = seedText.text.GetHashCode ();
						
						if (GlobalStore.Seed < 0f) {
								GlobalStore.Seed = -GlobalStore.Seed;
						}
				}
				print ("Ping1");	
				EventManager.TriggerGameRestart ();
				StartCoroutine (LevelLoad ("ropebase"));
				
	
		}
		
		IEnumerator LevelLoad (string name)
		{
				print ("Ping2");
				yield return new WaitForSeconds (1f);
				print ("Ping3");
				Application.LoadLevel (name);
		}
}
