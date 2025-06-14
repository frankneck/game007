using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public float gameDuration = 10f;
    private float timeRemaining;
    public bool IsGameActive { get; private set; }

    public event Action OnGameStart;
    public event Action OnGameEnd;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Update()
    {
        if (!IsGameActive) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            EndGame();
        }
    }

    public float GetRemainingTime() => timeRemaining;

    public void StartGame()
    {
        timeRemaining = gameDuration;
        IsGameActive = true;
        OnGameStart?.Invoke();
    }

    public void EndGame()
    {
        if (!IsGameActive) return;

        IsGameActive = false;
        OnGameEnd?.Invoke();
    }
}
