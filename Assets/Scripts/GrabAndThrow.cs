using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
public class GrabAndThrow : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor _interactor;

    [Header("Основные настройки")]
    [SerializeField] private float smoothSpeed = 20f;
    [SerializeField] private float objectMass = 5f;

    [Header("Настройки броска")]
    [SerializeField] private float throwForceMultiplier = 1.0f;
    [SerializeField] private float maxThrowSpeed = 5f;
    [SerializeField] private float minThrowSpeed = 1f;
    [SerializeField] private float dragCoefficient = 0.5f;

    private bool isGrabbed = false;
    private Vector3[] positionHistory = new Vector3[10];
    private float[] timeHistory = new float[10];
    private int historyIndex = 0;
    private const int velocitySampleCount = 3;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.mass = objectMass;
        _rigidbody.drag = dragCoefficient;
    }

    public void OnGrabbed(SelectEnterEventArgs args)
    {
        _interactor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
        if (_interactor != null)
        {
            isGrabbed = true;
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
            InitializeHistory();
        }
    }

    private void InitializeHistory()
    {
        for (int i = 0; i < positionHistory.Length; i++)
        {
            positionHistory[i] = _interactor.transform.position;
            timeHistory[i] = Time.time;
        }
        historyIndex = 0;
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
            ApplyThrowForce(throwVelocity);
        }

        _interactor = null;
    }

    private Vector3 CalculateThrowVelocity()
    {
        Vector3 velocity = Vector3.zero;
        int usableSamples = 0;

        // Используем только последние несколько сэмплов для более точного расчета
        for (int i = 1; i <= velocitySampleCount; i++)
        {
            int currentIndex = ((historyIndex - i) + positionHistory.Length) % positionHistory.Length;
            int previousIndex = ((historyIndex - i - 1) + positionHistory.Length) % positionHistory.Length;

            float deltaTime = timeHistory[currentIndex] - timeHistory[previousIndex];
            if (deltaTime > 0)
            {
                Vector3 deltaPosition = positionHistory[currentIndex] - positionHistory[previousIndex];
                velocity += deltaPosition / deltaTime;
                usableSamples++;
            }
        }

        if (usableSamples > 0)
        {
            velocity /= usableSamples;
        }

        return velocity;
    }

    private void ApplyThrowForce(Vector3 throwVelocity)
    {
        // Ограничиваем скорость броска
        float velocityMagnitude = throwVelocity.magnitude;
        velocityMagnitude = Mathf.Clamp(velocityMagnitude, minThrowSpeed, maxThrowSpeed);

        // Нормализуем направление и применяем ограниченную скорость
        Vector3 throwDirection = throwVelocity.normalized;

        // Применяем множитель силы броска с учетом массы
        float massInfluence = Mathf.Lerp(1f, 0.5f, objectMass / 10f);
        float finalThrowForce = velocityMagnitude * throwForceMultiplier * massInfluence;

        // Ограничиваем вертикальную составляющую
        throwDirection.y = Mathf.Clamp(throwDirection.y, -0.5f, 0.5f);

        // Применяем финальную скорость
        Vector3 finalVelocity = throwDirection * finalThrowForce;

        // Добавляем небольшое естественное вращение
        Vector3 randomRotation = Random.insideUnitSphere * finalThrowForce * 0.5f;

        _rigidbody.velocity = finalVelocity;
        _rigidbody.angularVelocity = randomRotation;
    }

    private void FixedUpdate()
    {
        if (isGrabbed && _interactor != null)
        {
            // Обновляем историю позиций
            historyIndex = (historyIndex + 1) % positionHistory.Length;
            positionHistory[historyIndex] = _interactor.transform.position;
            timeHistory[historyIndex] = Time.time;

            // Плавное следование за контроллером
            Vector3 targetPosition = _interactor.transform.position;
            Quaternion targetRotation = _interactor.transform.rotation;

            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * smoothSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * smoothSpeed);
        }
    }
}
