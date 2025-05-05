using UnityEngine;

public class ColliderHandler : MonoBehaviour
{
    private TargetController targetController;

    void Start()
    {
        // Находим TargetController на родительском объекте (Cube.005)
        targetController = GetComponentInParent<TargetController>();
        if (targetController == null)
        {
            Debug.LogError(gameObject.name + ": TargetController не найден на родительском объекте! Проверяю иерархию: " + transform.root.gameObject.name);
        }
        else
        {
            Debug.Log(gameObject.name + ": TargetController найден на " + targetController.gameObject.name);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(gameObject.name + ": Столкновение с объектом " + collision.gameObject.name + " с тегом " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Debug.Log(gameObject.name + ": Обнаружен тег Projectile, targetController = " + (targetController != null ? "найден" : "не найден"));
            if (targetController != null)
            {
                Debug.Log(gameObject.name + ": Передаём событие в TargetController!");
                targetController.OnProjectileHit();
            }
        }
    }
}