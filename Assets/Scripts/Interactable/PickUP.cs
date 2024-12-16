using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUP : Interactable
{
    public GameObject collectCube;

    protected override void Interact()
    {
        Debug.Log(collectCube);
        Destroy(collectCube);
        Debug.Log("Destroyed");
    }
}
