using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public string labelText = "text";
    private void OnGUI()
    {
        GUI.Box(new Rect(20, 20, 150, 25), $"Player Health: {labelText}");
    }
}
