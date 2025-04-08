using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.GetComponent<Rigidbody>();

        if (rb != null && !rb.isKinematic)
        {
            Vector3 force = hit.moveDirection;
            rb.AddForce(force, ForceMode.Impulse);
            Debug.Log("HIT!");
        }
    }
}
