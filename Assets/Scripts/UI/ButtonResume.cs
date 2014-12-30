using UnityEngine;
using System.Collections;

public class ButtonResume : MonoBehaviour
{
		//Resume game on click
		public void ClickResume ()
		{
			
				EventManager.TriggerGameResume ();
		
		}
}
