using UnityEngine;
using System.Collections;
using System.Linq;

using System.Collections.Generic;

public class ChunkManager : MonoBehaviour
{

	
		public GameObject sparseSmall;
		public GameObject sparseBig;
		public GameObject denseSmall;
		public GameObject denseBig;
		public GameObject Player;
		public float maxDistance;
		
		private float seed;
		
		private Dictionary<string, Chunk> allChunks = new Dictionary<string, Chunk> ();
		
		// Use this for initialization
		void Start ()
		{
				//get seed from global seed
				seed = GlobalStuff.Seed;
				
				//make first 9 chunbks
				allChunks.Add ("0,0", new Chunk (0, 0, seed, sparseSmall, sparseBig, denseSmall, denseBig));
				allChunks.Add ("1,0", new Chunk (1, 0, seed, sparseSmall, sparseBig, denseSmall, denseBig));
				allChunks.Add ("0,1", new Chunk (0, 1, seed, sparseSmall, sparseBig, denseSmall, denseBig));
				allChunks.Add ("1,1", new Chunk (1, 1, seed, sparseSmall, sparseBig, denseSmall, denseBig));
				allChunks.Add ("0,-1", new Chunk (0, -1, seed, sparseSmall, sparseBig, denseSmall, denseBig));
				allChunks.Add ("-1,0", new Chunk (-1, 0, seed, sparseSmall, sparseBig, denseSmall, denseBig));
				allChunks.Add ("1,-1", new Chunk (1, -1, seed, sparseSmall, sparseBig, denseSmall, denseBig));
				allChunks.Add ("-1,1", new Chunk (-1, 1, seed, sparseSmall, sparseBig, denseSmall, denseBig));
				allChunks.Add ("-1,-1", new Chunk (-1, -1, seed, sparseSmall, sparseBig, denseSmall, denseBig));
				
		
				
	
		}
		//Chunk - COntainer for information about the block field in a 100 by 100 area
		private class Chunk : Object
		{
				//position in index space
				int xIndex;
				int yIndex;
				
				//The chunk we create that contains the blocks
				public GameObject field;
				
	
				//new chunk
				public Chunk (int xCoord, int yCoord, float seed, GameObject sparseSmall, GameObject sparseBig, GameObject denseSmall, GameObject denseBig)
				{
						
						
						float density = Mathf.PerlinNoise (IndexToWorld (xCoord) / 1000f + seed, IndexToWorld (yCoord) / 1000f + seed);
						float size = Mathf.PerlinNoise (IndexToWorld (xCoord) / 1000f + (seed * 2f), IndexToWorld (yCoord) / 1000f + (seed * 2f));
						
						print ("desnity: " + density);
						print ("size: " + size);
						
						this.xIndex = xCoord;
						this.yIndex = yCoord;
						
						
						
						if (size < 0.5f & density < 0.5f) {
								
								this.field = (GameObject)Instantiate (sparseSmall, new Vector3 (IndexToWorld (xCoord), IndexToWorld (yCoord), 0f), Quaternion.identity);
						}
						if (size > 0.5f & density > 0.5f) {
								
								this.field = (GameObject)Instantiate (denseBig, new Vector3 (IndexToWorld (xCoord), IndexToWorld (yCoord), 0f), Quaternion.identity);
						}
						if (size < 0.5f & density > 0.5f) {
								
								this.field = (GameObject)Instantiate (denseSmall, new Vector3 (IndexToWorld (xCoord), IndexToWorld (yCoord), 0f), Quaternion.identity);
						}
						if (size > 0.5f & density < 0.5f) {
								
								this.field = (GameObject)Instantiate (sparseBig, new Vector3 (IndexToWorld (xCoord), IndexToWorld (yCoord), 0f), Quaternion.identity);
						}
						
						
						
						
						
	
				}
				
				
	
		}
		
		//Transform from index space to world space
		static float IndexToWorld (int index)
		{
			
				return  ((float)index) * 100f;
			
		}
		
		//Transform from world space to index space
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
				
				//check if the 3 x 3 chunks around the player and load  ones that are missing
				
				int currentX = WorldToIndex (Player.transform.position.x);
				int currentY = WorldToIndex (Player.transform.position.y);
				for (int q = -1; q <= 1; q++) {		
						for (int p = -1; p <= 1; p++) {
								string tempX = (currentX + q).ToString ();
								string tempY = (currentY + p).ToString ();
								if (allChunks.ContainsKey (tempX + "," + tempY) == false) {
				
										allChunks.Add (tempX + "," + tempY, new Chunk (currentX + q, currentY + p, seed, sparseSmall, sparseBig, denseSmall, denseBig));
										
				
								}
						}
				}
				List<string> list = allChunks.Select (x => x.Key).ToList ();
			
				
				//delete chunks that are too far away from teh player
				foreach (string entry in list) {
						if (Vector3.Distance (Player.transform.position, allChunks [entry].field.transform.position) > maxDistance) {
			
								DestroyObject (allChunks [entry].field);
								//allChunks [entry].field.SendMessage ("Kill");
								allChunks.Remove (entry);
			
						}
				}
				
				
		
		
		}
}