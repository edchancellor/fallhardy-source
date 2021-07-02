using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public float runSpeed = 0.9f;
    public bool movementAllowed = true;
    float horizontalMove = 0f;
    bool jump = false;
    int jumpAllowance;
    public int noOfSpears;
    bool throwSpear = false;
    int jumpLenience = 0;
    public AudioClip throwRejectSound;

    private AudioSource audio;
    
    void Start()
    {
        resetAirJumps();
        audio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float rawMovement = Input.GetAxisRaw("Horizontal");
        gameObject.GetComponent<Animator>().SetFloat("Running", rawMovement); 
        horizontalMove = rawMovement * runSpeed;

        if (jumpLenience > 0 && controller.m_Grounded == true)
        {
            jumpLenience = 0;
            jump = true; // have a seperate variable for jumplenience which gets set to the character controller
            Debug.Log("jumplenience");
        }

        else if (Input.GetButtonDown("Jump") && jumpLenience == 0)
        {
            jump = true;
            Debug.Log("normaljump");
            if (controller.m_Grounded == false)
            {
                jumpLenience = 10;
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (noOfSpears > 0)
            {
                throwSpear = true;
            }
            else
            {
                audio.PlayOneShot(throwRejectSound, 0.04f);
            }
        }
    }

    void FixedUpdate()
    {
        // Time.fixedDeltaTime ensures that we only move when this function is called
        if (movementAllowed)
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, jump, jumpAllowance, throwSpear);
        }
        else
        {
            controller.Move(0f * Time.fixedDeltaTime, false, 0, false);
        }
        jump = false;
        throwSpear = false;
        if (jumpAllowance > 0)
        {
            jumpAllowance --;
        }
        if (jumpLenience > 0)
        {
            jumpLenience--;
        }

    }

    public void resetAirJumps()
    {
        jumpAllowance = 5;
    }
}
