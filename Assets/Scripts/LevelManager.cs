using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private string currentMission;

    void OnGUI()
    {
        GUI.Box(new Rect(40, 40, 600, 25), currentMission);   
    }
}
