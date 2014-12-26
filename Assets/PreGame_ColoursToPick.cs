using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PreGame_ColoursToPick : MonoBehaviour
{

		public Color c1;
		public Color c2;
		public Color c3;
		public Color c4;
		private List<Color> colorSet;
		public static Color shade;
		

		// Use this for initialization
		void Awake ()
		{
	
				colorSet = new List<Color> ();
				
				colorSet.Add (c1);
				colorSet.Add (c2);
				colorSet.Add (c3);
				colorSet.Add (c4);
				
				PreGame_ColoursToPick.shade = colorSet [Random.Range (0, 3)];
				
				
	
	
		}
	
	
	
}
