using UnityEngine;
using System.Collections.Generic;

public class Gravity : MonoBehaviour
{

    public float starMass;
    public float range;

    private GameObject Player;
    private bool WasPlayerInRangeLastFrame;

    void Start()
    {

        Player = GameObject.Find("Player");

    }



    void FixedUpdate()
    {

        float distance = Vector3.Distance(Player.transform.position, transform.position);

        //if we are leaving the hole
        if (WasPlayerInRangeLastFrame && distance > range)
        {
            Time.timeScale = 1.0f;
        }
        
        //if we are in teh hole
        if (distance < range)
        {

            WasPlayerInRangeLastFrame = true;

            Vector3 offset = transform.position - Player.transform.position;

            Vector3 totalForce = offset / offset.sqrMagnitude * starMass * 4f;

            Player.rigidbody.AddForce(totalForce);
            
            Time.timeScale = (distance / range);

        }
        else
        {
            WasPlayerInRangeLastFrame = false;
        }

    }
}