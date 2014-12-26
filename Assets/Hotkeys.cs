using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Hotkeys : MonoBehaviour
{
		
		
		bool ignoreNext = false;

		// Use this for initialization
		void Start ()
		{
	
				
				
				
		
		
		}
	
		void Update ()
		{
				//hotkey resume
				if (Input.GetKeyDown (KeyCode.Escape) && (GlobalStuff.Paused)) {
			
						EventManager.TriggerGameResume ();
						
						
						ignoreNext = true;
			
						
			
				}
				
				//hotkey pause
				if ((Input.GetKeyDown (KeyCode.Escape)) && (GlobalStuff.Paused != true) && (ignoreNext == false)) {
			
						EventManager.TriggerGamePause ();
						
						
						
						
			
				}
				
				ignoreNext = false;
				
				
		}
}
