using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : Interactable
{
    [SerializeField] private Rigidbody rb;

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Interact()
    {
        rb.isKinematic = false;
        Destroy(this.GetComponent<Doors>());
    }
}
