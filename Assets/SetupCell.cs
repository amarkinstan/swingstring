using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetupCell : MonoBehaviour
{
		public int startXPos;
		private int xPos;
		
		public int startYPos;
		private int yPos;

		private GameObject player;
		
		public Color jungle;
		
		public Color tundra;
		
		public Color desert;
		
		public Color snow;
		
		public Color special;
		
		private Image thisImage;
		
		private float density;
		
		private float size;
		
		// Use this for initialization
		void Start ()
		{
	
				player = GameObject.Find ("Player");
				thisImage = GetComponent<Image> ();
				EventManager.GamePause += GamePause;
	
		}
	
	
	
		// Update is called once per frame
		void GamePause ()
		{
	
				xPos = startXPos + ChunkManager.WorldToIndex (player.transform.position.x);
				yPos = startYPos + ChunkManager.WorldToIndex (player.transform.position.y);
				
				density = GlobalStuff.getDensity (ChunkManager.IndexToWorld (xPos), ChunkManager.IndexToWorld (yPos), GlobalStore.Seed);
		
				size = GlobalStuff.getSize (ChunkManager.IndexToWorld (xPos), ChunkManager.IndexToWorld (yPos), GlobalStore.Seed);
				
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
				
				transform.localRotation = Quaternion.identity;
				transform.localScale = Vector3.one;
	
		}
}
