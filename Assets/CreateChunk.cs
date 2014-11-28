using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateChunk : MonoBehaviour
{
		
		//The blocks to place
		
		//default block
		public Transform prefab;
		
		public Transform StoneCircle;
		
		public Transform AGField;
		
		public Transform SlowField;
		
		public Transform Bouncer;
		
		public Transform Spinner;
		
		public Transform NoAttach;
				
		//should we make empty chunks?
		public bool canBeEmpty;
		
		public float emptyChance;
		
		//should we make mesas (huge blocks in the centre of a chunk?)
		public bool canBeMesa;
		
		public float mesaChance;
		
		private bool mesa = false;
		
		//should we make stoneCircles?
		public bool canBeStoneCircle;
		
		public float circleChance;
		
		private bool circle = false;
		
		//should we make chunks were the blocks overlap on creation?
		public bool canBeOverlap;
		
		public float overlapChance;
		
		private bool overlap = false;
		
		//should we spawn clouds that allow the player to freely spin around them?
		public bool canHaveSpinner;
		
		public float spinnerChance;
		
		//should we spawn clouds that are very bouncy?
		public bool canHaveBouncer;
		
		public float bouncerChance;
		
		//should we sspawn areas that slow the player?
		public bool canHaveSlow;
		
		public float slowChance;
		
		//should we spawn aint-gravity fields?
		public bool canHaveAG;
		
		public float agChance;
		
		//shouls we spwan blocks that he player can't attach to?
		public bool canHaveNoAttatch;
		
		public float noAttatchChance;
																				
		//number of blocks in field
		public int numberOfBlocks;
		//number of frames to  generate every frame
		public int blocksPerFrame;
		//how many blocks we have made so far
		private int blocksCreated;
		//size range for blocks - affected by size perlin
		public Vector3 minSize, maxSize;
		//extent of field should be 100
		public Vector2 fieldSize;
		
		//amout of ratation allowed for deault blocks in the the chunk (affects postion of sprites)
		public float maxRotation;
		
		
		//couont of blocks in field
		private int count = 0;
		
		private float seed;
		private int savedSeed;
		
		//number that respresents the amount of number of blocks int he chunk, effects numberOfBlocks
		private float density;
		//scale factor for default(and some others) blockss
		private float size;
		
		//two colours that define a range of clours between them
		public Color shade1;
		public Color shade2;
		
		
	
		// Use this for initialization
		void Start ()
		{
				
				//get seed from global seed
				seed = GlobalStuff.Seed;
				
				//get the seed at this position, needed for prooer terrain generation
				savedSeed = (int)(Mathf.PerlinNoise (transform.position.x / 1000f + 0.01f, transform.position.y / 1000f + 0.01f) * 1000000 + seed);
				
				Random.seed = (int)savedSeed;
				
				//find the density and size
				density = Mathf.PerlinNoise (transform.position.x / 1000f + seed, transform.position.y / 1000f + seed);
				size = Mathf.PerlinNoise (transform.position.x / 1000f + (seed * 2f), transform.position.y / 1000f + (seed * 2f));
				//make size effect scale
				size = size * 2f;
				//print ("desnity: " + density);
				//print ("size: " + size);
				minSize = size * minSize;
				maxSize = size * maxSize;
				
				//make sure that the min thickness for blcoks is 1
				if (minSize.x < 1f) {
						minSize.Set (1f, minSize.y, minSize.z);
				}
				if (minSize.y < 1f) {
						minSize.Set (minSize.x, 1f, minSize.z);
				}
				if (maxSize.x < 1f) {
						maxSize.Set (1f, maxSize.y, maxSize.z);
				}
				if (maxSize.y < 1f) {
						maxSize.Set (maxSize.x, 1f, maxSize.z);
				}
				//make density effet number of blocks, there must be at least soe blocks in the chunk
				numberOfBlocks = 3 + (int)(numberOfBlocks * density);
				
				print ("number of blocks " + numberOfBlocks);
				
				//setup empty distribution
				if (canBeEmpty) {
				
						if (Random.value < emptyChance) {
				
								numberOfBlocks = 0;
				
						}
				
				}
				
				//setup Overlap Distribution
				if (canBeOverlap) {
				
						if (Random.value < overlapChance) {
						
								overlap = true;
								numberOfBlocks = numberOfBlocks + 10;
						}
				
				}
				
				//setup CircleDistribution
				if (canBeStoneCircle) {
			
						if (Random.value < circleChance) {
				
								
								circle = true;
								
								if (circle) {
					
										numberOfBlocks = numberOfBlocks * 2;
										Transform o = (Transform)Instantiate (StoneCircle, transform.position, Quaternion.AngleAxis ((Random.value * 360f), Vector3.back));
										o.parent = transform;
					
								}
						}
			
				}
				//setup Mesa Distribution
				if (canBeMesa) {
			
						if (Random.value < mesaChance) {
				
								mesa = true;
				
						}
			
				}
				
					
		}
		
		//return gaussian (-4ish to 4 ish)
		static float Gaussian ()
		{
				float u, v, S;

				do {

						u = 2.0f * Random.value - 1.0f;
						v = 2.0f * Random.value - 1.0f;
						S = u * u + v * v;
				} while (S >= 1.0f);
		
				float fac = Mathf.Sqrt (-2.0f * Mathf.Log (S) / S);
				return u * fac;
		}
	
		//generate a single block
		void MakeBlock (Vector3 blockScale, Vector3 blockPosition)
		{		
				//select a block
				List<int> availableBlocks = new List<int> ();
				
				int blockNum = 0;
				
				Transform type = prefab;
				
				Transform o;
		
				Color C;
				//figure out which blocks we can select from
				if (canHaveAG) {
						if (Random.value < agChance) {
								availableBlocks.Add (1);
						}
				}
				if (canHaveSlow) {
						if (Random.value < slowChance) {
								availableBlocks.Add (2);
						}
				}
				if (canHaveBouncer) {
						if (Random.value < bouncerChance) {
								availableBlocks.Add (3);
						}
				}
				if (canHaveSpinner) {
						if (Random.value < spinnerChance) {
								availableBlocks.Add (4);
						}
				}
				if (canHaveNoAttatch) {
						if (Random.value < noAttatchChance) {
								availableBlocks.Add (5);
						}
				}
		
				
				if (availableBlocks.Count == 0) {
			
						blockNum = 0;
			
				} else {
						//choose a block from the selection
						blockNum = availableBlocks [Random.Range (0, availableBlocks.Count - 1)];
								}
				//make the different types of blocks, set up relationships, scale colour and sprites
				switch (blockNum) {
				 
				case 1:
						type = AGField;
						//make block
						o = (Transform)Instantiate (type, blockPosition, Quaternion.AngleAxis (Random.value * 360f, Vector3.back));
						//make the block a child of the chunk
						o.parent = transform;
						//change scale
						o.localScale = new Vector3 (blockScale.magnitude * 2.5f, blockScale.magnitude * 2.5f, Random.Range (2f, 4f));
						//set the colour as defined by the user
			
						break;
				 
				case 2:
						type = SlowField;
			
						//make block
						o = (Transform)Instantiate (type, blockPosition, Quaternion.AngleAxis (Random.value * 360f, Vector3.back));
						//make the block a child of the chunk
						o.parent = transform;
						//change scale
						o.localScale = new Vector3 (blockScale.magnitude * 2.5f, blockScale.magnitude * 2.5f, Random.Range (2f, 4f));
						if (o.localScale.x > 25f) {
								o.localScale = new Vector3 (25f, 25f, o.localScale.z);
						}
						
						//set the colour as defined by the user
			
			
			
						
						break;
						
				case 3:
						type = Bouncer;
						//make block
						o = (Transform)Instantiate (type);
						o.position = blockPosition;
						//make the block a child of the chunk
						o.parent = transform;
						//change scale
						o.localScale = o.localScale * Random.Range (0.8f, 2f);
						
			
						setUpSpritesSpecial (o);
			
						
			
						
						break;
						
				case 4:
						type = Spinner;
						//make block
						o = (Transform)Instantiate (type);
						o.position = blockPosition;
						//make the block a child of the chunk
						o.parent = transform;
						//change scale
						
						
			
						setUpSpritesSpecial (o);
						
			
						
						break;
				
				case 5:
						type = NoAttach;
			
			
						//make block
						o = (Transform)Instantiate (type, blockPosition, Quaternion.AngleAxis ((Random.value * 2f * maxRotation) - maxRotation, Vector3.back));
						//make the block a child of the chunk
						o.parent = transform;
						//change scale
						o.localScale = blockScale;
						//set the colour as defined by the user
				
						C = o.renderer.material.color;		
								
						o.renderer.material.SetColor ("_Color", C);
			
						setUpSprites (o, C);
						break;
				
				case 0:
						type = prefab;
						
			
						//make block
						o = (Transform)Instantiate (type, blockPosition, Quaternion.AngleAxis ((Random.value * 2f * maxRotation) - maxRotation, Vector3.back));
						//make the block a child of the chunk
						o.parent = transform;
						//change scale
						o.localScale = blockScale;
						//set the colour as defined by the user
			
			
			
						C = Color.Lerp (shade1, shade2, (float)Random.value);
			
						o.renderer.material.SetColor ("_Color", C);
			
						setUpSprites (o, C);
						break; 
				 
				}
				
				
				
		
		}
		
		//a minimal setup for sprites attached to special blocks
		void setUpSpritesSpecial (Transform block)
		{
				//get the sprite plane attatched to the block
				foreach (Transform childSprite in  block) {
			
						SpriteSettingsSpecial spriteSettings = childSprite.GetComponent<SpriteSettingsSpecial> ();
			
						//adjust colour
						
						Color C1 = Color.Lerp (childSprite.renderer.material.color, Color.white, spriteSettings.colourRange);
						Color C2 = Color.Lerp (childSprite.renderer.material.color, Color.black, spriteSettings.colourRange);
						
						childSprite.renderer.material.color = Color.Lerp (C1, C2, Random.value);
						
			
			
			
						//adjust width
						if (Random.value < 0.5f && spriteSettings.canFlip) {
								childSprite.localScale = new Vector3 (-childSprite.localScale.x, childSprite.localScale.y, childSprite.localScale.z);
						}
						//childSprite.localScale = new Vector3 (childSprite.localScale.x + (Random.Range (-0.05f, 0.05f)), childSprite.localScale.y, childSprite.localScale.z);
		
		
				}
		}
		
		//setup all the sprites on a block. 
		void setUpSprites (Transform block, Color shade)
		{
		
				
				//get the sprite plane attatched to the block
				foreach (Transform childSprite in  block) {
						
						//Read values from a helper script  that sits on each sprite object
						SpriteSettings spriteSettings = childSprite.GetComponent<SpriteSettings> ();
						
						//should the sprite appear?
						if (Random.value > spriteSettings.spriteChance) {
								childSprite.renderer.enabled = false;
						}
			
						if (block.localScale.x < spriteSettings.minBlockWidth) {
								childSprite.renderer.enabled = false;
						}
			
						//set size of sprite to something reasonable
						childSprite.localScale = new Vector3 (1f / block.localScale.x, 1f / block.localScale.y, 1f);
						childSprite.localScale = childSprite.localScale * spriteSettings.spriteScaling * Random.Range (0.5f, 1f);
			
						//adjust vertical position
						childSprite.localPosition = new Vector3 (childSprite.localPosition.x, 0.5f + (childSprite.localScale.y * 0.5f), childSprite.localPosition.z);
			
						//adjust horizontal postion
			
						childSprite.localPosition = new Vector3 (childSprite.localPosition.x + Random.Range (-0.4f, 0.4f), childSprite.localPosition.y, childSprite.localPosition.z);
			
						//adjust colour
						if (spriteSettings.changeColor) {
								childSprite.renderer.material.color = Color.Lerp (new Color (shade.r, shade.g, shade.b, 1f), Color.white, spriteSettings.spriteLightness);
						}
			
						
						
						//adjust width
						if (Random.value < 0.5f) {
								childSprite.localScale = new Vector3 (-childSprite.localScale.x, childSprite.localScale.y, childSprite.localScale.z);
						}
						//childSprite.localScale = new Vector3 (childSprite.localScale.x + (Random.Range (-0.05f, 0.05f)), childSprite.localScale.y, childSprite.localScale.z);
						
				}
		
		}
	
		// Update is called once per frame
		void Update ()
		{
				//keep making blokcs if we don't have enough
				if (blocksCreated < numberOfBlocks) {
					
						//make sure the seed is the same every time we generate
						Random.seed = savedSeed + blocksCreated;
		
						//make only the expected number of blocks per frame(update). should be a low number
						for (int i = 1; i <= blocksPerFrame; i++) {
								Vector3 scale = new Vector3 (
										Random.Range (minSize.x, maxSize.x),
										Random.Range (minSize.y, maxSize.y),
										Random.Range (minSize.z, maxSize.z));
										
								Vector3 position = new Vector3 (fieldSize.x * Random.Range (-1f, 1f), fieldSize.y * Random.Range (-1f, 1f), 0f);
								
								
								//if we are making a mesa make it first and make it huge
								if (mesa) {
								
										scale = new Vector3 (
						Random.Range (30f, 70f),
						Random.Range (30f, 70f),
						Random.Range (minSize.z, maxSize.z));
						
										position = Vector3.zero;
						
										mesa = false;
								
								}
								
								
								//transform to  position of chunk
								position = position + transform.position;
								
								blocksCreated++;
								//if we have space, place a block
								if (circle == false) {
										if (Physics.CheckSphere (position, scale.magnitude / 3f) == false || overlap) {
												count++;
					
												MakeBlock (scale, position);
										
												
										}
								}
								//if we have to make a stone circle
								if (circle) {
										if ((Physics.CheckSphere (position, scale.magnitude / 3f) == false) && ((Vector3.Distance (position, transform.position) < 50f) == false)) {
												count++;
						
												MakeBlock (scale, position);
						
												
										}
								}
								
								
		
		
						}
				}
		}
}
