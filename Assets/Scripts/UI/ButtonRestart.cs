using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ButtonRestart : MonoBehaviour
{
        public string LevelName;
		public Text seedText;

		//Called On click
		public void ClickRestart ()
		{
				
				
				//clear statics that need to be cleared, then load game
				EventManager.TriggerGameRestart ();
				StartCoroutine (LevelLoad (LevelName));
				
	
		}
		
		//wait 1 second then load level
		IEnumerator LevelLoad (string name)
		{
				
				yield return new WaitForSeconds (1f);
				
				Application.LoadLevel (name);
		}
}
