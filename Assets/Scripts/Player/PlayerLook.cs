using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    [Header("Sensitivity")]
    public float xSensitivity = 30f;
    public float ySensitivity = 30f;
    private float xRotation = 0f;
    
    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;
        // Calculate camera rotation for looking up and down
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        // Use value
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // Rotate player to look right and left
        transform.Rotate(Vector2.up * (mouseX * Time.deltaTime) * xSensitivity);
    }

}
