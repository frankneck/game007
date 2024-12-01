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
                // Получаем скорость контроллера
                var velocity = interactorObject.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceVelocity, out var deviceVelocity)
                    ? deviceVelocity
                    : Vector3.zero;

                // Динамическая настройка множителя
                float speedMagnitude = velocity.magnitude; // Сила движения контроллера

                // Ограничиваем максимальную скорость броска, чтобы не получить слишком сильный бросок
                float maxThrowSpeed = 3f; // Максимальная скорость броска
                float throwMultiplier = Mathf.Clamp(speedMagnitude * 0.03f, 0.1f, 0.3f); // Множитель для уменьшения скорости

                // Используем только направление контроллера, чтобы исключить отклонения
                Vector3 throwDirection = _interactor.transform.forward; // Направление контроллера

                // Если кинуть ровно вверх, то "forward" будет всё равно направлен немного по горизонтали, это можно компенсировать
                if (Mathf.Abs(velocity.y) > 0.1f)  // Если есть вертикальная скорость
                {
                    throwDirection = Vector3.up; // Если бросок вверх, корректируем направление
                }

                // Применяем скорость броска с ограничениями
                _rigidbody.velocity = throwDirection * velocity.magnitude * throwMultiplier;

                // Ограничиваем скорость
                _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, maxThrowSpeed);
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
