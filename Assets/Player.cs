using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    Animator anim;
    Rigidbody rb;
    float rotateSpeed;
    Vector3 movement;
    float speed = 25;
    bool isGrounded;
    bool isJumping;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        
    }

    private void Update()
    {

        movement = new Vector3(-Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical"));
        rb.AddForce(movement*speed);

        DoRun();
        DoJump();
        DoLand();

    }

    // Update is called once per frame
    void LateUpdate()
    {



    }


    private void FixedUpdate()
    {
        
    }

    void DoJump()
    {

        // check for jump
        if (Input.GetKey(KeyCode.LeftAlt) && (isGrounded == true))
        {
            anim.SetBool("jump", true);
            isJumping = true;

            // give the player a positive velocity in the y axis, and preserve the x velocity
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 5.5f, 0);
        }
    }

    void DoLand()
    {
        // check for player landing

        if (isJumping && isGrounded && (rb.linearVelocity.y <= 0))
        {
            print("landed!");
            // player was jumping and has now hit the ground
            isJumping = false;
            anim.SetBool("jump", false);
        }
    }

    void DoRun()
    {
        if (movement.magnitude > 0.1f)
        {
            anim.SetBool("run", true);
            transform.rotation = Quaternion.LookRotation(movement);
        }
        else
        {
            anim.SetBool("run", false);
        }
    }


private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
        //print("isgrounded");
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }



}
