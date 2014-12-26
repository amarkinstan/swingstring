using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
	
				int side = (int)Mathf.Sqrt ((float)gridSize);
				
				for (int q = -side/2; q <= side/2; q++) {		
						for (int p = -side/2; p <= side/2; p++) {
				
								o = (GameObject)Instantiate (Cell);
					
								o.transform.parent = transform;
					
								i = o.GetComponent<RectTransform> ();
					
								c = o.GetComponent<SetupCell> ();
								
								c.startXPos = q;
								
								c.startYPos = p;
								
								i.anchoredPosition = new Vector2 (20f * q, 20f * p);
					
						}
				}
		
		
				
	
		}
	
	
}
