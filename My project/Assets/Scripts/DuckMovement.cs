﻿using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

public class DuckMovement : MonoBehaviour
{
    // This script is for moving the player

    // References the character controller that we will be using to control the character.
    public CharacterController controller;

    // References the animator that we will be using to control the character's animation (for example when to activate the jump animation)
    public Animator animator;

    // A float for controlling the speed of the player at running
    public float speed;
    // A float for controlling the gravity amount applied to the player
    public float gravity;

    // A float for controlling the jump height of the player
    public float jumpHeight;

    // Referencing to the groundCheck game object that we're using to detect whenever the player is on the ground (meaing that it could jump)
    // This groundCheck game object is put at the very bottom of the duck
    public Transform groundCheck;

    // A float for storing the groundDistance
    float groundDistance = 1f;

    // A LayerMask variable for seeing which LayerMask will be the "groundMask", meaning the LayerMask that the player can jump on
    public LayerMask groundMask;

    // float for storing the Original stepOffSet when the player was on the ground, the stepOffSet will be changed when the player jumps (for avoiding jittering issues)
    // Kinda complicated thing to know, it's important but not that important, just like a patch for a bug Unity has.
    float OGstepOffSet;

    // Variable for storing the current velocity
    Vector3 velocity;

    // Bool for checking if the player is standing on ground or in air
    bool isGrounded;

    // Benchmark floats for storing the leftPosition/middlePosition/rightPosition, there are 3 routes that the player can switch to, in case the positions are switched to somewhere else, the moving system is made adjustable dependent to these three variables
    public float leftPosition;
    public float middlePosition;
    public float rightPosition;

    // A string containing the move command, for example "RM" would mean moving Right to Middle, "ML" would mean moving from Middle to Left
    string move_CMD = "";

    // A float for controlling how fast the player shifts to another route
    public float shiftSpeed;

    // String for storing what the currentPath is, for example "Middle"/"Right"/"Left"
    string currentPath;

    // This start function gets called at the start by default, will set the OGstepOffSet to the controller.stepOffset, so we won't lose the original value later on when we change the controller.stepOffSet.
    void Start()
    {
        OGstepOffSet = controller.stepOffset;
    }

    // This update function gets called every frame
    void Update()
    {
        //Path Switching (Inputting Part)

        // if the player pressed left mouse button, will move left.
        if (Input.GetButtonDown("Fire1"))
        {
            // if at right currently, will set move_CMD to Right to Middle, moving left.
            if (currentPath == "Right")
            {
                if (transform.position.x > middlePosition)
                {
                    move_CMD = "RM";
                }
            }
            // if at middle currently, will set move_CMD to Middle to Left, moving left.
            if (currentPath == "Middle")
            {
                if (transform.position.x > leftPosition)
                {
                    move_CMD = "ML";
                }
            }
        }
        
        // if the player pressed right mouse button, will move right.
        if (Input.GetButtonDown("Fire2"))
        {
            // if at left currently, will set move_CMD to Left to Middle, moving right.
            if (currentPath == "Left")
            {
                if (transform.position.x < middlePosition)
                {
                    move_CMD = "LM";
                }
            }
            // if at middle currently, will set move_CMD to Middle to Right, moving right.
            if (currentPath == "Middle")
            {
                if (transform.position.x < rightPosition)
                {
                    move_CMD = "MR";
                }
            }
        }

        //Path Switching (Checking Part)
        // Checks through which path the duck is at depending on its x position, if the x position matches the benchmark required for each route.
        // Every benchmark has a 0.2f fine-tune because the player cannot smoothly arrive at the exact position.
        if (transform.position.x > rightPosition - 0.2f && transform.position.x < rightPosition + 0.2f)
        {
            currentPath = "Right";
        }
        else if(transform.position.x > middlePosition - 0.2f && transform.position.x < middlePosition + 0.2f)
        {
            currentPath = "Middle";
        }
        else if(transform.position.x > leftPosition - 0.2f && transform.position.x < leftPosition + 0.2f)
        {
            currentPath = "Left";
        }

        //Path Switching (Moving Part)
        // Bunch of if statements executing the move_CMD that was given.
        if(currentPath != "Middle" && move_CMD == "RM")
        {
            Vector3 _move = new Vector3(-1, 0, 0);
            controller.Move(_move * Time.deltaTime * shiftSpeed);
        }
        if (currentPath != "Left" && move_CMD == "ML")
        {
            Vector3 _move = new Vector3(-1, 0, 0);
            controller.Move(_move * Time.deltaTime * shiftSpeed);
        }
        if (currentPath != "Middle" && move_CMD == "LM")
        {
            Vector3 _move = new Vector3(1, 0, 0);
            controller.Move(_move * Time.deltaTime * shiftSpeed);
        }
        if (currentPath != "Right" && move_CMD == "MR")
        {
            Vector3 _move = new Vector3(1, 0, 0);
            controller.Move(_move * Time.deltaTime * shiftSpeed);
        }

        //Running forward constantly
        Vector3 move = transform.forward * speed;
        controller.Move(move * speed * Time.deltaTime);

        //Jumping
        //checks if the player is on ground by checking if the groundCheck (at the bottom of player) is on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // if is ground (meaning it's running right now), it will reset the stepOffSet back to what it was
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;

            controller.stepOffset = OGstepOffSet;

            // Sets the animator "Run" float to 1, meaning it will do the run animation.
            // Vice versa, setting the "Jump" float to 0, meaning it will not do the jump animation
            animator.SetFloat("Run", 1);
            animator.SetFloat("Jump", 0);
        }

        // if space bar is pressed and the player is on the ground, will jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Calls the jumping function
            StartCoroutine(Jumping());

            // Sets the animator "Run" float to 0, meaning it will not do the run animation.
            // Vice versa, setting the "Jump" float to 1, meaning it will do the jump animation
            animator.SetFloat("Run", 0);
            animator.SetFloat("Jump", 1);
        }


        // Applies the gravity force to the player
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }
    // IEnumerator function for jumping, this is in IEnumerator because the jumping needs to be delayed, and it's only viable through an IEnumerator
    IEnumerator Jumping()
    {
        // this is for waiting for 0.4 seconds then jump, this is because the animation has delay, if the jumping doesn't delays, the animation will start in mid-air
        yield return new WaitForSeconds(0.00001f);

        // Sets the stepOffset of the player to 2 to avoid jittering issues when jumping close to an object
        controller.stepOffset = 2;

        // Applies the forces to jump
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
    
}