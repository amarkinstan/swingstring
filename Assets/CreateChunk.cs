using UnityEngine;
using System.Collections;

public class CreateChunk : MonoBehaviour
{
		
		public Transform prefab;
		
		public bool isGaussian;
		public int numberOfObjects;
		public Vector3 minSize, maxSize;
		public Vector2 fieldSize;
		public Color shade;
		private int count = 0;
		private float seed;

		Transform o;

		// Use this for initialization
		void Start ()
		{
				StartCoroutine ("MakeBlocks");
		}
		
		IEnumerator MakeBlocks ()
		{
				seed = GlobalStuff.Seed;
				print (transform.position);
				Random.seed = (int)(Mathf.PerlinNoise (transform.position.x + 0.01f, transform.position.y + 0.01f) * 1000 + seed);
				print ((int)(Mathf.PerlinNoise (transform.position.x + 0.01f, transform.position.y + 0.01f) * 1000 + seed));
				
				for (int i = 1; i < numberOfObjects; i++) {
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
						if (Physics.CheckSphere (position, scale.magnitude / 4f) == false) {
								o = (Transform)Instantiate (prefab, position, Quaternion.AngleAxis (Random.value * 360f, Vector3.back));
								o.parent = transform;
								o.localScale = scale;
								o.renderer.material.SetColor ("_Color", Color.Lerp (o.renderer.material.color, shade, (float)Random.value));
								count++;
								//print (count);
						}
				}
				yield return null;
	
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
		
		}
}
