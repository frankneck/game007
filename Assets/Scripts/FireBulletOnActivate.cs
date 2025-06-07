using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
public class FireBulletOnActivate : Sounds
{
    [Header("Настройки пули")]
    public GameObject bullet;
    public Transform spawnPoint;
    public float bulletSpeed;
   
    [Header("Разное")]
    public Animator animator;
    public InputActionProperty left;
    public InputActionProperty right;
    public Transform storageEjectPoint;
    public float delayOfShellFalling = 1f;

    [Header("Настройки магазина")]
    public int maxAmmo = 20;
    public GameObject storage;
    public GameObject ammo;
    public Material usedMaterial;
    public AnimationClip storageTakeOutClip;

    private int currentAmmo;
    private bool isEmpty;
    private bool storageDetached;

    void Start()
    {
        XRGrabInteractable grabable = GetComponent<XRGrabInteractable>();
        grabable.activated.AddListener(FireBullet);
        currentAmmo = 10; // для тестов
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

        Debug.Log($"Магазин пустой? {isEmpty}");
    }

    public void FireBullet(ActivateEventArgs arg)
    {
        if (currentAmmo <= 0)
        {
            if (animator != null)
            {
                PlaySound(sounds[2]); // сухой выстрел
                animator.SetTrigger("EMPTY");
            }

            Debug.Log("No ammo!");
        }
        else
        {
            if (animator != null)
            {
                PlaySound(sounds[0]); // выстрел
                animator.SetTrigger("SHOT");
                GameObject spawnedBullet = Instantiate(bullet);
                spawnedBullet.transform.position = spawnPoint.position;
                spawnedBullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * bulletSpeed;
                Debug.Log("Bullet spawned");
                currentAmmo--;

                StartCoroutine(BulletShellFalling());

                Destroy(spawnedBullet, 5);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Storage") && currentAmmo <= 0 && isEmpty)
        {
            Reload();
            Destroy(other.gameObject);
        }
    }


    public void Reload()
    {
        if (animator != null)
        {
            PlaySound(sounds[1]); // сухой выстрел
            animator.SetTrigger("RELOADED");
            storage.SetActive(true);
        }
        currentAmmo = maxAmmo;

        storageDetached = false; // разрешаем снова вытащить магазин
        isEmpty = false;
    }

    public void StorageDetach()
    {
        PlaySound(sounds[3]); // звук вытаскивания
        isEmpty = true;
        currentAmmo = 0;
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

            if (usedMaterial != null)
                newStorage.GetComponent<MeshRenderer>().material = usedMaterial;

            Rigidbody rb = newStorage.AddComponent<Rigidbody>();
        }
        storageDetached = true;
    }

    private IEnumerator BulletShellFalling()
    {
        yield return new WaitForSeconds(delayOfShellFalling);

        PlaySound(sounds[4]); // звук падения гильзы
    }
}

