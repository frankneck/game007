using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TargetAndColliderController : MonoBehaviour
{
    public float rotationSpeed = 5f; // Скорость вращения вверх (К изначальной позиции)
    public float rotationSpeedUp = 20f; // Скорость вращения вниз (После вызова MoveDown())
    public GameBehaviour gameManager;
    [SerializeField] private Transform boneTransform; // !!! мы вращаем это !!!
    private bool isRotating = false; // флаг, показывающтй, идет ли вращение кости
    private Quaternion targetRotation; // К чему кость должна повернуться
    private Quaternion initialBoneRotation; // Начальная ориентация кости
    private Quaternion activeBoneRotation; // Конеченая ориентация кости
    private bool isActiveState = false; // активное состояние (нет)
    private Transform rootTarget; // (Cube.005) Самый внешний объект (родительский)

    void Start()
    {
        GameObject temp = GameObject.Find("GameManager");
        gameManager = temp.GetComponent<GameBehaviour>();
        Transform currentParent = transform;
        SkinnedMeshRenderer skinnedMesh = null;
        while (currentParent != null)
        {
            if (currentParent.name.StartsWith("Cube"))
            {
                rootTarget = currentParent;
                skinnedMesh = rootTarget.GetComponent<SkinnedMeshRenderer>();
                break;
            }
            currentParent = currentParent.parent;
        }

        if (skinnedMesh != null)
        {
            Debug.Log(rootTarget.name + ": SkinnedMeshRenderer ������, Root Bone: " + skinnedMesh.rootBone.name + " �� �������: " + gameObject.name);
            if (boneTransform != null)
            {
                if (boneTransform.name != "�����.001")
                {
                    Debug.LogWarning(rootTarget.name + ": ��������� ����� (" + boneTransform.name + ") �� �������� �����.001 �� �������: " + gameObject.name + "!");
                }

                initialBoneRotation = Quaternion.Euler(0.264194429f, 57.6771584f, 180f); // изначальная позиция мишени
                activeBoneRotation = Quaternion.Euler(331.093536f, 29.2526035f, 270.054413f); // конечная позиция мишении (активированной)

                // Инициализация данных 
                boneTransform.localRotation = initialBoneRotation;
                targetRotation = initialBoneRotation;
                isActiveState = false;

                Debug.Log(rootTarget.name + " ��������� ������� �����: " + boneTransform.localRotation.eulerAngles + " �� �������: " + gameObject.name);
            }
            else
            {
                Debug.LogError(rootTarget.name + ": Bone Transform �� ������ � Inspector! ����� �����.001 �� �������: " + gameObject.name + ".");
            }
        }
        else
        {
            Debug.LogError("SkinnedMeshRenderer �� ������ �� ��������� " + gameObject.name + "! ��������� �������� �������.");
        }
    }

    public void MoveDown()
    {
        Debug.Log("MoveDown ������ ��� �������: " + gameObject.name + " �� ���������: " + new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name);

        if (boneTransform != null && !isRotating) // isRoating == false при инициализации
        {
            Debug.Log(rootTarget.name + " �������� ������� � ��������� ��������� �� �������: " + gameObject.name + "!");
            targetRotation = activeBoneRotation; // наша цель - конечная позиция мишени (активной) 
            isRotating = true;
            isActiveState = true;
            Debug.Log("����������� ���� ��������: " + targetRotation.eulerAngles);
        }
        else   // иначе ошибка
        {
            Debug.LogWarning(rootTarget.name + " �� ����� ����������: boneTransform = " + (boneTransform != null) + ", isRotating = " + isRotating + " �� �������: " + gameObject.name);
            if (boneTransform == null) Debug.LogError("boneTransform �� ��������!");
            if (isRotating) Debug.LogWarning("�������� ��� � ��������!");
        }
    }

    void Update()
    {
        if (isRotating && boneTransform != null)
        {
            // Если объект активируется (движется в активное положение), вращаем быстрее (rotationSpeedUp),
            // если возвращается в исходное — медленнее (rotationSpeed)
            float speed = isActiveState ? rotationSpeed : rotationSpeedUp;

            // поворт кости до целевой ориентации
            boneTransform.localRotation = Quaternion.Lerp(boneTransform.localRotation, targetRotation, speed * Time.deltaTime);

            Debug.Log(rootTarget.name + " ������� ������� �����: " + boneTransform.localRotation.eulerAngles + ", ����: " + targetRotation.eulerAngles + " �� �������: " + gameObject.name + ", �������: " + Quaternion.Angle(boneTransform.localRotation, targetRotation));

            if (Quaternion.Angle(boneTransform.localRotation, targetRotation) < 0.1f)
            {
                Debug.Log(rootTarget.name + " ����� �������� ����: " + (isActiveState ? "���������" : "��������") + " ��������� �� �������: " + gameObject.name);
                boneTransform.localRotation = targetRotation;
                isRotating = false;
                Debug.Log("�������� ��������� ��� �������: " + gameObject.name);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile") && isActiveState)
        {
            Debug.Log("Есть попадание");
            Destroy(collision.gameObject);
            targetRotation = initialBoneRotation;
            isRotating = true;
            isActiveState = false;
            gameManager.Items += 10;
        }
        else
        {
            Debug.Log("Не засчитан!");
            Destroy(collision.gameObject);
        }
    }

    public void ResetTarget() // метод в TargetAndColliderController, который возвращает мишень в исходное состояние
    {
        targetRotation = initialBoneRotation;
        isRotating = true;
        isActiveState = false;
    }

}