using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : Sounds
{
    [SerializeField] private Animator animator;

    public void OpenDoor()
    {
        PlaySound(sounds[0]); // звук скрипа двери
        StartCoroutine(DelayedOpening(sounds[0].length / 2));
    }

    private IEnumerator DelayedOpening(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetTrigger("IsOpened"); 
    }
}
