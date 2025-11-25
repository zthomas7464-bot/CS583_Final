using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    //Create way to easily change movement speed
    [Header("Movement")]
    public float moveSpeed = 5f;

    //Crete way to easily change gravity and jump behavior
    [Header("Gravity & Jumping")]
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        //Check if player is on the ground
        bool onGround = controller.isGrounded;

        //Stop falling when hitting the ground
        if (onGround && velocity.y < 0)
            velocity.y = -2f; //0 keeps causing a jitter IDK this is what reddit said
        
        //For movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Convert input to direction in game & apply it
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        //Yump
        if (Input.GetButtonDown("Jump") && onGround)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        //Apply gravity & vertical
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
