using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
public class GrabAndThrow : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor _interactor;
    private Transform _grabPoint;

    [SerializeField]
    private float smoothSpeed = 20f; // Скорость сглаживания движения
    private bool isGrabbed = false;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void OnGrabbed(SelectEnterEventArgs args)
    {
        _interactor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;

        if (_interactor != null)
        {
            isGrabbed = true;
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;

            // Мгновенная телепортация в руку
            transform.position = _interactor.transform.position;
            transform.rotation = _interactor.transform.rotation;

            _grabPoint = _interactor.transform;
        }
    }

    public void OnReleased(SelectExitEventArgs args)
    {
        isGrabbed = false;
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;

        if (_interactor != null)
        {
            var interactorObject = _interactor.transform.GetComponent<XRController>();
            if (interactorObject != null)
            {
                // Получаем скорость и угловую скорость контроллера
                var velocity = interactorObject.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceVelocity, out var deviceVelocity)
                    ? deviceVelocity
                    : Vector3.zero;

                var angularVelocity = interactorObject.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceAngularVelocity, out var deviceAngularVelocity)
                    ? deviceAngularVelocity
                    : Vector3.zero;

                // Динамическая настройка множителя в зависимости от скорости контроллера
                float speedMagnitude = velocity.magnitude; // Сила движения контроллера

                // Ограничиваем максимальную скорость броска, чтобы не получить слишком сильный бросок
                float maxThrowSpeed = 5f; // Уменьшаем максимальную скорость броска
                float throwMultiplier = Mathf.Clamp(speedMagnitude * 0.05f, 0.1f, 0.5f); // Сильное уменьшение множителя

                // Применяем динамическую скорость, ограниченную максимальной
                _rigidbody.velocity = Vector3.ClampMagnitude(velocity * throwMultiplier, maxThrowSpeed);
                _rigidbody.angularVelocity = angularVelocity * throwMultiplier;

                // Убедимся, что камень летит в правильном направлении, ориентируясь на контроллер
                // Мы передаем вектор скорости в направлении контроллера
                Vector3 throwDirection = _interactor.transform.forward; // Направление контроллера
                _rigidbody.velocity = throwDirection * velocity.magnitude * throwMultiplier;
            }
        }

        _interactor = null;
    }



    private void FixedUpdate()
    {
        if (isGrabbed && _interactor != null)
        {
            // Плавное следование за контроллером
            targetPosition = _interactor.transform.position;
            targetRotation = _interactor.transform.rotation;

            // Интерполяция позиции и поворота
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * smoothSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * smoothSpeed);
        }
    }
}
