using UnityEngine;
using System.Collections;

public class ButtonResume : MonoBehaviour
{

		public void ClickResume ()
		{
		
				EventManager.TriggerGameResume ();
		
		}
}
