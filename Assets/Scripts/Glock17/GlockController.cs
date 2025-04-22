using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GlockController : MonoBehaviour
{
    [Header("Shooting Settings")]
    public int maxAmmo = 17; // Максимум патронов в магазине
    public float fireRate = 0.1f; // Скорострельность
    public Transform firePoint; // Точка, откуда вылетает пуля
    public GameObject bulletPrefab; // Префаб пули

    [Header("Audio Settings")]
    public AudioClip fireSound; // Звук выстрела
    public AudioClip emptyClickSound; // Звук пустого курка
    private AudioSource audioSource;

    [Header("Animation Settings")]
    public Animator animator; // Ссылка на Animator

    [Header("XR Settings")]
    public ActionBasedController controller; // Ссылка на VR-контроллер
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable; // Компонент для взятия

    private int currentAmmo;
    private float nextTimeToFire = 0f;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void Update()
    {
        if (isReloading) return;

        // Проверяем, держат ли пистолет
        if (grabInteractable.isSelected && controller != null)
        {
            // Стрельба при нажатии на триггер
            float triggerValue = controller.activateAction.action.ReadValue<float>();
            Debug.Log($"Trigger Value (Activate): {triggerValue}"); // Отладка

            if (triggerValue > 0.5f && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + fireRate;
                Shoot();
            }

            // Перезарядка при нажатии на кнопку (например, "A" на Oculus)
            if (controller.selectAction.action.ReadValue<float>() > 0.5f && currentAmmo < maxAmmo)
            {
                StartReloading();
            }
        }
    }

    void Shoot()
    {
        if (currentAmmo > 0)
        {
            animator.SetTrigger("SHOT");
            audioSource.PlayOneShot(fireSound);

            if (bulletPrefab != null && firePoint != null)
            {
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            }

            currentAmmo--;
            Debug.Log("Патроны: " + currentAmmo);
        }
        else
        {
            audioSource.PlayOneShot(emptyClickSound);
            Debug.Log("Магазин пуст! Нажми кнопку для перезарядки.");
        }
    }

    void StartReloading()
    {
        isReloading = true;
        animator.SetTrigger("Reload");
        Invoke("FinishReloading", 2f);
    }

    void FinishReloading()
    {
        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Перезарядка завершена! Патроны: " + currentAmmo);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        controller = args.interactorObject.transform.GetComponent<ActionBasedController>();
        if (controller == null)
        {
            Debug.LogError("Контроллер не найден!");
        }
        else
        {
            Debug.Log("Пистолет взят в руки! Контроллер: " + controller.name);
        }
    }

    void OnRelease(SelectExitEventArgs args)
    {
        if (!args.isCanceled)
        {
            controller = null;
            Debug.Log("Пистолет отпущен!");
        }
    }
}