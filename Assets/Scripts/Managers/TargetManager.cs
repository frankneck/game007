using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField] private TargetAndColliderController[] targetControllers;

    private List<int> availableIndices;
    private bool isFirstSelection = true;
    private System.Random random;

    private void Awake()
    {
        random = new System.Random();
        availableIndices = Enumerable.Range(0, targetControllers.Length).ToList();
    }

    public void SelectNextTarget()
    {
        Debug.Log("Вызывается SelectNextTarget");

        if (availableIndices.Count <= 1)
        {
            availableIndices = Enumerable.Range(0, targetControllers.Length).ToList();
            isFirstSelection = true;
        }

        int randIdx = isFirstSelection && availableIndices.Count > 1 ? random.Next(1, availableIndices.Count) : random.Next(availableIndices.Count);

        isFirstSelection = false;

        int chosenIndex = availableIndices[randIdx];
        availableIndices.RemoveAt(randIdx);

        var target = targetControllers[chosenIndex];
        if (target != null)
        {
            target.MoveDown();
            // Debug.Log($"Выбранный индекс: {chosenIndex}");
        }
        else
        {
            // Debug.LogWarning($"TargetController по индексу {chosenIndex} = null");
        }
    }

    public void ResetAllTargets()
    {
        foreach (var target in targetControllers)
        {
            target.ResetTarget();
        }
    }
}
