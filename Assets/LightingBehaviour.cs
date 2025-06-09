using System.Collections;
using TMPro;
using UnityEngine;

public class LightingBehaviour : Sounds
{
    [Header("Light Groups")]
    [SerializeField] private Light[] firstRowLights;
    [SerializeField] private Light[] secondRowLights;
    [SerializeField] private Light[] thirdRowLights;

    [Header("Single Lights")]
    [SerializeField] private Light deskLampLight;
    [SerializeField] private Light monitorLight;
    [SerializeField] private Canvas pcInterface;

    [Header("Indicator")]
    [SerializeField] private LightColorChanger indicatorLight;

    [Header("Timing")]
    [SerializeField, Tooltip("Задержка между этапами включения")] private float delayBetweenSteps = 0.5f;

    private bool powerIsOn = false;

    void Start()
    {
        TurnOffAllLights();
        indicatorLight.SetColorZero(); // Красный индикатор изначально
    }

    void TurnOffAllLights()
    {
        foreach (var light in firstRowLights) light.enabled = false;
        foreach (var light in secondRowLights) light.enabled = false;
        foreach (var light in thirdRowLights) light.enabled = false;

        if (deskLampLight != null) deskLampLight.enabled = false;
        if (monitorLight != null) monitorLight.enabled = false;
        if (pcInterface != null) pcInterface.gameObject.SetActive(false);
    }

    public void TogglePower()
    {
        if (!powerIsOn)
        {
            powerIsOn = true;
            indicatorLight.SetColorOne(); // Зеленый индикатор
            if (sounds.Length > 0)
                PlaySound(sounds[0]); // звук переключателя
            else
                Debug.LogWarning("Отсутсвует клип");
            StartCoroutine(TurnOnLightsSequence());
        }
        else
        {
            powerIsOn = false;
            StopAllCoroutines();
            TurnOffAllLights();
            indicatorLight.SetColorZero(); // Красный индикатор
            PlaySound(sounds[0]); // звук переключателя
            PlaySound(sounds[3]); // звук выключения света
        }
    }

    private IEnumerator TurnOnLightsSequence()
    {
        yield return TurnOnLightGroup(firstRowLights);
        yield return new WaitForSeconds(delayBetweenSteps);

        yield return TurnOnLightGroup(secondRowLights);
        yield return new WaitForSeconds(delayBetweenSteps);

        yield return TurnOnLightGroup(thirdRowLights);
        yield return new WaitForSeconds(delayBetweenSteps);

        if (deskLampLight != null)
            deskLampLight.enabled = true;
        yield return new WaitForSeconds(delayBetweenSteps);

        if (monitorLight != null)
            monitorLight.enabled = true;

        if (pcInterface != null)
        {
            PlaySound(sounds[2]); // звук включения компьютера
            pcInterface.gameObject.SetActive(true);
        }
    }

    private IEnumerator TurnOnLightGroup(Light[] lights)
    {
        foreach (var light in lights)
            light.enabled = true;

        if (sounds.Length > 0)
            PlaySound(sounds[1], volume: 0.5f, p1: 0.9f, p2: 1f); // звук переключателя
        else
            Debug.LogWarning("Отсутсвует клип");    

        yield return null;
    }
}
