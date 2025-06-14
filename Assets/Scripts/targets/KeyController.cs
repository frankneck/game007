using System.Collections;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    public GameController gameController;
    public UIManager uiManager;
    public TargetManager targetManager;

    [SerializeField] private float tickInterval = 1f;
    [SerializeField] private float pressDistance = 0.07f;

    private Vector3 initialPosition;
    private bool isPressed;
    private Rigidbody rb;
    private bool ignoreUpdate;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;

        uiManager?.ClearMessage(0f);
    }

    private void Update()
    {
        float currentDistance = initialPosition.y - transform.position.y;

        if (rb != null)
        {
            if (rb.velocity.magnitude > 5f)
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, 5f);

            float minY = initialPosition.y - pressDistance;
            float maxY = initialPosition.y;

            if (transform.position.y < minY - 0.01f || transform.position.y > maxY + 0.01f)
            {
                float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
                transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            }
        }

        if (currentDistance >= pressDistance && !isPressed && !ignoreUpdate)
            Press();

        if (currentDistance < pressDistance * 0.5f && isPressed)
            isPressed = false;
    }

    public void Press()
    {
        if (isPressed || ignoreUpdate) return;

        isPressed = true;
        ignoreUpdate = true;

        transform.position = new Vector3(transform.position.x, initialPosition.y - pressDistance, transform.position.z);
        rb.velocity = Vector3.zero;

        gameController.PrepareGame();
        isPressed = true;
        ignoreUpdate = false;
    }
}
