using UnityEngine;
using System.Collections;

public class CreateChunk : MonoBehaviour
{
		
		public Transform prefab1;
		public Transform prefab2;
		private Transform prefab;
		
		public bool isGaussian;
		public int numberOfBlocks;
		public int blocksPerFrame;
		private int blocksCreated;
		public Vector3 minSize, maxSize;
		public Vector2 fieldSize;
		public Color shade1;
		public Color shade2;
		private Color shade;
		private int count = 0;
		private float seed;
		private int savedSeed;
		private float density;
		private float size;

		Transform o;

		// Use this for initialization
		void Start ()
		{
				seed = GlobalStuff.Seed;
				//print (transform.position);
				savedSeed = (int)(Mathf.PerlinNoise (transform.position.x / 1000f + 0.01f, transform.position.y / 1000f + 0.01f) * 1000000 + seed);
				//print (savedSeed);
				
				density = Mathf.PerlinNoise (transform.position.x / 1000f + seed, transform.position.y / 1000f + seed);
				size = Mathf.PerlinNoise (transform.position.x / 1000f + seed, transform.position.y / 1000f + seed) * Mathf.PerlinNoise (transform.position.x / 1000f + seed * 2f, transform.position.y / 1000f + seed * 2f);
				size = size * 3f;
				print ("desnity: " + density);
				print ("size: " + size);
				minSize = size * minSize;
				maxSize = size * maxSize;
				numberOfBlocks = 5 + (int)(numberOfBlocks * density);
				if (density < 0.5f) {
						shade = shade1;
						prefab = prefab1;
				} else {
						shade = shade2;
						prefab = prefab2;
				}
				
					
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
					
		
		
						Random.seed = savedSeed + blocksCreated;
		
		
						for (int i = 1; i < blocksPerFrame; i++) {
								Vector3 scale = new Vector3 (
										Random.Range (minSize.x, maxSize.x),
										Random.Range (minSize.y, maxSize.y),
										Random.Range (minSize.z, maxSize.z));
								Vector3 position;
			
								if (isGaussian) {
										position = new Vector3 (fieldSize.x * Gaussian (), fieldSize.y * Gaussian (), 0f);
								} else {
										position = new Vector3 (fieldSize.x * Random.Range (-1f, 1f), fieldSize.y * Random.Range (-1f, 1f), 0f);
								}
								position = position + transform.position;
								blocksCreated++;
								if (Physics.CheckSphere (position, scale.magnitude / 3f) == false) {
										o = (Transform)Instantiate (prefab, position, Quaternion.AngleAxis (Random.value * 360f, Vector3.back));
										o.parent = transform;
										o.localScale = scale;
										o.renderer.material.SetColor ("_Color", Color.Lerp (o.renderer.material.color, shade, (float)Random.value));
										count++;
										
										//print (count);
								}
						}
		
		
				}
		}
}
