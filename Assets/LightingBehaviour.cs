using System.Collections;
using TMPro;
using UnityEngine;

public class LightingBehaviour : Sounds
{
    [Header("Light Groups")]
    [SerializeField] private Light[] firstRowLights;
    [SerializeField] private Light[] secondRowLights;
    [SerializeField] private Light[] thirdRowLights;
    [SerializeField] private GameObject[] firstRowVolumetricLights;
    [SerializeField] private GameObject[] secondRowVolumetricLights;
    [SerializeField] private GameObject[] thirdRowVolumetricLights;

    [Header("Настройка света лампы")]
    [SerializeField] private Light deskLampLight;
    [SerializeField] private Light deskLampSpotLight;
    [SerializeField] private GameObject deskVolumetricLight;

    [Header("Настройка одиночных источников")]
    [SerializeField] private Canvas pcInterface;

    [Header("Индикатор")]
    [SerializeField] private LightColorChanger indicatorLight;

    [Header("Время")]
    [SerializeField, Tooltip("Задержка между этапами включения")] private float delayBetweenSteps = 0.5f;

    private bool powerIsOn = false;

    void Start()
    {
        TurnOffAllLights();
        indicatorLight.SetColorZero(); // Красный индикатор изначально
    }

    void TurnOffAllLights()
    {
        // Отключение точечных источников света
        foreach (var light in firstRowLights) light.enabled = false;
        foreach (var light in secondRowLights) light.enabled = false;
        foreach (var light in thirdRowLights) light.enabled = false;

        // Отключение метричных лучей
        foreach (var light in firstRowVolumetricLights) light.gameObject.SetActive(false);
        foreach (var light in secondRowVolumetricLights) light.gameObject.SetActive(false);
        foreach (var light in thirdRowVolumetricLights) light.gameObject.SetActive(false);

        // Отключение светильника
        if (deskLampLight != null) deskLampLight.enabled = false;
        if (deskLampSpotLight != null) deskLampLight.enabled = false;
        if (deskVolumetricLight != null) deskVolumetricLight.gameObject.SetActive(false);

        // Отключение интерфейса приложения
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
        yield return TurnOnLightGroup(firstRowLights, firstRowVolumetricLights);
        yield return new WaitForSeconds(delayBetweenSteps);

        yield return TurnOnLightGroup(secondRowLights, secondRowVolumetricLights);
        yield return new WaitForSeconds(delayBetweenSteps);

        yield return TurnOnLightGroup(thirdRowLights, thirdRowVolumetricLights);
        yield return new WaitForSeconds(delayBetweenSteps);

        if (deskLampLight != null)
        {
            PlaySound(sounds[4]); // звук включения настольной лампы
            deskLampLight.enabled = true;
            deskLampSpotLight.enabled = true;
            deskVolumetricLight.SetActive(true);
        }
        yield return new WaitForSeconds(delayBetweenSteps);

        if (pcInterface != null)
        {
            PlaySound(sounds[2]); // звук включения компьютера
            pcInterface.gameObject.SetActive(true);
        }
    }

    private IEnumerator TurnOnLightGroup(Light[] pointLights, GameObject[] volumetricLights)
    {
        foreach (var light in pointLights)
            light.enabled = true;

        foreach (var light in volumetricLights)
            light.SetActive(true);

        if (sounds.Length > 0)
            PlaySound(sounds[1], volume: 0.5f, p1: 0.9f, p2: 1f); // звук переключателя
        else
            Debug.LogWarning("Отсутсвует клип");    

        yield return null;
    }
}
