using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class RopeControl2 : MonoBehaviour {

    #region Public Variables

    /* GameObjects */
    public GameObject ropeBend;             // Attach the GameObject to appear on rope bends
    public GameObject player;               // The object that the ropes are attached to
    public Camera mainCamera;               //THe camera the game uses to view the action
    public GameObject lineContainer;         // where all the lines are rendered

    /* Rope configuration */
    public Color ropeColor;                 // Rope colour
    public float ropeWidth = 0.5f;          // Size of Rope in game
    public float maxRopeLength = 72f;       // Max attach distance
    public float retractSpeed = 2f;         // Retract speed
    public float spring = 50f;              // Spring constant
    public float damper = 0.2f;             // Damper constant

    /* Game Settings */
    public int maxRopes = 8;                // Set the max number of ropes to be generated at once
    public bool autoAimOn = true;             //autoaim on

    #endregion Public Variables

    #region Private Variables

    private List<Rope> Ropes = new List<Rope>();
        
    private int splitMask = ~(1 << 9 | 1 << 12 | 1 << 11);  //The mask which defines which layers split the rope
    private int hitMask = ~(1 << 11 | 1 << 12 | 1 << 2);    //The mask that defines which layers the rope can attatch to

    #endregion Private Variables

      
    
    //Get the true angle between two lines, can return negatives
    public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
    {
        return Mathf.Atan2(
    Vector3.Dot(n, Vector3.Cross(v1, v2)),
    Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }

    public static float pointToLineDistance(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
    {
        return Vector3.Magnitude(Vector3.Cross((point - lineStart), (point - lineEnd))) / Vector3.Magnitude(lineEnd - lineStart);
    }

    public static List<Vector3> defineCube()
    {

        Vector3 centrePos = Vector3.zero;
        
        List<Vector3> corners = new List<Vector3>();
        
        //topleft
        corners.Add(new Vector3(centrePos.x + 0.5f, centrePos.y + 0.5f, 0f));
        //topright
        corners.Add(new Vector3(centrePos.x - 0.5f, centrePos.y + 0.5f, 0f));
        //botleft
        corners.Add(new Vector3(centrePos.x + 0.5f, centrePos.y - 0.5f, 0f));
        //topleft
        corners.Add(new Vector3(centrePos.x - 0.5f, centrePos.y - 0.5f, 0f));

        return corners;
        

    }

    //Given a point in world space and an object, get the closest vertex of that object, then find a point wireSize away from that.
    //Assumes Object is a cube
    public static Vector3 FindClosestCornerToEdge(Vector3 hitSpot, Transform hitCube, float wireSize)
    {

        float height;
        float width;
        List<Vector3> corners = RopeControl2.defineCube();


        height = hitCube.localScale.y;
        width = hitCube.localScale.x;

        width = width + wireSize;
        height = height + wireSize;


        corners = corners.OrderBy(vec3 => Vector3.Distance(vec3, hitCube.InverseTransformPoint(hitSpot))).ToList();

        // move away by wireSize
        Vector3 diagDir = Vector3.Normalize(new Vector3(corners[0].x, 0f, 0f)) + Vector3.Normalize(new Vector3(0f, corners[0].y, 0f));

        Vector3.Normalize(diagDir);

        return hitCube.TransformPoint(corners[0]) + (Vector3.Normalize(hitCube.TransformDirection(diagDir)) * (wireSize / 2f));

    }
    
    //variant for autoaim
    public static Vector3 FindClosestCornerToLine(Vector3 lineStart, Vector3 lineEnd, Transform hitCube, float wireSize)
    {


        float height;
        float width;
        List<Vector3> corners = RopeControl2.defineCube();


        height = hitCube.localScale.y;
        width = hitCube.localScale.x;

        width = width + wireSize;
        height = height + wireSize;

        //closest corner
        corners = corners.OrderBy(vec3 => RopeControl2.pointToLineDistance(lineStart, lineEnd, hitCube.TransformPoint(vec3))).ToList();

        // move away by wireSize
        Vector3 diagDir = Vector3.Normalize(new Vector3(corners[0].x, 0f, 0f)) + Vector3.Normalize(new Vector3(0f, corners[0].y, 0f));

        Vector3.Normalize(diagDir);

        return hitCube.TransformPoint(corners[0]) + (Vector3.Normalize(hitCube.TransformDirection(diagDir)) * (wireSize / 2f));


    }

    
    //did we hit something?
    public Vector3? HitTest(Camera camera, int mask, Vector3 origin, bool autoAim)
    {
        Vector3 clickVector;
        Vector3 direction;
        RaycastHit hit;
        Vector3 mousePos;
        Vector3? anchorPoint = null;        // Return value

        float distance;
        mousePos = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        clickVector = mousePos - origin;
        distance = clickVector.magnitude;
        direction = clickVector / distance;
        Ray tryAnchor = new Ray(origin, direction);
        if (Physics.Raycast(tryAnchor, out hit, maxRopeLength, mask))
        {

            anchorPoint = hit.point + (hit.normal * 0.05f);
            //special block interactions
            if (hit.transform.tag == "spinner")
            {

                anchorPoint = hit.transform.position;

            }
            if (hit.transform.tag == "NoAttatch")
            {

                return null;

            }

            GlobalStuff.setLastColor(hit.transform.GetComponent<Renderer>().material.color, player);
                       

            return anchorPoint;

        }
        else if (autoAim)
        {

            RaycastHit[] possibleHits = Physics.SphereCastAll(tryAnchor, 1.0f, distance * 2f, mask);

            if (possibleHits.Length == 0)
            {
                return null;
            }
            else
            {

                List<Vector3> cornerlist = new List<Vector3>();

                foreach (RaycastHit possibleHit in possibleHits)
                {

                    if (possibleHit.transform.tag != "NoAttatch")
                    {
                        if (possibleHit.transform.tag == "spinner")
                        {
                            anchorPoint = possibleHit.transform.position;

                            GlobalStuff.setLastColor(possibleHit.transform.GetComponent<Renderer>().material.color, player);

                            return anchorPoint;
                        }
                        else
                        {
                            cornerlist.Add(FindClosestCornerToLine(origin, mousePos, possibleHit.transform, 0.1f));
                        }
                    }
                }

                Vector3[] listCopy = cornerlist.ToArray<Vector3>();

                foreach (Vector3 cornerPoint in listCopy)
                {
                    Ray canConnectRay = new Ray(cornerPoint, Vector3.Normalize(cornerPoint - origin));
                    if (Physics.Raycast(canConnectRay, Vector3.Distance(origin, cornerPoint), mask))
                    {
                        cornerlist.Remove(cornerPoint);
                    }
                }
                cornerlist = cornerlist.OrderBy(vec3 => pointToLineDistance(origin, mousePos, vec3)).ToList();

                if (cornerlist.Count > 0)
                { 
                    anchorPoint = cornerlist[0];
                }

                //do some colour thing here

                return anchorPoint;
            }
                        
        }

        return anchorPoint;

    }

    void FireRope()
    {
        Vector3? anchorPoint = HitTest(mainCamera, hitMask, player.transform.position, autoAimOn);

        
        if (anchorPoint != null)
        {
            Ropes.Add(new Rope(player, ropeBend, (Vector3)anchorPoint, player.transform.position, spring, damper,ropeColor,ropeWidth,lineContainer));
        }
    }


    void FixedUpdate()
    {
      
        foreach (Rope r in Ropes)
        {
            if (r.isRetracted())
            {
                r.RopeJoin();
                
            }
            if (r.isStraight())
            {
                r.RopeJoin();
                
            }

            Vector3? ropeCol = r.isSplit(splitMask);
            if (ropeCol != null)
            {
                r.RopeSplit((Vector3)ropeCol);
                
            }
            
            r.PhysicsUpdate();
        }

        
    }

    void LateUpdate()
    {
        // Handle visuals
        foreach (Rope r in Ropes)
        {
            r.VisualUpdate();
        }
    }

    void Update()
    {
        // Handle control actions
        if ((Input.GetButtonDown("Attach + Pull")) == true && !GlobalStuff.Paused && !GlobalStuff.isDead)
        {
            

            // Add a new rope
            if (Ropes.Count < maxRopes)
            {
                FireRope();
            }
        }

        // Retract Rope
        if (Input.GetButton("Attach + Pull") && Ropes.Count > 0 == true && !GlobalStuff.Paused && !GlobalStuff.isDead)
        {
            //Retract the first rope
            Ropes[Ropes.Count - 1].Retract(retractSpeed * Time.deltaTime);
        }

        // Detach - destroy all ropes
        if (Input.GetButtonDown("Dettach") && !GlobalStuff.Paused && !GlobalStuff.isDead && Ropes.Count > 0)
        {
           
                this.Ropes[Ropes.Count - 1].Destroy();
                Destroy(this.Ropes[Ropes.Count - 1]);
                this.Ropes.RemoveAt(Ropes.Count - 1);
            
          
        }
    }

}

public class RopeSegment : Object
{
    // The component that draws the line
    private LineRenderer lineRenderer;
    public GameObject line;

    // The Object to smooth rope ends
    public GameObject bend;

    // Line definition
    public Vector3 anchor;
    public Vector3 end;

    // Stored Length
    public float length;
       
    
    // RopeSegment constructor
    public RopeSegment(GameObject bend, Vector3 anchor, Vector3 end,  Color ropeColor, float width, float length,GameObject lineParent)
    {
        this.bend = (GameObject)Instantiate(bend);
        this.bend.transform.parent = lineParent.transform;
        this.bend.name = "Bend";
        this.bend.transform.position = anchor;
        this.bend.GetComponent<Renderer>().enabled = true;

        this.anchor = anchor;
        this.end = end;

        this.length = length;

        // Configure the LineRenderer
        this.line = new GameObject();
        this.line.transform.parent = lineParent.transform;
        this.line.name = "Line";
        this.lineRenderer = this.line.AddComponent<LineRenderer>();
        this.lineRenderer.SetWidth(width, width);
        this.lineRenderer.SetVertexCount(2);
        this.lineRenderer.material.color = ropeColor;
        this.lineRenderer.material.shader = Shader.Find("Self-Illumin/Diffuse");
        
    }

    // Draw the rope segment
    public void DrawLine()
    {
        this.lineRenderer.SetPosition(0, this.anchor);
        this.lineRenderer.SetPosition(1, this.end);
        this.lineRenderer.enabled = true;
    }

    // Drop all handles and destroy this object
    public void Destroy()
    {
        Destroy(this.bend);
        Destroy(this.line);
        Destroy(this);
    }

    public Vector3 Direction()
    {
        return (this.anchor - this.end);
    }


}

public class Rope : Object
{
    
    /* GameObjects */

    // The game object to be used to smooth rope ends
    private GameObject bend;
    private GameObject player;
    public GameObject lineContainer;

    // Visuals
    public Color color;
    public float width;

    // Store spring constants
    public float spring;
    public float damper;

    // A container to track rope segments
    private Stack<RopeSegment> RopeSegments;

    private Vector3 anchorPoint;

    private float lastAngle;
    private float currentAngle;
    private float deltaAngle;

    // Spin direction on init, needed for wrap/unwrap checks
    public bool isAntiClockwise;

    // Physics controls
    public ConfigurableJoint ropeJoint;
    private SoftJointLimit ropeLimit;

   
    

    // Rope constructor
    public Rope(GameObject player, GameObject bend, Vector3 anchor, Vector3 end, float spring, float damper,Color color, float width,  GameObject lineContainer)
    {
        this.player = player;
        this.bend = bend;
        this.spring = spring;
        this.damper = damper;
        this.color = color;
        this.width = width;
        this.lineContainer = lineContainer;      

        // Create the initial rope segment
        this.RopeSegments = new Stack<RopeSegment>();
        this.RopeSegments.Push(new RopeSegment(this.bend, anchor, end, this.color, this.width, Vector3.Distance(anchor, end),lineContainer));

        // Configure the rope physics
        this.ropeJoint = player.AddComponent<ConfigurableJoint>();
        this.ropeJoint.autoConfigureConnectedAnchor = false;
        this.ropeJoint.axis = Vector3.left;
        this.ropeJoint.secondaryAxis = Vector3.left;
        this.ropeJoint.configuredInWorldSpace = true;
        this.ropeJoint.anchor = Vector3.zero;
        this.ropeJoint.connectedAnchor = anchor;
        this.ropeJoint.xMotion = ConfigurableJointMotion.Limited;
        this.ropeJoint.yMotion = ConfigurableJointMotion.Limited;
        this.ropeJoint.zMotion = ConfigurableJointMotion.Limited;
        this.ropeLimit.limit = Vector3.Distance(anchor, end);
        this.ropeJoint.linearLimit = this.ropeLimit;
        this.ropeLimit.spring = spring;
        this.ropeLimit.damper = damper;
        this.ropeLimit.spring = spring;

        
    }

    // Reduce the length of the rope segment by a certain amount
    public void Retract(float amount)
    {
        this.ropeLimit.limit -= amount;
        this.ropeJoint.linearLimit = this.ropeLimit;
        this.LastSegment().length -= amount;
        if (this.RopeSegments.Count > 1)
        {
            this.RopeSegments.ElementAt(1).length -= amount;
        }
    }

    public RopeSegment LastSegment()
    {
        return this.RopeSegments.Peek();
    }

    // Update angle records and visuals
    public void VisualUpdate()
    {
       foreach (RopeSegment segment in RopeSegments)
        {
            segment.DrawLine();
        }
    }
    
    public void PhysicsUpdate()
    {
         // Update the end position of the first LineSegment
        this.LastSegment().end = this.player.transform.position;
        
        // Get deltaAngle each frame
        if (this.RopeSegments.Count > 1)
        {
            this.currentAngle = RopeControl2.AngleSigned(this.LastSegment().end - this.LastSegment().anchor,
                    Vector3.left, Vector3.back);
            this.deltaAngle = this.currentAngle - this.lastAngle;
            this.lastAngle = this.currentAngle;
        }
    }

    // Splits a single rope into two when wrapping around an object
    public void RopeSplit(Vector3 splitPoint)
    {
        // The 'anchor' of the existing rope segment is left untouched since this is 
        // where the bend object is


        //set the length of the new segment to the length of the old rope minus the part that was split
        float newDistance = this.LastSegment().length - (Vector3.Distance(this.LastSegment().anchor, splitPoint));

        // Adjust the end location of the colliding rope segment to the split point
        this.LastSegment().end = splitPoint;
                

        // Create a new rope segment from the player to the split point
        this.RopeSegments.Push(new RopeSegment(this.bend, splitPoint, this.player.transform.position, this.color, this.width,newDistance,this.lineContainer));

        // Adjust the spring (ConfigurableJoint) to reflect the anchor of the most
        // recently added RopeSegment
        this.RefreshJoint();

        // Record the initial swing direction
        this.isAntiClockwise = deltaAngle > 0f;
    }

    // Merges two ropes when unwrapping from an object
    public void RopeJoin()
    {
                
        // Destroy and derefence the most recently added RopeSegment from the list
        
        this.RopeSegments.Pop().Destroy();

        // Extend the new top-of stack RopeSegment to catch the player
        this.LastSegment().end = this.player.transform.position;
                
        // Refresh the spring joint
        this.RefreshJoint();

        
    }

    
    public void RefreshJoint()
    {
        // Kill the existing spring joint
        Destroy(this.ropeJoint);

        // Attach a new joint
        this.ropeJoint = player.AddComponent<ConfigurableJoint>();
        this.ropeJoint.autoConfigureConnectedAnchor = false;
        this.ropeJoint.axis = Vector3.left;
        this.ropeJoint.secondaryAxis = Vector3.left;
        this.ropeJoint.configuredInWorldSpace = true;
        this.ropeJoint.anchor = Vector3.zero;
        this.ropeJoint.connectedAnchor = this.LastSegment().anchor;
        this.ropeJoint.xMotion = ConfigurableJointMotion.Limited;
        this.ropeJoint.yMotion = ConfigurableJointMotion.Limited;
        this.ropeJoint.zMotion = ConfigurableJointMotion.Limited;
        this.ropeLimit.limit = this.LastSegment().length;
        this.ropeJoint.linearLimit = this.ropeLimit;
        this.ropeLimit.spring = spring;
        this.ropeLimit.damper = damper;
        this.ropeLimit.spring = spring;
    }

    // Have we retracted very close to the anchor point?
    public bool isRetracted()
    {
        return (this.RopeSegments.Count > 1) && (this.ropeJoint.linearLimit.limit < 0.25f);
    }

    // Do we need to join the ropes? (Unwrap around object)
    public bool isStraight()
    {
        float distance = Vector3.Distance(this.LastSegment().anchor, player.transform.position);
        if ((this.RopeSegments.Count > 1) && (distance > 0.3f))
        {
            float angle = RopeControl2.AngleSigned(this.RopeSegments.ElementAt(1).end - this.RopeSegments.ElementAt(1).anchor,
                this.LastSegment().anchor - player.transform.position, Vector3.back);

          

            return ((angle < 0f && this.isAntiClockwise)
                || (angle > 0f && !this.isAntiClockwise));
        }
        return false;
    }

    //Should we split the rope?
    public Vector3? isSplit(int mask)
    {

        RaycastHit hit;
        Vector3 direction;
        float distance;


        //Look for Splits
        direction = this.LastSegment().anchor - player.transform.position;
        direction.Normalize();
        Ray lookSplit = new Ray(player.transform.position, direction);
        
        distance = Vector3.Distance(player.transform.position, this.LastSegment().anchor) - 0.25f;
        if (distance > 0f)
        {
            
            if (Physics.Raycast(lookSplit, out hit, distance, mask))
            {
                return RopeControl2.FindClosestCornerToEdge(hit.point, hit.transform, this.width);
            }
        }
        return null;
    }
    

    public void Destroy()
    {
        foreach (RopeSegment rs in RopeSegments)
        {
            rs.Destroy();
        }
        Destroy(this.ropeJoint);
        Destroy(this);
    }
}
