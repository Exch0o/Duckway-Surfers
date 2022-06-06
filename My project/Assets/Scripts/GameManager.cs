using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // This script is for managing all the different stages and scenes of the game, for example losing scene.

    // This public function will be called by the DuckCollider.cs script when the duck hits something and loses.
    public void EndGame()
    {
        print("You lost!");
    }
}
