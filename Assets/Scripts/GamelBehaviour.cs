using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Playables;
using Random = System.Random; // использую Random System

public class GameBehaviour : Sounds
{
    [SerializeField] private PlaySteps playSteps;
    [SerializeField] private TargetAndColliderController[] targetControllers;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI currentScore;
    [SerializeField] private TextMeshProUGUI playerUI;
    [SerializeField] private int highScore;
    [SerializeField] public MonoBehaviour[] scriptsToToggle;
    public string labelText = "Попади во все цели за определнное время";
    public float gameDuration = 10f;
    private float timeRemaining;
    private bool isGameActive = false;
    public int maxItems = 5;
    public bool showWinScreen = false;
    private bool showLossScreen = false;
    private int _itemsCollected = 0;
    public Random random = new Random();
    private string _state;
    private List<int> availableIndices = new List<int>();
    private int count = 0;
    private bool isFirstSelection = true;

    public string State
    {
        get { return _state; }
        set { _state = value; }
    }
    public int Items
    {
        get { return _itemsCollected; }
        set
        {
            _itemsCollected = value;

            // Если в списке меньше 1 цели — обновим его заранее
            if (availableIndices.Count <= 1)
            {
                availableIndices = Enumerable.Range(0, targetControllers.Length).ToList();
                isFirstSelection = true;
                Debug.Log("Список доступных индексов обновлён.");
            }

            if (availableIndices.Count == 0)
            {
                Debug.LogWarning("Нет доступных целей для выбора.");
                return;
            }

            int randIdx;

            if (isFirstSelection && availableIndices.Count > 1)
            {
                // Исключаем индекс 0 при первом вызове
                randIdx = random.Next(1, availableIndices.Count);
                isFirstSelection = false;
            }
            else
            {
                randIdx = random.Next(availableIndices.Count);
            }

            int chosenIndex = availableIndices[randIdx];
            availableIndices.RemoveAt(randIdx);

            if (targetControllers[chosenIndex] != null)
            {
                targetControllers[chosenIndex].MoveDown();
                Debug.Log($"Выбранный индекс: {chosenIndex}");
            }
            else
            {
                Debug.LogWarning($"TargetController по индексу {chosenIndex} = null");
            }
        }
    }

    void Start()
    {
        random = new Random();
        availableIndices = Enumerable.Range(0, targetControllers.Length).ToList(); // все индексы
        score.text = $"Рекорд: {highScore}";
    }

    void Update()
    {
        if (isGameActive)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                playerUI.text = "Время вышло";
                isGameActive = false;
                labelText = "Время вышло";

                currentScore.text = $"Cчет: {_itemsCollected}"; // C - англйиская тк я ХЗ ЧЕ С Русской не такы

                if (_itemsCollected > highScore)
                {
                    highScore = _itemsCollected;
                    score.text = $"Рекорд: {highScore}";
                    Debug.Log("Новый рекорд!");
                    playSteps?.PlayStepIndex(1); // октрытие двери
                }

                StartCoroutine(ResetPlayerUI(1f));
                ResetAllTargets();
                PlaySound(sounds[0], volume: 0.3f, p1: 0.8f, p2: 0.9f); // звук завершения игры 
                // PlaySound(sounds[0], volume: 0.6f, p1: 1.5f, p2: 1.5f); // +50% скорость
            }
        }
    }

    private void OnGUI()
    {
        if (isGameActive)
        {
            GUI.Label(new Rect(20, 50, 200, 25), $"Осталось времени: {Mathf.CeilToInt(timeRemaining)} сек");
        }
    }

    public void StartGame()
    {
        timeRemaining = gameDuration;
        isGameActive = true;

        _itemsCollected = 0; // решил вместо публичной Items обнулять приватную (все вроде работает)
        Debug.Log("Игра началась");
        StartCoroutine(ResetPlayerUI(0.2f));
    }

    private void ResetAllTargets()
    {
        foreach (var target in targetControllers)
        {
            target.ResetTarget(); // метод в TargetAndColliderController, который возвращает мишень в исходное состояние
        }
    }

    private IEnumerator ResetPlayerUI(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerUI.text = "";
    }

        public void DisableScripts()
    {
        foreach (var script in scriptsToToggle)
            if (script != null) script.enabled = false;
    }

    public void EnableScripts()
    {
        foreach (var script in scriptsToToggle)
            if (script != null) script.enabled = true;
    }
}
