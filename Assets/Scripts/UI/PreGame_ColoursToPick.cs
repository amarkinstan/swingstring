using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//define a set of colours that the pregame menu can be 
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
				
				PreGame_ColoursToPick.shade = colorSet [Random.Range (0, 4)];
								
				print (colorSet [0] + " " + colorSet [1] + " " + colorSet [2] + " " + colorSet [3]);
	
		}
	
	
	
}
