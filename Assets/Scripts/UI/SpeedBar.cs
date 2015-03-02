using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpeedBar : MonoBehaviour
{
    //The player object
    public GameObject player;

    //are we tracking the player or the darkness?
    public bool isPlayer;



    private Color colour;

    //what is the position of hte bar?
    public bool Bot;
    public bool Top;
    public bool Left;
    public bool Right;

    
    //seped of the thing we are tracking
    private float speed;



    // Update is called once per frame
    void Update()
    {


        //Add the spped at this frame to the list

        if (isPlayer)
        {
            speed = GlobalStuff.AveragePlayerSpeed;

        }
        else
        {

            speed = GlobalStuff.DarkSpeed;
        }




        //set the size of the bar based on speed speed (based on 60 being top speed)

        if (Bot)
        {
            transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, (speed / (9.8f)), 0.5f), transform.localScale.y, 1f);
        }
        if (Top)
        {
            float size = (speed / (9.8f)) - 3.125f;
            if (size < 0f)
            {
                size = 0f;
            }
            transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, size, 0.5f), transform.localScale.y, 1f);
        }
        if (Right)
        {
            float size = (speed / (5.4f)) - 3.623f;

            if (size < 0f)
            {
                size = 0f;
            }

            transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(transform.localScale.y, size, 0.5f), 1f);
        }
        if (Left)
        {
            float size = ((speed / (5.4f)) - 9.375f) * 1.1428f;

            if (size < 0f)
            {
                size = 0f;
            }

            transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(transform.localScale.y, size, 0.5f), 1f);
        }



        //set teh colour of the bar based on the last block attatched to.
        if (isPlayer)
        {
            colour = GlobalStuff.LastColour;
            colour = new Color(colour.r, colour.g, colour.b, 1f);
            colour = Color.Lerp(colour, Color.black, 0.6f);
            guiTexture.color = colour;
        }

    }
}
