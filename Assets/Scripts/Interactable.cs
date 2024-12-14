using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // message displayed to player when looking at an interactable
    public string message;
    // this func will be called from our player
    public void BaseInteract()
    {
        Interact();
    }
    protected virtual void Interact()
    {
        // we won't have any code written in this func
        // this is a template func to be overriden by our subclasses
    }
}
