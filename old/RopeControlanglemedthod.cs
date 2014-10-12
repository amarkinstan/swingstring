using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RopeControlanlgemethod : MonoBehaviour
{
		public GameObject ropeBend;
		public float maxRopeLength = 72f;
		public float retractSpeed = 2f;
		public float spring = 50f;
		public float damper = 0.2f;
		public GameObject Player;
		private Vector3 anchorPoint;
		private GameObject anchorBend;
		private bool attatched;
		private List<Rope> Ropes;
		private Vector3 lastPosition;

		
		
		public class Rope : Object
		{
				public Vector3 splitDirection;
				public LineRenderer line;
				public GameObject hasLine;
				public GameObject hasBend;
				public Vector3 anchor;
				public Vector3 end;
				public ConfigurableJoint ropeJoint;
				public SoftJointLimit jointLimit;
				

				public Rope (Vector3 anchor, Vector3 end, GameObject bend)
				{
						this.anchor = anchor;
						this.end = end;
						this.hasBend = (GameObject)Instantiate (bend);
						this.hasBend.renderer.enabled = false;
						this.hasLine = new GameObject ();
						this.line = this.hasLine.AddComponent<LineRenderer> ();
						this.line.SetWidth (0.1f, 0.1f);
						this.line.SetVertexCount (2);
						this.line.material.color = Color.black;
						this.line.enabled = false;
				}
		
				public void Retract (float amount)
				{
						this.jointLimit.limit -= amount;
						this.ropeJoint.linearLimit = this.jointLimit;
				}

				public void setUpRope (GameObject addTo, float length, float spring, float damper)
				{
						this.ropeJoint = addTo.AddComponent<ConfigurableJoint> ();
						this.ropeJoint.autoConfigureConnectedAnchor = false;
						this.ropeJoint.axis = Vector3.left;
						this.ropeJoint.secondaryAxis = Vector3.left;
						this.ropeJoint.configuredInWorldSpace = true;
						this.ropeJoint.anchor = Vector3.zero;
						this.ropeJoint.connectedAnchor = anchor;
						this.ropeJoint.xMotion = ConfigurableJointMotion.Limited;
						this.ropeJoint.yMotion = ConfigurableJointMotion.Limited;
						this.jointLimit.limit = length;
						this.jointLimit.spring = spring;
						this.jointLimit.damper = damper;
						this.ropeJoint.linearLimit = this.jointLimit;
						this.hasBend.renderer.enabled = true;
						
				}

				public void drawLine (Vector3 start, Vector3 end)
				{
						this.line.enabled = true;
						this.line.SetPosition (0, start);
						this.line.SetPosition (1, end);
				}
		}
	
		void RopeJoin (List<Rope> toJoin)
		{
				float oldLength1 = Vector3.Distance (toJoin [1].end, toJoin [1].anchor);
				float oldLength2 = toJoin [0].jointLimit.limit;
				DestroyObject (toJoin [0].hasLine);
				//Component.Destroy (Player.GetComponent<ConfigurableJoint> ());
				KillJoints ();
				DestroyObject (toJoin [0].hasBend);
				toJoin.RemoveAt (0);
				toJoin [0].end = Player.transform.position;
				toJoin [0].setUpRope (Player, oldLength1 + oldLength2, spring, damper);
				
				toJoin [0].hasBend.transform.position = toJoin [0].anchor;
		
		}

		void RopeSplit (List<Rope> toSplit, Vector3 splitPoint)
		{
				toSplit [0].end = splitPoint;
				toSplit [0].hasBend.renderer.enabled = true;
				toSplit [0].hasBend.transform.position = splitPoint;
				//add to beginning
				toSplit.Insert (0, new Rope (splitPoint, Player.transform.position, ropeBend));
				//Component.Destroy (Player.GetComponent<ConfigurableJoint> ());
				KillJoints ();
				toSplit [0].setUpRope (Player, Vector3.Distance (Player.transform.position, splitPoint), spring, damper);
				toSplit [0].hasBend.transform.position = splitPoint;
				toSplit [0].splitDirection = Player.transform.position - lastPosition;
		}
	
		void KillJoints ()
		{
		
				ConfigurableJoint[] allRopes = Player.GetComponents<ConfigurableJoint> ();

				foreach (Component rope in allRopes) {
			
						Component.Destroy (rope);
			
				}
		
		}
	
		Vector3 FindCorner (Vector3 hitSpot, Transform hitCube, float wireSize)
		{
		
				Vector3 centrePos;
				float height;
				float width;
				List<Vector3> corners = new List<Vector3> ();
		
				centrePos = hitCube.position;
				height = hitCube.localScale.y;
				width = hitCube.localScale.x;
		
				width = width + wireSize;
				height = height + wireSize;
		
				//topleft
				corners.Add (new Vector3 (centrePos.x + (width / 2f), centrePos.y + (height / 2f), 0f));
				//topright
				corners.Add (new Vector3 (centrePos.x - (width / 2f), centrePos.y + (height / 2f), 0f));
				//botleft
				corners.Add (new Vector3 (centrePos.x + (width / 2f), centrePos.y - (height / 2f), 0f));
				//topleft
				corners.Add (new Vector3 (centrePos.x - (width / 2f), centrePos.y - (height / 2f), 0f));
		
				corners = corners.OrderBy (vec3 => Vector3.Distance (vec3, hitSpot)).ToList ();
		
				return corners [0];
		
		}

		bool isRetracted (List<Rope> toCheck)
		{

				if (attatched && (toCheck.Count > 1) && (Vector3.Distance (toCheck [0].end, toCheck [0].anchor) < 0.25f)) {
						return true;	
				} else { 
						return false; 
				}

		}
		
		float AngleSigned (Vector3 v1, Vector3 v2, Vector3 n)
		{
				return Mathf.Atan2 (
				Vector3.Dot (n, Vector3.Cross (v1, v2)),
				Vector3.Dot (v1, v2)) * Mathf.Rad2Deg;
		}
	

		bool isJoined (List<Rope> toCheck)
		{

				if (attatched) {
						RaycastHit hit;
						Vector3 direction;
						Vector3 cross;
						
						
						if (toCheck.Count > 1) {
						
								
								
								
								
								float angle = AngleSigned (toCheck [1].end - toCheck [1].anchor, toCheck [0].anchor - Player.transform.position, Vector3.back);
								cross = Vector3.Cross (toCheck [0].splitDirection * 10f, Vector3.right);
				
								print (angle);
								print (cross * 100f);
				
								
								
								//if (1 == 0) {
								
								cross = cross * 100f;
										
								
								if (angle * cross.normalized.z > 0f) {
										return true;
								} else {
										return false;
								}
//								direction = toCheck [1].end - toCheck [1].anchor;
//								direction.Normalize ();
//
//								Ray lookJoin = new Ray (toCheck [1].anchor, direction);
//
//								Debug.DrawLine (toCheck [1].end, toCheck [1].anchor, Color.blue, Time.deltaTime);
//
//								if (Physics.Raycast (lookJoin, out hit)) {
//										if (hit.transform.tag == "Player") {
//												return true;
//										} else {
//												return false; 
//										}
//								}
						}
				}
				return false;

		}
		
		bool isSplit (List<Rope> toCheck)
		{
		
				if (attatched) {
						RaycastHit hit;
						Vector3 direction;
						float distance;
						//Look for Splits
						direction = toCheck [0].anchor - Player.transform.position;
						direction.Normalize ();
						Ray lookSplit = new Ray (Player.transform.position, direction);
						Debug.DrawLine (Player.transform.position, toCheck [0].anchor, Color.red, Time.deltaTime);
						distance = Vector3.Distance (Player.transform.position, toCheck [0].anchor) - 0.25f;
						if (distance > 0f) {
								if (Physics.Raycast (lookSplit, out hit, distance)) {
										RopeSplit (toCheck, FindCorner (hit.point, hit.transform, 0.1f));
										
										return true;
								}
						}
				}
				return false;
		
		}
	
		//Visuals
		void LateUpdate ()
		{
				
				if (isJoined (Ropes)) {
						RopeJoin (Ropes);
				}
				if (isSplit (Ropes)) {
					
				}
			 		
				if (isRetracted (Ropes)) {
						RopeJoin (Ropes);
				}
				
				
				
		
				if (attatched) {
						Ropes [0].end = Player.transform.position;
				}
		
				if (attatched) {
						foreach (Rope itemRope in Ropes) {
								itemRope.drawLine (itemRope.end, itemRope.anchor);
						}
				}
				lastPosition = Player.transform.position;
				
		}

		bool isAnchored ()
		{
				Vector3 clickVector;
				Vector3 direction;
				RaycastHit hit;
				Vector3 mousePos;
				float distance;
				mousePos = camera.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10f));
				clickVector = mousePos - Player.transform.position;
				distance = clickVector.magnitude;
				direction = clickVector / distance;
				Ray tryAnchor = new Ray (Player.transform.position, direction);
				if (Physics.Raycast (tryAnchor, out hit, maxRopeLength)) {
						
						anchorPoint = hit.point + (hit.normal * 0.05f);	
						anchorBend = (GameObject)Instantiate (ropeBend);
						anchorBend.transform.position = anchorPoint;
						return true;
				} else {
						return false;
				}
		}
		// Controls
		void Update ()
		{
				if ((Input.GetButton ("Attach + Pull")) == true && attatched == false) {
						if (isAnchored ()) {
								Ropes = new List<Rope> ();
								Ropes.Add (new Rope (anchorPoint, Player.transform.position, ropeBend));
								//Make first rope in list attach
								Ropes [0].setUpRope (Player, Vector3.Distance (Player.transform.position, anchorPoint), spring, damper);
								Ropes [0].hasBend.renderer.enabled = false;
								
								attatched = true;
						}
				}
				if ((Input.GetButton ("Attach + Pull")) == true && attatched == true) {
						//Retract the first rope
						Ropes [0].Retract (retractSpeed * Time.deltaTime);
				}
				if ((Input.GetButton ("Dettach") == true) && attatched == true) {
						//delete all ropeJoints
						attatched = false;
						foreach (Rope itemRope in Ropes) {
								DestroyObject (itemRope.hasLine);
								DestroyObject (itemRope.hasBend);
						}
						KillJoints ();
						DestroyObject (anchorBend);
						//Component.Destroy (Player.GetComponent<ConfigurableJoint> ());
				}
		}
}