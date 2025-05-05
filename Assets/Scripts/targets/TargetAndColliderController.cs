using UnityEngine;

public class TargetAndColliderController : MonoBehaviour
{
    public float rotationSpeed = 5f; // Увеличиваем скорость опускания
    public float rotationSpeedUp = 10f; // Увеличиваем скорость подъёма
    [SerializeField] private Transform boneTransform; // Указываем Кость.001 вручную в Inspector
    private bool isRotating = false; // Флаг вращения
    private Quaternion targetRotation; // Целевой поворот кости
    private Quaternion initialBoneRotation; // Начальный поворот (поднятое состояние)
    private Quaternion activeBoneRotation; // Целевой поворот (опущенное состояние)
    private bool isActiveState = false; // Текущее состояние
    private Transform rootTarget; // Корневой объект мишени (Cube.005)

    void Start()
    {
        // Ищем SkinnedMeshRenderer на родителях, пока не найдём Cube.XXX
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
            Debug.Log(rootTarget.name + ": SkinnedMeshRenderer найден, Root Bone: " + skinnedMesh.rootBone.name + " на объекте: " + gameObject.name);
            if (boneTransform != null)
            {
                if (boneTransform.name != "Кость.001")
                {
                    Debug.LogWarning(rootTarget.name + ": Указанная кость (" + boneTransform.name + ") не является Кость.001 на объекте: " + gameObject.name + "!");
                }
                initialBoneRotation = Quaternion.Euler(0.264194429f, 57.6771584f, 180f); // Поднятое
                activeBoneRotation = Quaternion.Euler(331.093536f, 29.2526035f, 270.054413f); // Опущенное
                boneTransform.localRotation = initialBoneRotation;
                targetRotation = initialBoneRotation;
                isActiveState = false;

                Debug.Log(rootTarget.name + " начальный поворот кости: " + boneTransform.localRotation.eulerAngles + " на объекте: " + gameObject.name);
            }
            else
            {
                Debug.LogError(rootTarget.name + ": Bone Transform не указан в Inspector! Укажи Кость.001 на объекте: " + gameObject.name + ".");
            }
        }
        else
        {
            Debug.LogError("SkinnedMeshRenderer не найден на родителях " + gameObject.name + "! Проверьте иерархию объекта.");
        }
    }

    public void MoveDown()
    {
        Debug.Log("MoveDown вызван для объекта: " + gameObject.name + " из источника: " + new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name);
        if (boneTransform != null && !isRotating)
        {
            Debug.Log(rootTarget.name + " начинает переход в опущенное состояние на объекте: " + gameObject.name + "!");
            targetRotation = activeBoneRotation;
            isRotating = true;
            isActiveState = true;
            Debug.Log("Установлена цель вращения: " + targetRotation.eulerAngles);
        }
        else
        {
            Debug.LogWarning(rootTarget.name + " не может опуститься: boneTransform = " + (boneTransform != null) + ", isRotating = " + isRotating + " на объекте: " + gameObject.name);
            if (boneTransform == null) Debug.LogError("boneTransform не назначен!");
            if (isRotating) Debug.LogWarning("Вращение уже в процессе!");
        }
    }

    void Update()
    {
        if (isRotating && boneTransform != null)
        {
            float speed = isActiveState ? rotationSpeed : rotationSpeedUp;
            Quaternion previousRotation = boneTransform.localRotation;
            boneTransform.localRotation = Quaternion.Lerp(boneTransform.localRotation, targetRotation, speed * Time.deltaTime);

            Debug.Log(rootTarget.name + " текущий поворот кости: " + boneTransform.localRotation.eulerAngles + ", цель: " + targetRotation.eulerAngles + " на объекте: " + gameObject.name + ", разница: " + Quaternion.Angle(boneTransform.localRotation, targetRotation));

            if (Quaternion.Angle(boneTransform.localRotation, targetRotation) < 0.1f)
            {
                Debug.Log(rootTarget.name + " кость достигла цели: " + (isActiveState ? "опущенное" : "поднятое") + " состояние на объекте: " + gameObject.name);
                boneTransform.localRotation = targetRotation;
                isRotating = false;
                Debug.Log("Вращение завершено для объекта: " + gameObject.name);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile") && !isRotating)
        {
            Debug.Log(rootTarget.name + " поражена снарядом, поднимаем кость на объекте: " + gameObject.name + " от объекта: " + collision.gameObject.name + "!");
            Destroy(collision.gameObject); // Уничтожаем пулю
            targetRotation = initialBoneRotation;
            isRotating = true;
            isActiveState = false;
        }
    }
}