using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI currentScore;
    [SerializeField] private TextMeshProUGUI playerUI;

    public void UpdateScore(int highScore)
    {
        score.text = $"Рекорд: {highScore}";
    }

    public void ShowFinalScore(int collected)
    {
        currentScore.text = $"Cчет: {collected}";
    }

    public void SetMessage(string message)
    {
        playerUI.text = message;
    }

    public void ClearMessage(float delay)
    {
        Debug.Log("Корутина вызвана");
        StartCoroutine(ClearAfterDelay(delay));
    }

    private IEnumerator ClearAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerUI.text = "";
    }
}
