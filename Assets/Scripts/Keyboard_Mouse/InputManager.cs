using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public PlayerInput.OnFootActions onFoot; 
    private PlayerInput playerInput;
    private PlayerMotor playerMotor; 
    private PlayerLook playerLook;
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput(); // Создаем в менеджере новый экземпляр
        onFoot = playerInput.OnFoot; // 
        playerMotor = GetComponent<PlayerMotor>();
        playerLook = GetComponent<PlayerLook>();
        onFoot.Jump.performed += ctx => playerMotor.Jump();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // tell the playerMotor to move using the value from our movement action.
        playerMotor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        playerLook.ProcessLook(onFoot.Look.ReadValue<Vector2>());     
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}
