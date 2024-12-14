using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;

    public float gravity = -9.8f;
    public float speed = 10f;
    public float height = 10f;

    private void Awake()
    {
        controller = transform.GetComponent<CharacterController>();
    }
    private void Update()
    {
        isGrounded = controller.isGrounded;
    }
    public void ProcessMove(Vector3 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        // Move character
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        // Create gravity
        playerVelocity.y += gravity * Time.deltaTime;

        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2.0f;
        // Use gravity
        controller.Move(playerVelocity * Time.deltaTime);
        //Debug.Log(playerVelocity.y);
    }
    public void Jump()
    {
        if (isGrounded)
        {
            // 2gh
            playerVelocity.y = Mathf.Sqrt(-3.0f * gravity * height); 
        }
    }
}



