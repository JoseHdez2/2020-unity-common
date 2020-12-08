using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //TODO set all the floor to 'Ground' layer

    public CharacterController controller;

    public float speed = 6f;
    public float dashCoef = 2f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;    

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 move, velocity;
    bool isGrounded;
    [SerializeField] private bool enableGravity = true;
    private bool isDashing;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        move = transform.right * x + transform.forward * z;
        move.Normalize();
        isDashing = Input.GetButton("Fire3");
        if(isDashing){
            move *= dashCoef;
        }

        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        if(enableGravity){
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    public bool IsWalking() => isGrounded && move.magnitude > 0;
    public float MoveMagnitude() => move.magnitude;
}
