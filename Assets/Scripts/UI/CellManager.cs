using UnityEngine;
using System.Collections;
using UnityEngine.UI;


//Make a grid of Cells to form a map
public class CellManager : MonoBehaviour
{

		public int gridSize;
	
		public GameObject Cell;
		
		private GameObject o;
		
		private RectTransform i;
		
		private SetupCell c;

		// Use this for initialization
		void Start ()
		{
				//get length of grid side
				int side = (int)Mathf.Sqrt ((float)gridSize);
				
				//iterate through grid
				for (int q = -side/2; q <= side/2; q++) {		
						for (int p = -side/2; p <= side/2; p++) {
				
								//make cell
								o = (GameObject)Instantiate (Cell);
					
								//set cells parent
								o.transform.parent = transform;
					
								//get the transform so we can set postition
								i = o.GetComponent<RectTransform> ();
					
								//get the script so we can specify what part of the world the cell should represent
								c = o.GetComponent<SetupCell> ();
								
								//set the bit of the world to look at
								c.startXPos = q;
								c.startYPos = p;
								
								//set potision
								i.anchoredPosition = new Vector2 (20f * q, 20f * p);
					
						}
				}
		
		
				
	
		}
	
	
}
