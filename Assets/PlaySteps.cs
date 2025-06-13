using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlaySteps : MonoBehaviour
{
    PlayableDirector director;
    public List<Step> steps;

    // Start is called before the first frame update
    void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    // Создание класса шаг
    [System.Serializable]
    public class Step
    {
        public string name;
        public float time;
        public bool hasPlayed = false;
    }

    // метод проигрыващюший шаг
    public void PlayStepIndex(int index)
    {
        Step step = steps[index];

        if (!step.hasPlayed)
        {
            step.hasPlayed = true;

            director.Stop();
            director.time = step.time;
            Debug.Log("Анимация запустилась!");
            director.Play();
        }
    }
}
