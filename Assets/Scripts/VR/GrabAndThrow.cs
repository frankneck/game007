using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.XR;

[RequireComponent(typeof(Rigidbody))]
public class GrabAndThrow : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor _interactor;

    [Header("Настройки объекта")]
    [SerializeField] private float objectMass = 5f;
    [SerializeField] private float airResistance = 0.98f;
    [SerializeField] private float velocitySmoothing = 0.5f;
    [SerializeField] private int velocityBufferSize = 5;

    [Header("Настройки броска")]
    [SerializeField] private float throwForceMultiplier = 0.5f;
    [SerializeField] private float maxThrowSpeed = 5f;
    [SerializeField] private float minThrowSpeed = 0.1f;
    [SerializeField] private float angularVelocityMultiplier = 0.3f;

    private bool isGrabbed = false;
    private Vector3[] velocityBuffer;
    private int bufferIndex = 0;
    private Vector3 previousPosition;
    private Quaternion previousRotation;
    private Vector3 initialVelocity;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.mass = objectMass;
        _rigidbody.drag = 1f - airResistance;
        velocityBuffer = new Vector3[velocityBufferSize];
    }

    public void OnGrabbed(SelectEnterEventArgs args)
    {
        _interactor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
        if (_interactor != null)
        {
            isGrabbed = true;
            initialVelocity = _rigidbody.velocity;
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;

            for (int i = 0; i < velocityBuffer.Length; i++)
                velocityBuffer[i] = Vector3.zero;

            previousPosition = _interactor.transform.position;
            previousRotation = _interactor.transform.rotation;
            bufferIndex = 0;
        }
    }

    private Vector3 CalculateThrowVelocity()
    {
        // Вычисляем среднюю линейную скорость из буфера
        Vector3 averageVelocity = Vector3.zero;
        foreach (Vector3 velocity in velocityBuffer)
        {
            averageVelocity += velocity;
        }
        averageVelocity /= velocityBuffer.Length;

        // Ограничиваем скорость
        float currentSpeed = averageVelocity.magnitude;
        if (currentSpeed > maxThrowSpeed)
        {
            averageVelocity = averageVelocity.normalized * maxThrowSpeed;
        }
        else if (currentSpeed < minThrowSpeed)
        {
            averageVelocity = Vector3.zero;
        }

        // Добавляем начальную скорость объекта
        averageVelocity += initialVelocity * 0.5f;

        return averageVelocity * throwForceMultiplier;
    }

    public void OnReleased(SelectExitEventArgs args)
    {
        if (!isGrabbed) return;

        isGrabbed = false;
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;

        if (_interactor != null)
        {
            Vector3 throwVelocity = CalculateThrowVelocity();

            // Применяем силу броска
            _rigidbody.velocity = throwVelocity;

            // Добавляем небольшое вращение на основе движения контроллера
            Vector3 angularVelocity = (_interactor.transform.rotation.eulerAngles - previousRotation.eulerAngles) * angularVelocityMultiplier;
            _rigidbody.angularVelocity = Vector3.ClampMagnitude(angularVelocity, 5f);
        }

        _interactor = null;
    }

    private void FixedUpdate()
    {
        if (isGrabbed && _interactor != null)
        {
            transform.position = _interactor.transform.position;
            transform.rotation = _interactor.transform.rotation;

            float deltaTime = Time.fixedDeltaTime;

            if (deltaTime > 0)
            {
                // Вычисляем только линейную скорость движения
                Vector3 velocity = (_interactor.transform.position - previousPosition) / deltaTime;

                // Применяем сглаживание
                velocity = Vector3.Lerp(velocityBuffer[bufferIndex], velocity, velocitySmoothing);

                velocityBuffer[bufferIndex] = velocity;
                bufferIndex = (bufferIndex + 1) % velocityBuffer.Length;
            }

            previousPosition = _interactor.transform.position;
            previousRotation = _interactor.transform.rotation;
        }
    }
}
