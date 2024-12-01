using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
public class ThrowableObject : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable _grabInteractable;

    private void Awake()
    {
        // �������� ����������
        _rigidbody = GetComponent<Rigidbody>();
        _grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        // ������������� �� ������� ������� � ����������
        _grabInteractable.selectEntered.AddListener(OnGrabbed);
        _grabInteractable.selectExited.AddListener(OnReleased);
    }

    private void OnDestroy()
    {
        // ������������ �� ������� ��� ����������� �������
        _grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        _grabInteractable.selectExited.RemoveListener(OnReleased);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        // ��������� ������, ����� ������ �������� �� ������������
        _rigidbody.isKinematic = true;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        // �������� ������ � ��������� ���� ��� ������
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = args.interactorObject.transform.forward * 2f; // �������� ������
    }
}
