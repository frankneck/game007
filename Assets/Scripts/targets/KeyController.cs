using System.Collections;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    public AudioSource tickAudioSource;
    public AudioSource hornAudioSource;
    public GameBehaviour gameBehaviour;
    [SerializeField] private float tickInterval = 1f;
    [SerializeField] private TargetAndColliderController[] targetControllers;
    [SerializeField] private float pressDistance = 0.07f;
    private Vector3 initialPosition;
    private bool isPressed;
    private Rigidbody rb;
    private bool ignoreUpdate;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
    }

    private void Update()
    {
        float currentDistance = initialPosition.y - transform.position.y;

        if (rb != null)
        {
            if (rb.velocity.magnitude > 5f)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, 5f);
            }

            if (transform.position.y < initialPosition.y - pressDistance - 0.01f)
            {
                transform.position = new Vector3(transform.position.x, initialPosition.y - pressDistance, transform.position.z);
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            }

            if (transform.position.y > initialPosition.y + 0.01f)
            {
                transform.position = new Vector3(transform.position.x, initialPosition.y, transform.position.z);
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            }
        }

        if (currentDistance >= pressDistance && !isPressed && !ignoreUpdate)
        {
            Press();
        }

        if (currentDistance < pressDistance * 0.5f && isPressed)
        {
            isPressed = false;
        }
    }

    public void Press()
    {
        if (isPressed || ignoreUpdate) return;

        isPressed = true;
        ignoreUpdate = true;

        transform.position = new Vector3(transform.position.x, initialPosition.y - pressDistance, transform.position.z);
        rb.velocity = Vector3.zero;

        StartCoroutine(DelayedTrigger(4f));
    }

    private IEnumerator DelayedTrigger(float delay)
    {
        float elapsed = 0f;

        while (elapsed < delay)
        {
            if (elapsed < 3f) // первые 3 секунды – тик
            {
                if (tickAudioSource != null)
                {
                    tickAudioSource.Play();
                }
            }
            else if (Mathf.Approximately(elapsed, 3f)) // на 4-й секунде – гудок
            {
                if (tickAudioSource != null)
                {
                    hornAudioSource.Play();
                }
            }

            yield return new WaitForSeconds(tickInterval);
            elapsed += tickInterval;
        }

        targetControllers[0]?.MoveDown();
        gameBehaviour?.StartGame();

        isPressed = true;
        ignoreUpdate = false;
    }
}
