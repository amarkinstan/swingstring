using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetupCell : MonoBehaviour
{
		//postion of this cell relative to 0,0 being the centre
		public int startXPos;
		
		private int xPos;
		
		//postion of this cell relative to 0,0 being the centre
		public int startYPos;
		
		private int yPos;

		private GameObject player;
		
		//colour to represent jungle zone
		public Color jungle;
		
		//colour to represent tundra zone
		public Color tundra;
		
		//colour to represent desert zone
		public Color desert;
		
		//colour to represent snow zone
		public Color snow;
		
		//colour to where the player is
		public Color special;
		
		private Image thisImage;
		
		//The block desnity of the chunk we are looking at
		private float density;
		
		//The block size of the chunk we are looking at
		private float size;
		
		// Use this for initialization
		void Start ()
		{
	
				player = GameObject.Find ("Player");
				thisImage = GetComponent<Image> ();
				EventManager.GamePause += GamePause;
				
	
		}
	
	
	
		//Only update on Pause
		void GamePause ()
		{
				
				//get the postion of the chunk we are looking at in the game world
				xPos = startXPos + ChunkManager.WorldToIndex (player.transform.position.x);
				yPos = startYPos + ChunkManager.WorldToIndex (player.transform.position.y);
				
				//what is that chunks block desnity and size?
				density = GlobalStuff.getDensity (ChunkManager.IndexToWorld (xPos), ChunkManager.IndexToWorld (yPos), GlobalStore.Seed);
		
				size = GlobalStuff.getSize (ChunkManager.IndexToWorld (xPos), ChunkManager.IndexToWorld (yPos), GlobalStore.Seed);
				
				//Set the colour of the cell to represent the zone that we would find
				if (size < 0.5f & density < 0.5f) {
						thisImage.color = desert;
			
				}
				if (size > 0.5f & density > 0.5f) {
						thisImage.color = jungle;
			
				}
				if (size < 0.5f & density > 0.5f) {
						thisImage.color = tundra;
			
				}
				if (size > 0.5f & density < 0.5f) {
						thisImage.color = snow;
			
				}
				if (startYPos == 0) {
						if (startXPos == 0) {
								thisImage.color = special;
						}
				}
				
				//make sure nothing moves
				transform.localRotation = Quaternion.identity;
				transform.localScale = Vector3.one;
	
		}
}
