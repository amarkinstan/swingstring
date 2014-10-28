using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RopeControl : MonoBehaviour
{
		//The object that is going to appear when the rope bends (makes the rope appear smooth)
		public GameObject ropeBend;
	
		//maximum distance tha tth eplayer can attact to an object
		public float maxRopeLength = 72f;
	
		//Speed that a rope is shortened
		public float retractSpeed = 2f;
	
		//Spring constant for ropes
		public float spring = 50f;
	
		//Spring Damper for Ropes
		public float damper = 0.2f;
	
		//The object that is attached to he ropes (usually the player)
		public GameObject player;
	
		//Internal variables
	
		//The position of the first anchored attached to an object
		private Vector3 anchorPoint;
	
		//The object that is going to appear when the rope begins (makes the rope appear smooth)
		private GameObject anchorBendObject;
	
		//Is the player attached to a rope??
		private bool attatched = false;
	
		//The series of Ropes attached to the player 
		private List<Rope> Ropes;
	
		//The angle between the first(active) rope and the normal left vector from last frame
		private float lastAngle;
	
		//The angle between the first(active) rope and the normal left vector from this frame
		private float currentAngle;
	
		//The angle between the currentAngle rope and lastAngle, from which we can determine clockwise or anti-clockwise movement
		private float deltaAngle;
	
	
		//The Rope Segment
		public class Rope : Object
		{
				//the component that creates the rope line
				public LineRenderer line;
				
				//The Object in the world that the line renderer sit inside
				public GameObject hasLine;
				
				//The Object that will contain the dot model that makes the ropes appear to have rounded ends
				public GameObject hasBend;
				
				//anchor position of rope
				public Vector3 anchor;
				
				//position other end of the rope
				public Vector3 end;
				
				//The physics component that actually does the swinging
				public ConfigurableJoint ropeJoint;
				
				//The property box that contains information about the Joint
				public SoftJointLimit jointLimit;
				
				//Which direction the Rope is going in
				public bool isAntiClockwise;
				
		
				//Make New Rope
				public Rope (Vector3 anchor, Vector3 end, GameObject bend)
				{
						this.anchor = anchor;
						this.end = end;
						this.hasBend = (GameObject)Instantiate (bend);
						this.hasBend.renderer.enabled = false;
						this.hasLine = new GameObject ();
						this.line = this.hasLine.AddComponent<LineRenderer> ();
						//Visual size of Rope - should be param
						this.line.SetWidth (0.1f, 0.1f);
						this.line.SetVertexCount (2);
						this.line.material.color = Color.black;
						this.line.enabled = false;
				}
		
				//Retract the rope by some amount
				public void Retract (float amount)
				{
						this.jointLimit.limit -= amount;
						this.ropeJoint.linearLimit = this.jointLimit;
				}
		
				//Configure a rope's physics - could be added to constructor(?)
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
		
				//Draw a rope's line
				public void drawLine (Vector3 start, Vector3 end)
				{
						this.line.enabled = true;
						this.line.SetPosition (0, start);
						this.line.SetPosition (1, end);
				}
		}
	
		//Joins two ropes into one when no longer wrapping around something
		void RopeJoin (List<Rope> toJoin)
		{
				//make sure the length of the new rope is correct
				float oldLength1 = Vector3.Distance (toJoin [1].end, toJoin [1].anchor);
				float oldLength2 = toJoin [0].jointLimit.limit;
				//Clean up Objects
				DestroyObject (toJoin [0].hasLine);
				KillJoints ();
				DestroyObject (toJoin [0].hasBend);
				//Delete Rope
				toJoin.RemoveAt (0);
				//Correct the position/length of remaining rope
				toJoin [0].end = player.transform.position;
				toJoin [0].setUpRope (player, oldLength1 + oldLength2, spring, damper);
				//Move bend object to appear round
				toJoin [0].hasBend.transform.position = toJoin [0].anchor;
		
		}
	
		//Splits a single rope into two when wrapping around an object
		void RopeSplit (List<Rope> toSplit, Vector3 splitPoint)
		{
				toSplit [0].end = splitPoint;
				toSplit [0].hasBend.renderer.enabled = true;
				toSplit [0].hasBend.transform.position = splitPoint;
				//add to beginning of list of ropes - Shoudl use a Stack
				toSplit.Insert (0, new Rope (splitPoint, player.transform.position, ropeBend));
				//clean up
				KillJoints ();
				//setup new Rope
				toSplit [0].setUpRope (player, Vector3.Distance (player.transform.position, splitPoint), spring, damper);
				toSplit [0].hasBend.transform.position = splitPoint;
				//check direction of new rope
				if (deltaAngle > 0f) {
						toSplit [0].isAntiClockwise = true;
				} else {
						toSplit [0].isAntiClockwise = false;
			
				}
		
		}
	
		//Remove all of the physics joint components that are attached to the player
		void KillJoints ()
		{
		
				ConfigurableJoint[] allRopes = player.GetComponents<ConfigurableJoint> ();
		
				foreach (Component rope in allRopes) {
			
						Component.Destroy (rope);
			
				}
		
		}
	
		//Given a point in world space and an object, get the closest vertex of that object, then find a point wireSize away from that.
		//Assumes Object is a cube
		//TODO use any object
		Vector3 FindCorner (Vector3 hitSpot, Transform hitCube, float wireSize)
		{
		
				Vector3 centrePos;
				float height;
				float width;
				List<Vector3> corners = new List<Vector3> ();
		
								
				centrePos = Vector3.zero;
				height = hitCube.localScale.y;
				width = hitCube.localScale.x;
		
				width = width + wireSize;
				height = height + wireSize;
				
				
				
				//topleft
				corners.Add (new Vector3 (centrePos.x + 0.5f, centrePos.y + 0.5f, 0f));
				//topright
				corners.Add (new Vector3 (centrePos.x - 0.5f, centrePos.y + 0.5f, 0f));
				//botleft
				corners.Add (new Vector3 (centrePos.x + 0.5f, centrePos.y - 0.5f, 0f));
				//topleft
				corners.Add (new Vector3 (centrePos.x - 0.5f, centrePos.y - 0.5f, 0f));
				//closest corner
				corners = corners.OrderBy (vec3 => Vector3.Distance (vec3, hitCube.InverseTransformPoint (hitSpot))).ToList ();
				
				// move away by wireSize
				Vector3 diagDir = Vector3.Normalize (new Vector3 (corners [0].x, 0f, 0f)) + Vector3.Normalize (new Vector3 (0f, corners [0].y, 0f));
		
				Vector3.Normalize (diagDir);
				
				//Debug.DrawRay (hitCube.TransformPoint (corners [0]), hitCube.TransformDirection (diagDir), Color.white, 5000f);
					
				return hitCube.TransformPoint (corners [0]) + (Vector3.Normalize (hitCube.TransformDirection (diagDir)) * 0.0707f);
		
		}
	
		//have we retracted very close to the anchor point?
		bool isRetracted (List<Rope> toCheck)
		{
		
				if (attatched && (toCheck.Count > 1) && (Vector3.Distance (toCheck [0].end, toCheck [0].anchor) < 0.25f)) {
						return true;
				} else {
						return false;
				}
		
		}
	
	
		//Do owe need to join the ropes? (Unwrap around object)
		bool isJoined (List<Rope> toCheck)
		{
		
				if (attatched) {
			
						float distance = Vector3.Distance (toCheck [0].anchor, player.transform.position);
						
						if (toCheck.Count > 1 && distance > 0.3f) {
								
								float angle = AngleSigned (toCheck [1].end - toCheck [1].anchor, toCheck [0].anchor - player.transform.position, Vector3.back);
								
								if ((angle < 0f && toCheck [0].isAntiClockwise == true) || (angle > 0f && toCheck [0].isAntiClockwise == false)) {
										
										return true;
					
								} else {
										return false;
								}							
						}
				}
				return false;
		}
	
		//Do we need to split ropes? (Wrap around object)
		bool isSplit (List<Rope> toCheck)
		{
		
				if (attatched) {
						RaycastHit hit;
						Vector3 direction;
						float distance;
			
			
						//Look for Splits
						direction = toCheck [0].anchor - player.transform.position;
						direction.Normalize ();
						Ray lookSplit = new Ray (player.transform.position, direction);
						Debug.DrawLine (player.transform.position, toCheck [0].anchor, Color.red, Time.deltaTime);
						distance = Vector3.Distance (player.transform.position, toCheck [0].anchor) - 0.25f;
						if (distance > 0f) {
								if (Physics.Raycast (lookSplit, out hit, distance)) {
										RopeSplit (toCheck, FindCorner (hit.point, hit.transform, 0.1f));
					
										return true;
								}
						}
				}
				return false;
		
		}
	
		//Get the true angle between two lines, can return negatives
		float AngleSigned (Vector3 v1, Vector3 v2, Vector3 n)
		{
				return Mathf.Atan2 (
			Vector3.Dot (n, Vector3.Cross (v1, v2)),
			Vector3.Dot (v1, v2)) * Mathf.Rad2Deg;
		}
	
	
		//Visuals
		void LateUpdate ()
		{
				
				if (attatched) {
						Ropes [0].end = player.transform.position;
				}
		
				if (attatched) {
						foreach (Rope itemRope in Ropes) {
								itemRope.drawLine (itemRope.end, itemRope.anchor);
						}
				}
				if (attatched) {
						lastAngle = AngleSigned (player.transform.position - Ropes [0].anchor, Vector3.left, Vector3.back);
				}
		
		}
	
		//Do Checks for wrapping/unwrapping
		void FixedUpdate ()
		{
		
		
				if (isRetracted (Ropes)) {
						RopeJoin (Ropes);
				}
				if (isJoined (Ropes)) {
						RopeJoin (Ropes);
				}
				if (isSplit (Ropes)) {
			
				}
		}
	
		//Is the position we just clicked a good spot  to put an anchor?
		bool isAnchored ()
		{
				Vector3 clickVector;
				Vector3 direction;
				RaycastHit hit;
				Vector3 mousePos;
				float distance;
				mousePos = camera.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10f));
				clickVector = mousePos - player.transform.position;
				distance = clickVector.magnitude;
				direction = clickVector / distance;
				Ray tryAnchor = new Ray (player.transform.position, direction);
				if (Physics.Raycast (tryAnchor, out hit, maxRopeLength)) {
			
						anchorPoint = hit.point + (hit.normal * 0.05f);
						anchorBendObject = (GameObject)Instantiate (ropeBend);
						anchorBendObject.transform.position = anchorPoint;
						player.renderer.material.color = hit.transform.renderer.material.color;
						Color trail = hit.transform.renderer.material.color;
						trail = Color.Lerp (trail, Color.white, 0.5f);
						player.GetComponent<TrailRenderer> ().material.SetColor ("_Color", trail);
						
						return true;
						
				} else {
						return false;
				}
		}
		
		// Controls
		void Update ()
		{
				//Get deltaAngle each frame
				if (attatched) {
						currentAngle = AngleSigned (player.transform.position - Ropes [0].anchor, Vector3.left, Vector3.back);
						deltaAngle = currentAngle - lastAngle;
						//print (deltaAngle);
			
				}
				//Make the rope with left click
				if ((Input.GetButtonDown ("Attach + Pull")) == true && attatched == false) {
						if (isAnchored ()) {
								Ropes = new List<Rope> ();
								Ropes.Add (new Rope (anchorPoint, player.transform.position, ropeBend));
								//Make first rope in list attach
								Ropes [0].setUpRope (player, Vector3.Distance (player.transform.position, anchorPoint), spring, damper);
								Ropes [0].hasBend.renderer.enabled = false;
				
								attatched = true;
								
						}
				}
				//Retract Rope
				if ((Input.GetButton ("Attach + Pull")) == true && attatched == true) {
						//Retract the first rope
						Ropes [0].Retract (retractSpeed * Time.deltaTime);
				}
				//Detach
				if ((Input.GetButtonDown ("Dettach") == true) && attatched == true) {
						//delete all ropeJoints
						attatched = false;
						foreach (Rope itemRope in Ropes) {
								DestroyObject (itemRope.hasLine);
								DestroyObject (itemRope.hasBend);
						}
						KillJoints ();
						DestroyObject (anchorBendObject);
						//Component.Destroy (player.GetComponent<ConfigurableJoint> ());
				}
		}
}