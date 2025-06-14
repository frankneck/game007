using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void OpenDoor()
    {
        AudioManager.instance.PlayOneShot("DoorOpened", transform.position); // звук скрипа двери

        float length = AudioManager.instance.GetClipLength("DoorOpened");
        StartCoroutine(DelayedOpening(length / 2));
    }

    private IEnumerator DelayedOpening(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetTrigger("IsOpened"); 
    }
}
