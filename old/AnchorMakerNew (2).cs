using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnchorMakerNew : MonoBehaviour
{

		//LineRenderer line;
		//ConfigurableJoint rope;
		//SoftJointLimit limit;
		public float retractSpeed = 10f;
		public float spring = 2f;
		public float damper = 1f;
		public GameObject Player;
		private Vector3 anchorPoint;
		private Vector3 mousePos;
		private Vector3 direction;
		private float distance;
		private Vector3 clickVector;
		private bool attatched;
		private List<Rope> Ropes;

		public class Rope : Object
		{
				public LineRenderer line;
				public GameObject hasLine;
				public Vector3 anchor;
				public Vector3 end;
				public ConfigurableJoint rope;
				public SoftJointLimit jointLimit;

				public Rope (Vector3 anchor, Vector3 end)
				{

						this.anchor = anchor;
						this.end = end;
						this.hasLine = new GameObject();
						this.line = this.hasLine.AddComponent<LineRenderer> ();
						this.line.SetWidth (0.1f, 0.1f);
						this.line.SetVertexCount (2);
						this.line.material.color = Color.black;
						this.line.enabled = false;
				}

				public void Retract (float amount)
				{


						this.jointLimit.limit -= amount;
	
						this.rope.linearLimit = this.jointLimit;

				}

				public void setUpRope (GameObject addTo, float length, float spring, float damper)
				{
			
						this.rope = addTo.AddComponent<ConfigurableJoint> ();
						this.rope.autoConfigureConnectedAnchor = false;
						this.rope.axis = Vector3.left;
						this.rope.secondaryAxis = Vector3.left;
						this.rope.configuredInWorldSpace = true;
						this.rope.anchor = Vector3.zero;
						this.rope.connectedAnchor = anchor;
			
						this.rope.xMotion = ConfigurableJointMotion.Limited;
						this.rope.yMotion = ConfigurableJointMotion.Limited;
			
						this.jointLimit.limit = length;
						this.jointLimit.spring = spring;
						this.jointLimit.damper = damper;
						this.rope.linearLimit = this.jointLimit;
			
				}

				public void drawLine (Vector3 start, Vector3 end)
				{
						this.line.enabled = true;
						this.line.SetPosition (0, start);
						this.line.SetPosition (1, end);

				}


		}




		//Visuals
		void LateUpdate ()
		{

				if (attatched) {

						foreach (Rope itemRope in Ropes) {
							itemRope.drawLine (Player.transform.position, anchorPoint);
						}

				} 


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
						distance = Vector3.Distance (Player.transform.position, anchorPoint);
						return true;


				} else {
						return false;
				}

		}

		// COntrols
		void Update ()
		{


				if ((Input.GetButton ("Fire1")) == true && attatched == false) {
						

						if (getAnchor ()) {

								Ropes = new List<Rope>();
								Ropes.Add(new Rope (anchorPoint, Player.transform.position));
								//Shotrope = new Rope (anchorPoint, Player.transform.position);
								Ropes[0].setUpRope(Player, distance, spring, damper);
								//Shotrope.setUpRope (Player, distance, spring, damper);
								attatched = true;
						}

				}
				
				if ((Input.GetButton ("Fire1")) == true && attatched == true) {

						//retract
						//Shotrope.Retract (retractSpeed * Time.deltaTime);
						Ropes[0].Retract(retractSpeed * Time.deltaTime);

				}


				if ((Input.GetButton ("Fire2") == true) && attatched == true) {
						attatched = false;
							foreach (Rope itemRope in Ropes) {
								DestroyObject(itemRope.hasLine);
							//Component.Destroy(itemRope.hasLine.GetComponent<LineRenderer>());
							}
							Component.Destroy(Player.GetComponent<ConfigurableJoint>());
						//delete all ropes

				}







		}
}
