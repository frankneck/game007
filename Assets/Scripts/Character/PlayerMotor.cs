using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVeloctity;
    private bool isGrounded;
    
    public float gravity = -9.8f;
    public float speed = 5f;
    public float jumpHeight = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
    } 

    public void ProcessMove(Vector3 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVeloctity.y += gravity * Time.deltaTime;
        
        if (isGrounded && playerVeloctity.y < 0)
            playerVeloctity.y = -2f;
        
        controller.Move(playerVeloctity * Time.deltaTime);
        Debug.Log(playerVeloctity.y);
    }
    public void Jump()
    {
        
        if (isGrounded)
        {
            playerVeloctity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity); // v * v = 2 * g * h
        }
    }

}
    