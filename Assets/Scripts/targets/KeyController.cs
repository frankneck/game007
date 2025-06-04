using UnityEngine;

public class KeyController : MonoBehaviour
{
    [SerializeField] private TargetAndColliderController[] targetControllers; // ������ ��� ���������
    [SerializeField] private float pressDistance = 0.07f; // ���������� ������� ������� (��������� �� 30%)
    private Vector3 initialPosition; // ��������� ������� �������
    private bool isPressed = false; // ����, ����� �� �������� MoveDown() ��������
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position; // ��������� ��������� �������
        // Debug.Log(gameObject.name + " ��������� �������: " + initialPosition);
        // Debug.Log(gameObject.name + " ���������� �������: " + (targetControllers != null ? targetControllers.Length : 0));
    }

    void Update()
    {
        float currentDistance = initialPosition.y - transform.position.y; // ���������� ���������
        // Debug.Log(gameObject.name + " ������� �������: " + transform.position + ", ��������� ���������: " + currentDistance + ", isPressed: " + isPressed);

        if (rb != null)
        {
            if (rb.velocity.magnitude > 5f) // ����������� ��������
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, 5f);
            }
            if (transform.position.y < initialPosition.y - pressDistance - 0.01f) // �� ��� ������������� ����
            {
                transform.position = new Vector3(transform.position.x, initialPosition.y - pressDistance, transform.position.z);
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // ���������� �������� �� Y
                // Debug.Log(gameObject.name + " ������ ������� �������!");
            }
            if (transform.position.y > initialPosition.y + 0.01f) // �� ��� ����������� ����
            {
                transform.position = new Vector3(transform.position.x, initialPosition.y, transform.position.z);
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // ���������� �������� �� Y
                // Debug.Log(gameObject.name + " ������ �������� �������!");
            }
        }

        if (currentDistance >= pressDistance && !isPressed)
        {
            // Debug.Log(gameObject.name + " ��������� ������!");
            isPressed = true;
            if (targetControllers != null && targetControllers.Length > 0)
            {
                foreach (var controller in targetControllers)
                {
                    if (controller != null)
                    {
                        // Debug.Log(gameObject.name + " �������� MoveDown ��� " + controller.gameObject.name);
                        controller.MoveDown();
                    }
                    else
                    {
                        // Debug.LogWarning(gameObject.name + " ��������� null � targetControllers!");
                    }
                }
            }
            else
            {
                // Debug.LogWarning(gameObject.name + " ������ targetControllers ���� ��� �� ���������������!");
            }
        }
        else if (currentDistance < pressDistance * 0.5f) // ���� ������� ��������� ���� ��������
        {
            if (isPressed)
            {
                // Debug.Log(gameObject.name + " ���������� isPressed");
                isPressed = false; // ���������� ����
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Debug.Log(gameObject.name + " ���������� � " + collision.gameObject.name);
    }
}