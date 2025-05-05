using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Button3DController : MonoBehaviour
{
    [SerializeField]
    private TargetAndColliderController[] targetControllers; // Массив мишеней

    void Start()
    {
        if (targetControllers == null || targetControllers.Length == 0)
        {
            Debug.LogError("Не указаны мишени (TargetAndColliderController) на объекте: " + gameObject.name);
            targetControllers = FindObjectsOfType<TargetAndColliderController>();
            if (targetControllers.Length == 0)
            {
                Debug.LogError("TargetAndColliderController не найдены в сцене!");
            }
            else
            {
                Debug.Log("Автоматически найдено " + targetControllers.Length + " мишеней.");
            }
        }
        else
        {
            Debug.Log("Найдено " + targetControllers.Length + " мишеней для объекта: " + gameObject.name);
        }

        // Подписываемся на событие нажатия через XR
        var interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnSelect);
            interactable.hoverEntered.AddListener(OnHoverEntered); // Убедимся, что Hover подписан
            interactable.hoverExited.AddListener(OnHoverExited);   // Добавляем отладку для выхода луча
            Debug.Log("XRSimpleInteractable найден и настроен на объекте: " + gameObject.name);
        }
        else
        {
            Debug.LogError("XRSimpleInteractable не найден на кнопке: " + gameObject.name + "! Проверьте, добавлен ли компонент.");
        }
    }

    public void OnHoverEntered(HoverEnterEventArgs args)
    {
        Debug.Log("Луч наведён на 3D-кнопку: " + gameObject.name + ", источник: " + (args.interactorObject != null ? args.interactorObject.transform.name : "Неизвестно"));
    }

    public void OnHoverExited(HoverExitEventArgs args)
    {
        Debug.Log("Луч ушёл с 3D-кнопки: " + gameObject.name + ", источник: " + (args.interactorObject != null ? args.interactorObject.transform.name : "Неизвестно"));
    }

    private void OnSelect(SelectEnterEventArgs args)
    {
        Debug.Log("3D-кнопка нажата на объекте: " + gameObject.name + ", источник: " + (args.interactorObject != null ? args.interactorObject.transform.name : "Неизвестно"));
        if (targetControllers != null)
        {
            Debug.Log("Обрабатываем " + targetControllers.Length + " мишеней...");
            foreach (var target in targetControllers)
            {
                if (target != null)
                {
                    Debug.Log("Вызываем MoveDown для мишени: " + target.gameObject.name);
                    target.MoveDown();
                }
                else
                {
                    Debug.LogWarning("Одна из мишеней в массиве targetControllers равна null!");
                }
            }
        }
        else
        {
            Debug.LogError("targetControllers равен null! Проверьте массив мишеней.");
        }
    }

    public void TriggerFromUI()
    {
        Debug.Log("Нажата UI-кнопка на объекте: " + gameObject.name + ", выполняем то же самое, что и для 3D-кнопки");
        if (targetControllers != null)
        {
            Debug.Log("Обрабатываем " + targetControllers.Length + " мишеней из TriggerFromUI...");
            foreach (var target in targetControllers)
            {
                if (target != null)
                {
                    Debug.Log("Вызываем MoveDown для мишени из UI: " + target.gameObject.name);
                    target.MoveDown();
                }
                else
                {
                    Debug.LogWarning("Одна из мишеней в массиве targetControllers равна null в TriggerFromUI!");
                }
            }
        }
        else
        {
            Debug.LogError("targetControllers равен null в TriggerFromUI! Проверьте массив мишеней.");
        }
    }
}