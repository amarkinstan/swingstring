using UnityEngine;
using System.Collections;

//smoothly zoom the camera
public class CameraZoom : MonoBehaviour
{

		public float zoomSensitivity = 15.0f;
		public float zoomLerpSpeed = 5.0f;
		public float zoomMin = 1f;
		public float zoomMax = 30f;
	
		private float zoom;
	
	
		void Start ()
		{
				zoom = camera.orthographicSize;
		}
	
		void Update ()
		{
				zoom -= Input.GetAxis ("Zoom") * zoomSensitivity;
				zoom = Mathf.Clamp (zoom, zoomMin, zoomMax);
		}
	
		void LateUpdate ()
		{
				camera.orthographicSize = Mathf.Lerp (camera.orthographicSize, zoom, Time.deltaTime * zoomLerpSpeed);
		}
	

}
