using UnityEngine;

public class TargetController : MonoBehaviour
{
    public float rotationSpeed = 2f; // Скорость опускания
    public float rotationSpeedUp = 5f; // Скорость подъёма
    private bool isRotating = false; // Общий флаг вращения
    private Quaternion targetRotation; // Целевой поворот кости
    private Quaternion initialBoneRotation; // Начальный поворот (неактивное состояние)
    private Quaternion activeBoneRotation; // Целевой поворот (активное состояние)
    private Transform boneTransform; // Трансформ кости Кость.001
    private bool isActiveState = false; // Текущее состояние мишени

    void Start()
    {
        // Находим арматуру и кость Кость.001
        SkinnedMeshRenderer skinnedMesh = GetComponent<SkinnedMeshRenderer>();
        if (skinnedMesh != null)
        {
            Transform armature = skinnedMesh.rootBone;
            if (armature != null)
            {
                boneTransform = FindBone(armature, "Кость.001");
                if (boneTransform != null)
                {
                    initialBoneRotation = Quaternion.Euler(0.264194429f, 57.6771584f, 180f); // Неактивное
                    activeBoneRotation = Quaternion.Euler(331.093536f, 29.2526035f, 270.054413f); // Активное
                    boneTransform.localRotation = initialBoneRotation;
                    targetRotation = initialBoneRotation;
                    isActiveState = false;

                    Debug.Log(gameObject.name + " начальный поворот кости: " + boneTransform.localRotation.eulerAngles);
                }
                else
                {
                    Debug.LogError(gameObject.name + ": Кость Кость.001 не найдена!");
                }
            }
            else
            {
                Debug.LogError(gameObject.name + ": Корневая кость не найдена!");
            }
        }
        else
        {
            Debug.LogError(gameObject.name + ": SkinnedMeshRenderer не найден!");
        }
    }

    private Transform FindBone(Transform parent, string boneName)
    {
        if (parent.name == boneName)
            return parent;

        foreach (Transform child in parent)
        {
            Transform found = FindBone(child, boneName);
            if (found != null)
                return found;
        }
        return null;
    }

    public void MoveDown()
    {
        if (boneTransform != null && !isRotating)
        {
            Debug.Log(gameObject.name + " начинает переход в активное состояние!");
            targetRotation = activeBoneRotation;
            isRotating = true;
            isActiveState = true;
        }
    }

    void Update()
    {
        if (isRotating && boneTransform != null)
        {
            float speed = isActiveState ? rotationSpeed : rotationSpeedUp;
            boneTransform.localRotation = Quaternion.Lerp(boneTransform.localRotation, targetRotation, speed * Time.deltaTime);

            Debug.Log(gameObject.name + " текущий поворот кости: " + boneTransform.localRotation.eulerAngles + ", цель: " + targetRotation.eulerAngles);

            if (Quaternion.Angle(boneTransform.localRotation, targetRotation) < 0.1f)
            {
                Debug.Log(gameObject.name + " кость достигла цели: " + (isActiveState ? "активное" : "неактивное") + " состояние");
                boneTransform.localRotation = targetRotation;
                isRotating = false;
            }
        }
    }

    // Метод для обработки попадания пули
    public void OnProjectileHit()
    {
        if (boneTransform != null && !isRotating)
        {
            Debug.Log(gameObject.name + " поражена снарядом, поднимаем кость в неактивное состояние!");
            targetRotation = initialBoneRotation;
            isRotating = true;
            isActiveState = false;
        }
    }
}