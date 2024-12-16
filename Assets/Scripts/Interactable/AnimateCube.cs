using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateCube : Interactable
{
    Animator animator;
    private bool isSpin;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    protected override void Interact()
    {
        isSpin = !isSpin;
        animator.SetTrigger("PlaySpin");
        Debug.Log("Spined cube");
    }
}
