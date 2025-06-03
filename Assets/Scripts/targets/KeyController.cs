using UnityEngine;

public class KeyController : MonoBehaviour
{
    [SerializeField] private TargetAndColliderController[] targetControllers; // Мишени для опускания
    [SerializeField] private float pressDistance = 0.07f; // Расстояние полного нажатия (уменьшено на 30%)
    private Vector3 initialPosition; // Начальная позиция клавиши
    private bool isPressed = false; // Флаг, чтобы не вызывать MoveDown() повторно
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position; // Сохраняем начальную позицию
        Debug.Log(gameObject.name + " начальная позиция: " + initialPosition);
        Debug.Log(gameObject.name + " количество мишеней: " + (targetControllers != null ? targetControllers.Length : 0));
    }

    void Update()
    {
        float currentDistance = initialPosition.y - transform.position.y; // Расстояние опускания
        Debug.Log(gameObject.name + " текущая позиция: " + transform.position + ", дистанция опускания: " + currentDistance + ", isPressed: " + isPressed);

        if (rb != null)
        {
            if (rb.velocity.magnitude > 5f) // Ограничение скорости
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, 5f);
            }
            if (transform.position.y < initialPosition.y - pressDistance - 0.01f) // Не даём проваливаться ниже
            {
                transform.position = new Vector3(transform.position.x, initialPosition.y - pressDistance, transform.position.z);
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // Сбрасываем скорость по Y
                Debug.Log(gameObject.name + " достиг нижнего предела!");
            }
            if (transform.position.y > initialPosition.y + 0.01f) // Не даём подниматься выше
            {
                transform.position = new Vector3(transform.position.x, initialPosition.y, transform.position.z);
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // Сбрасываем скорость по Y
                Debug.Log(gameObject.name + " достиг верхнего предела!");
            }
        }

        if (currentDistance >= pressDistance && !isPressed)
        {
            Debug.Log(gameObject.name + " полностью нажата!");
            isPressed = true;
            if (targetControllers != null && targetControllers.Length > 0)
            {
                foreach (var controller in targetControllers)
                {
                    if (controller != null)
                    {
                        Debug.Log(gameObject.name + " вызывает MoveDown для " + controller.gameObject.name);
                        controller.MoveDown();
                    }
                    else
                    {
                        Debug.LogWarning(gameObject.name + " обнаружен null в targetControllers!");
                    }
                }
            }
            else
            {
                Debug.LogWarning(gameObject.name + " массив targetControllers пуст или не инициализирован!");
            }
        }
        else if (currentDistance < pressDistance * 0.5f) // Если клавиша поднялась выше половины
        {
            if (isPressed)
            {
                Debug.Log(gameObject.name + " сбрасывает isPressed");
                isPressed = false; // Сбрасываем флаг
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(gameObject.name + " столкнулся с " + collision.gameObject.name);
    }
}