using UnityEngine;
using System.Collections;

public class CreateChunk : MonoBehaviour
{
		
		//The block to place
		public Transform prefab;
			
		//number of blocks in field
		public int numberOfBlocks;
		//number of frames to  generate every frame
		public int blocksPerFrame;
		//how many blocks we ahve made so far
		private int blocksCreated;
		//size range for blocks - affected by size perlin
		public Vector3 minSize, maxSize;
		//extent of field should be 100
		public Vector2 fieldSize;
		
		//amout of ratation allowed
		public float maxRotation;
		
		//chance of sprite appearing 0 -> 1
		public float spriteChance;
		
		//smallest block a sprite will appear on?
		public float spriteMinSize;
		
		//scale factor to apply to sprite
		public float spriteScaling;
		
		//couont of blocks in field
		private int count = 0;
		
		private float seed;
		private int savedSeed;
		
		private float density;
		private float size;
		
		//two colours that define a range of clours between them
		public Color shade1;
		public Color shade2;
		
			
		// what we will call our generic block
		Transform o;
		
		//generic sprite object
		private Transform childSprite;
	
		// Use this for initialization
		void Start ()
		{
				
				
				
				
				//get seed from global seed
				seed = GlobalStuff.Seed;
				
				//get the seed at this position
				savedSeed = (int)(Mathf.PerlinNoise (transform.position.x / 1000f + 0.01f, transform.position.y / 1000f + 0.01f) * 1000000 + seed);
				
				
				density = Mathf.PerlinNoise (transform.position.x / 1000f + seed, transform.position.y / 1000f + seed);
				size = Mathf.PerlinNoise (transform.position.x / 1000f + seed, transform.position.y / 1000f + seed) * Mathf.PerlinNoise (transform.position.x / 1000f + seed * 2f, transform.position.y / 1000f + seed * 2f);
				size = size * 3f;
				print ("desnity: " + density);
				print ("size: " + size);
				minSize = size * minSize;
				maxSize = size * maxSize;
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
				numberOfBlocks = 3 + (int)(numberOfBlocks * density);
				
				
					
		}
		

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
	
		// Update is called once per frame
		void Update ()
		{

				if (blocksCreated < numberOfBlocks) {
					
						//make sure the ssed is the same every time we generate
						Random.seed = savedSeed + blocksCreated;
		
						//make only the expected number of blocks per frame(update)
						for (int i = 1; i <= blocksPerFrame; i++) {
								Vector3 scale = new Vector3 (
										Random.Range (minSize.x, maxSize.x),
										Random.Range (minSize.y, maxSize.y),
										Random.Range (minSize.z, maxSize.z));
								
								Vector3 position = new Vector3 (fieldSize.x * Random.Range (-1f, 1f), fieldSize.y * Random.Range (-1f, 1f), 0f);
								//transform to  position of chunk
								position = position + transform.position;
								
								blocksCreated++;
								//if we have space, place a block
								if (Physics.CheckSphere (position, scale.magnitude / 3f) == false) {
										count++;
					
										//make block
										o = (Transform)Instantiate (prefab, position, Quaternion.AngleAxis ((Random.value * 2f * maxRotation) - maxRotation, Vector3.back));
										//make the block a child of the chunk
										o.parent = transform;
										//change scale
										o.localScale = scale;
										//set the colour as defined by the user
										
										Color C = Color.Lerp (shade1, shade2, (float)Random.value);
										
										o.renderer.material.SetColor ("_Color", C);
										
										
										
										//get the sprite plane attatched to the block
										childSprite = o.GetChild (0);
										
										//should the sprite appear?
										if (Random.value > spriteChance) {
												childSprite.renderer.enabled = false;
										}
										
										if (o.localScale.x < spriteMinSize) {
												childSprite.renderer.enabled = false;
										}
										
										//set size of sprite to something reasonable
										childSprite.localScale = new Vector3 (1f / o.localScale.x, 1f / o.localScale.y, 1f);
										childSprite.localScale = childSprite.localScale * spriteScaling * Random.Range (0.5f, 1f);
										
										//adjust vertical position
										childSprite.localPosition = new Vector3 (childSprite.localPosition.x, 0.5f + (childSprite.localScale.y * 0.5f), childSprite.localPosition.z);
										
										//adjust horizontal postion
										
										childSprite.localPosition = new Vector3 (childSprite.localPosition.x + Random.Range (-0.4f, 0.4f), childSprite.localPosition.y, childSprite.localPosition.z);
										
										//adjust colour
										childSprite.renderer.material.color = Color.Lerp (new Color (C.r, C.g, C.b, 1f), Color.white, 0.35f);
										
										
										//adjust width
										if (Random.value < 0.5f) {
												childSprite.localScale = new Vector3 (-childSprite.localScale.x, childSprite.localScale.y, childSprite.localScale.z);
										}
										childSprite.localScale = new Vector3 (childSprite.localScale.x + (Random.Range (-0.1f, 0.1f)), childSprite.localScale.y, childSprite.localScale.z);
										
								}
		
		
						}
				}
		}
}