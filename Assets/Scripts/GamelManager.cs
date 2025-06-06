using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBehaviour : MonoBehaviour
{
    public string labelText = "Попади во все цели за определнное время";
    public int maxItems = 5;
    public bool showWinScreen = false;
    private bool showLossScreen = false; 
    private int _itemsCollected = 0;    
    private string _state;
    public string State
    {
        get { return _state; }
        set { _state = value; }
    }

    public int Items {
        get { return _itemsCollected; }
        set 
        { 
            _itemsCollected = value;
            
            if (_itemsCollected >= maxItems)
            {
                labelText = "Ты попал во все мишени!";
                ChangeGameState(false, true);
            }
            else
            {
                labelText = "А ты меткий стрелок." + (maxItems - _itemsCollected);
            }
        }
    }

    private void ChangeGameState(bool showLossScreen = false, bool showWinScreen = false)
    {   
        if (showLossScreen)
        {
            this.showLossScreen = showLossScreen;
        }
        else if (showWinScreen)
        {
            this.showWinScreen = showWinScreen;
        }

        Time.timeScale = 0f;
    }

    private void OnGUI()
    {        
        GUI.Box(new Rect(20, 20, 150, 25), $"Попадания: {Items}");

        if (showWinScreen)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "Ты выиграл!")) { }
        }

        if (showLossScreen)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "Ты проиграл...")) { }
        }

        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - 50, 300, 50), labelText);
    } 
}
