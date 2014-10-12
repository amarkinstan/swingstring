using UnityEngine;
using System.Collections;

public class CreateField : MonoBehaviour
{

		public Transform prefab;
		public int numberOfObjects;
		public Vector3 minSize, maxSize;
		public Vector2 fieldSize;
		public Color shade;
		private int count = 0;

		Transform o;

		// Use this for initialization
		void Start ()
		{

				for (int i = 1; i < numberOfObjects; i++) {
						Vector3 scale = new Vector3 (
				Random.Range (minSize.x, maxSize.x),
				Random.Range (minSize.y, maxSize.y),
				Random.Range (minSize.z, maxSize.z));
						Vector3 position;
						position = new Vector3 (fieldSize.x * Gaussian (), fieldSize.y * Gaussian (), 0f);
						if (Physics.CheckSphere (position, scale.magnitude / 4f) == false) {
								o = (Transform)Instantiate (prefab, position, Quaternion.AngleAxis (Random.value * 360f, Vector3.back));
								o.localScale = scale;
								o.renderer.material.SetColor ("_Color", Color.Lerp (o.renderer.material.color, shade, (float)Random.value));
								count++;
								print (count);
						}
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
		
		}
}
