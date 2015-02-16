using UnityEngine;
using System.Collections.Generic;

public class Gravity : MonoBehaviour
{
    
    public float starMass;
    public float range;

    private GameObject Player;

    void Start()
    {

        Player = GameObject.Find("Player");

    }

    void FixedUpdate()
    {
        
        

        if (Vector3.Distance(Player.transform.position,transform.position) < range)
        {
           
                
                    
                    Vector3 offset = transform.position - Player.transform.position;
                    Vector3 totalForce = offset / offset.sqrMagnitude * starMass * 4f;
                    //	if (totalForce.magnitude + rb.velocity.magnitude < playerController.maxVel){
                    Player.rigidbody.AddForce(totalForce);
                     float distance = Vector3.Distance(transform.position, Player.transform.position);
                    //	}
                    Time.timeScale = (distance / range);
                
            
        }
        else
        {
            Time.timeScale = 1.0f;

        }
    }
}