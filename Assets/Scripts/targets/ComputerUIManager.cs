using UnityEngine;

public class ComputerUIManager : MonoBehaviour
{
    public TargetController[] targets;

    public void StartTargetsApp()
    {
        Debug.Log("Кнопка нажата! Запускаю мишени.");
        foreach (var target in targets)
        {
            target.MoveDown();
        }
    }
}