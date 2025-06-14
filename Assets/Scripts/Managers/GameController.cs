using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private TargetManager targetManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private PlaySteps playSteps;  // Добавляем ссылку
    [SerializeField] private Transform hornSoundPos;

    private int itemsCollected = 0;
    private int highScore = 0;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStart += OnGameStart;
            GameManager.Instance.OnGameEnd += OnGameEnd;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStart -= OnGameStart;
            GameManager.Instance.OnGameEnd -= OnGameEnd;
        }
    }

    private void Update()
    {
        // if (GameManager.Instance.IsGameActive)
        // {
        //     uiManager.SetMessage($"Осталось времени: {Mathf.CeilToInt(GameManager.Instance.GetRemainingTime())} сек");
        // }
    }

    private void OnGameStart()
    {
        itemsCollected = 0;
        uiManager.ClearMessage(0f);
        targetManager?.SelectNextTarget();
    }

    private void OnGameEnd()
    {
        uiManager.UpdateScore(0);
        uiManager.SetMessage("Время вышло");
        uiManager.ClearMessage(0.5f);
        uiManager.ShowFinalScore(itemsCollected);
        targetManager.ResetAllTargets();

        if (itemsCollected > highScore)
        {
            highScore = itemsCollected;
            uiManager.UpdateScore(highScore);
            Debug.Log("Новый рекорд!");

            // Вызов анимации открытия двери через playSteps
            playSteps?.PlayStepIndex(1);
        }

        AudioManager.instance.PlayOneShot("GameOver", hornSoundPos.position);
    }

    public void ItemCollected()
    {
        itemsCollected++;
        Debug.Log("Набрал очко");
        targetManager.SelectNextTarget();
    }


    public void PrepareGame()
    {
        StartCoroutine(DelayedTrigger(4f));
    }

    private IEnumerator DelayedTrigger(float delay)
    {
        float tickInterval = 1f;
        int count = 3;

        while (count > 0)
        {
            AudioManager.instance.PlayOneShot("Tick", hornSoundPos.position);
            uiManager?.SetMessage($"{count--}...");
            yield return new WaitForSeconds(tickInterval);
        }

        AudioManager.instance.PlayOneShot("Horn", hornSoundPos.position);
        uiManager?.SetMessage("Начали!");
        yield return new WaitForSeconds(1f);

        GameManager.Instance.StartGame(); // Запускаем игру только после отсчёта
    }

}
