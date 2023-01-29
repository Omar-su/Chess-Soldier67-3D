using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform orientation;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jump = 1f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float playerHeight = 1f;

    Vector3 velocity;
    bool isGrounded;
    private bool disabled = false;

    // Update is called once per frame
    void Update()
    {
        if(!disabled) {
            isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, groundMask);

            if(isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = orientation.right * x + orientation.forward * z;

            controller.Move(move * speed * Time.deltaTime);

            if(Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jump * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }
    }

    public void SetDisabled(bool b){
        disabled = b;
    }
}