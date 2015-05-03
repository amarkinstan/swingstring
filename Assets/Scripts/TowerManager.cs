using UnityEngine;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour
{

   

    public GameObject tower;

    public int numberOfObjects;
    public float recycleOffset;
    public Vector3 minGap, maxGap;
    public GameObject Player;

    private Vector3 nextPosition;
    private Queue<GameObject> objectQueue;



    void Start()
    {
        nextPosition = transform.localPosition;
        
        objectQueue = new Queue<GameObject>(numberOfObjects);
        for (int i = 0; i < numberOfObjects; i++)
        {
            GameObject o = (GameObject)Instantiate(tower, new Vector3(0f, -48f, 0f), Quaternion.identity);
           
                objectQueue.Enqueue(o);
               
        }
        for (int i = 0; i < numberOfObjects; i++)
        {
                        Recycle();
        }
        
    }







    void Update()
    {
        if (Mathf.Abs(objectQueue.Peek().transform.localPosition.x) + recycleOffset < Mathf.Abs(Player.transform.localPosition.x))
        {
            Recycle();
        }
    }

    private void Recycle()
    {

               

        Vector3 position = nextPosition;
        

        GameObject o = objectQueue.Dequeue();

        
        o.transform.localPosition = position;


        objectQueue.Enqueue(o);

        nextPosition += new Vector3(
            Random.Range(minGap.x, maxGap.x),
            Random.Range(minGap.y, maxGap.y),
            Random.Range(minGap.z, maxGap.z));

  
    }
}