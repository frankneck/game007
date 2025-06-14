using UnityEngine;

public class TargetAndColliderController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float rotationSpeedUp = 20f;
    [SerializeField] private Transform boneTransform;
    [SerializeField] private GameController gameController;

    private Quaternion initialBoneRotation;
    private Quaternion activeBoneRotation;
    private Quaternion targetRotation;
    private bool isRotating = false;
    private bool isActiveState = false;
    private Transform rootTarget;

    void Start()
    {
        SetupRootTarget();

        if (boneTransform == null)
        {
            Debug.LogError($"{name}: BoneTransform не назначен!");
            enabled = false;
            return;
        }

        initialBoneRotation = Quaternion.Euler(0.264194429f, 57.6771584f, 180f);
        activeBoneRotation = Quaternion.Euler(331.093536f, 29.2526035f, 270.054413f);
        boneTransform.localRotation = initialBoneRotation;
        targetRotation = initialBoneRotation;
    }

    void SetupRootTarget()
    {
        Transform currentParent = transform;
        while (currentParent != null)
        {
            if (currentParent.name.StartsWith("Cube"))
            {
                rootTarget = currentParent;
                break;
            }
            currentParent = currentParent.parent;
        }
    }

    public void MoveDown()
    {
        if (boneTransform != null && !isRotating)
        {
            targetRotation = activeBoneRotation;
            isRotating = true;
            isActiveState = true;
        }
    }

    public void ResetTarget()
    {
        targetRotation = initialBoneRotation;
        isRotating = true;
        isActiveState = false;
    }

    void Update()
    {
        if (isRotating && boneTransform != null)
        {
            float speed = isActiveState ? rotationSpeed : rotationSpeedUp;
            boneTransform.localRotation = Quaternion.Lerp(boneTransform.localRotation, targetRotation, speed * Time.deltaTime);

            if (Quaternion.Angle(boneTransform.localRotation, targetRotation) < 0.1f)
            {
                boneTransform.localRotation = targetRotation;
                isRotating = false;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Projectile")) return;

        Destroy(collision.gameObject);

        if (isActiveState)
        {
            ResetTarget();
            Debug.Log("Попал!");
            gameController?.ItemCollected(); // Логика начисления очков вынесена
        }
    }
}
