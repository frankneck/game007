using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
public class FireBulletOnActivate : MonoBehaviour
{
    public GameObject bullet;
    public Transform spawnPoint;
    public float bulletSpeed;
    public Animator animator;
    public GameObject ammo;
    public InputActionProperty left;
    public InputActionProperty right;
    public Transform storageEjectPoint;

    public GameObject storage;
    private bool storageDetached;

    public int maxAmmo = 20;
    public AnimationClip storageTakeOutClip;
    private int currentAmmo = 0;

    void Start()
    {
        XRGrabInteractable grabable = GetComponent<XRGrabInteractable>();
        grabable.activated.AddListener(FireBullet);
        currentAmmo = 0;
    }

    void Update()
    {
        if (!storageDetached)
        {
            if (left.action.WasPressedThisFrame())
            {
                StorageDetach();
            }
            else if (right.action.WasPressedThisFrame())
            {
                StorageDetach();
            }
        }
    }

    public void FireBullet(ActivateEventArgs arg)
    {
        if (currentAmmo <= 0)
        {
            if (animator != null)
            {
                animator.SetTrigger("EMPTY");
            }

            Debug.Log("No ammo!");
        }
        else
        {
            if (animator != null)
            {
                animator.SetTrigger("SHOT");
                GameObject spawnedBullet = Instantiate(bullet);
                spawnedBullet.transform.position = spawnPoint.position;
                spawnedBullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * bulletSpeed;
                Debug.Log("Bullet spawned");
                currentAmmo--;

                Destroy(spawnedBullet, 5);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Storage") && currentAmmo <= 0)
        {
            Reload();
            Destroy(other.gameObject);
        }
    }


    public void Reload()
    {
        if (animator != null)
        {
            animator.SetTrigger("RELOADED");
            Debug.Log("Преждевременная перезарядка");
            storage.SetActive(true);
        }
        currentAmmo = maxAmmo;

        storageDetached = false; // разрешаем снова вытащить магазин
    }

    public void StorageDetach()
    {
        animator.SetTrigger("StorageTakeOut");
        StartCoroutine(DisableStorage());
    }

    private IEnumerator DisableStorage()
    {
        yield return new WaitForSeconds(storageTakeOutClip.length);
        if (storage != null)
        {
            storage.SetActive(false);
            GameObject newStorage = GameObject.Instantiate(ammo, storageEjectPoint.position, storageEjectPoint.rotation);
            newStorage.tag = "Used";
            Rigidbody rb = newStorage.AddComponent<Rigidbody>();
        }
        storageDetached = true;
    }
}

