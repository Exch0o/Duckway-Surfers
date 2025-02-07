﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DuckCollider : MonoBehaviour
{
    // This script is for checking if the duck hits anything, and runs codes based on the thing that he touched.

    // This function gets called whenever the duck collider hits something.
    // ControllerColliderHit is a type of variable that stores the information of the object being hit by the duck, the name I gave it is colObj (Collided Object)

    // Reference to the DuckMovement.cs script
    public DuckMovement movement;

    float coinNumber = 0;

    public Text coinDisplay;

    public Text finalCoinDisplay;
    private void Update()
    {
        coinDisplay.text = coinNumber.ToString();
        finalCoinDisplay.text = coinNumber.ToString();
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Coin")
        {
            coinNumber += 1;
            Destroy(col.gameObject);
        }
    }
    void OnControllerColliderHit(ControllerColliderHit colObj)
    {

        // This tag is for checking the tag of the collided object, seeing if it was tagged by me as "LoseTrigger" (if so, the player loses because he touched something that will make he lose)
        tag = colObj.gameObject.tag;
        
        // If it wasn't tagged by us, it will just return
        if (tag == "Untagged")
        {
            return;
        }

        // If the tag was "LoseTrigger", meaning it's something that you shouldn't have touched, you will lose.
        if(tag == "LoseTrigger")
        {
            movement.enabled = false;
            FindObjectOfType<GameManager>().EndGame();
        }

        ///This print statement can be a comment or an actual code without the //, it is for printing out the tag of the object that we collided with
        //print(colObj.gameObject.tag);
        
    }
}
