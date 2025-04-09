using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : Interactable
{
    [SerializeField] private GameObject door;

    protected override void Interact()
    {
        door.GetComponent<Animator>().SetBool("IsOpen", true);
    }
}
