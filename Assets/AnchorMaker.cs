using UnityEngine;
using System.Collections;

public class AnchorMaker : MonoBehaviour
{

		LineRenderer line;
		ConfigurableJoint rope;
		SoftJointLimit limit;
		public float retractSpeed = 10f;
		public float startWidth = 0.05f;
		public float endWidth = 0.05f;
		public float spring = 2f;
		public float damper = 1f;
		public GameObject Player;
		private Vector3 anchorPoint;
		private Vector3 mousePos;
		private Vector3 direction;
		private float distance;
		private Vector3 clickVector;
		private bool attatched;

		void Start ()
		{
				//Sets up the line drawing settings
				line = gameObject.AddComponent<LineRenderer> ();
				line.SetWidth (startWidth, endWidth);
				line.SetVertexCount (2);
				line.material.color = Color.black;
				line.enabled = false;


		}

		//make rope line appear	
		void drawLine (GameObject source, Vector3 anchor)
		{
				//sets the line positions start and end points
				//and enables the line to be drawn
				line.enabled = true;
				line.SetPosition (0, source.transform.position);
				line.SetPosition (1, anchor);
		}


		//Visuals
		void LateUpdate ()
		{

				if (attatched) {

						drawLine (Player, anchorPoint);

				} else {
						line.enabled = false;
				}


		}

		//condigure rope
		void setRope (float length, Vector3 anchor)
		{
				rope = Player.AddComponent<ConfigurableJoint> ();
				rope.autoConfigureConnectedAnchor = false;
				rope.axis = Vector3.left;
				rope.secondaryAxis = Vector3.left;
				rope.configuredInWorldSpace = true;
				rope.anchor = Vector3.zero;
				rope.connectedAnchor = anchor;

				rope.xMotion = ConfigurableJointMotion.Limited;
				rope.yMotion = ConfigurableJointMotion.Limited;

				limit.limit = length;
				limit.spring = spring;
				limit.damper = damper;
				rope.linearLimit = limit;

		}

		bool getAnchor ()
		{

				mousePos = camera.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10f));
				print (mousePos);
				
				clickVector = mousePos - Player.transform.position;

				distance = clickVector.magnitude;
				direction = clickVector / distance;
				
				RaycastHit hit;
				Ray tryAnchor = new Ray (Player.transform.position, direction);
				print (tryAnchor);
					if (Physics.Raycast (tryAnchor, out hit)) {
							
							anchorPoint = hit.point;
							distance = Vector3.Distance(Player.transform.position,anchorPoint);
			return true;


		} else { return false;}

		}

		// COntrols
		void Update ()
		{


				if ((Input.GetButton ("Fire1")) == true && attatched == false) {
						

						//Make a new rope to anchorPlace


						if (getAnchor ()) {
								setRope (distance, anchorPoint);
				attatched = true;
						}



				}
				
				if ((Input.GetButton ("Fire1")) == true && attatched == true) {


				limit.limit = limit.limit - (retractSpeed *Time.deltaTime);
			rope.linearLimit = limit;
				}


				if ((Input.GetButton ("Fire2") == true) && attatched == true) {
						attatched = false;


						//delete all ropes

						Component.Destroy (rope);

				}







		}
}
