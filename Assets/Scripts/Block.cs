using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Block : MonoBehaviour {

    public int numVerts;
    public float minVertDistance;
    public float maxVertDistance;
    public float maxLengthDelta;
    public float minLengthDelta;

    public PhysicMaterial pMat;

    public List<Vector3> outerPoints;

    // Use this for initialization
    void Start()
    {

        //test code yo

        List<Vector3> vectorList = GenerateMesh.MakeShape(Vector3.zero, numVerts, minVertDistance, maxVertDistance, maxLengthDelta, minLengthDelta);

        outerPoints = GlobalStuff.DeepClone<List<Vector3>>(vectorList);

        outerPoints = GenerateMesh.RemoveConcave(outerPoints, Vector3.zero);
        
        //outerPoints = vectorList.Select(ele => transform.TransformPoint(ele)).ToList();
        
        Mesh blockMesh = GenerateMesh.CreateMesh(vectorList, 2f);
        
        gameObject.AddComponent("MeshRenderer");
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material.color = Color.blue;

        gameObject.AddComponent("MeshFilter");
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        meshFilter.mesh = blockMesh;

        gameObject.AddComponent("MeshCollider");
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = blockMesh;
        meshCollider.smoothSphereCollisions = true;
        meshCollider.material = pMat;

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
