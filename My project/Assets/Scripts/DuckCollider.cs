using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckCollider : MonoBehaviour
{
    // This script is for checking if the duck hits anything, and runs codes based on the thing that he touched.

    // This function gets called whenever the duck collider hits something.
    // ControllerColliderHit is a type of variable that stores the information of the object being hit by the duck, the name I gave it is colObj (Collided Object)
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
            FindObjectOfType<GameManager>().EndGame();
        }

        ///This print statement can be a comment or an actual code without the //, it is for printing out the tag of the object that we collided with
        //print(colObj.gameObject.tag);
        
    }
}
