using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private RaycastHit hit;

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag("Target")) // для мишеней отдельная обработка - скрипт мишени
            Destroy(gameObject);
    }
}
