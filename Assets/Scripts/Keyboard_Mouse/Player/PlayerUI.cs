using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private Image image;

    private void Start()
    {
        image.gameObject.SetActive(false);
    }

    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }
    public void UpdateCursor(bool isInteract)
    {
        if (isInteract)
            image.gameObject.SetActive(true);
        else
            image.gameObject.SetActive(false);
    }
}
