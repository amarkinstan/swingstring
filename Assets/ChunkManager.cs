using UnityEngine;
using System.Collections;
using System.Linq;

using System.Collections.Generic;

public class ChunkManager : MonoBehaviour
{

		public GameObject basicField;
		public GameObject Player;
		public float maxDistance;
		
		private Dictionary<string, Chunk> allChunks = new Dictionary<string, Chunk> ();
		
		// Use this for initialization
		void Start ()
		{
				//make first 9 chunbks
				allChunks.Add ("0,0", new Chunk (0, 0, basicField));
				allChunks.Add ("1,0", new Chunk (1, 0, basicField));
				allChunks.Add ("0,1", new Chunk (0, 1, basicField));
				allChunks.Add ("1,1", new Chunk (1, 1, basicField));
				allChunks.Add ("0,-1", new Chunk (0, -1, basicField));
				allChunks.Add ("-1,0", new Chunk (-1, 0, basicField));
				allChunks.Add ("1,-1", new Chunk (1, -1, basicField));
				allChunks.Add ("-1,1", new Chunk (-1, 1, basicField));
				allChunks.Add ("-1,-1", new Chunk (-1, -1, basicField));
				
		
				//Chunk dingo = new Chunk (0, 0, basicField);
	
		}
		private class Chunk : Object
		{
	
				int xIndex;
				int yIndex;
				GameObject fieldType;
				public GameObject field;
				Object save;
	
				public Chunk (int xCoord, int yCoord, GameObject type)
				{
	
						this.xIndex = xCoord;
						this.yIndex = yCoord;
						this.fieldType = type;
						this.field = (GameObject)Instantiate (type, new Vector3 (IndexToWorld (xCoord), IndexToWorld (yCoord), 0f), Quaternion.identity);
						//do something to save the new chunk
						
	
				}
				
				
	
		}
		
		static float IndexToWorld (int index)
		{
			
				return  ((float)index) * 100f;
			
		}
		
		static int WorldToIndex (float world)
		{
			
				float top = Mathf.Ceil (world / 100f);
				float bottom = Mathf.Floor (world / 100f);
				
				if (Mathf.Repeat (world, 100f) > 50f) {
						return (int)top;
				} else {
						return (int)bottom;
				}
				//return (int)(world / 100f);
		}
	
		
	
		// Update is called once per frame
		void Update ()
		{
				int currentX = WorldToIndex (Player.transform.position.x);
				int currentY = WorldToIndex (Player.transform.position.y);
				for (int q = -1; q <= 1; q++) {		
						for (int p = -1; p <= 1; p++) {
								string tempX = (currentX + q).ToString ();
								string tempY = (currentY + p).ToString ();
								if (allChunks.ContainsKey (tempX + "," + tempY) == false) {
				
										allChunks.Add (tempX + "," + tempY, new Chunk (currentX + q, currentY + p, basicField));
										
				
								}
						}
				}
				List<string> list = allChunks.Select (x => x.Key).ToList ();
			
				foreach (string entry in list) {
						if (Vector3.Distance (Player.transform.position, allChunks [entry].field.transform.position) > maxDistance) {
			
								Object.Destroy (allChunks [entry].field);
								allChunks.Remove (entry);
			
						}
				}
				
				
		
		
		}
}