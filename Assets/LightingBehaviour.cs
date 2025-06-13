using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingBehaviour : Sounds
{
    [Serializable]
    public class LightGroupRow
    {
        [Tooltip("Точечные источники света в ряду")]
        public Light[] pointLights;

        [Tooltip("Объемные (Volumetric) источники света в ряду")]
        public GameObject[] volumetricLights;

        [Tooltip("Объекты ламп для смены материала")]
        public List<GameObject> materialObjects;
    }

    [Header("Ряды света")]
    [Tooltip("Список рядов с точечными и объемными источниками и материалами")]
    [SerializeField]
    private List<LightGroupRow> lightRows = new List<LightGroupRow>();

    [Header("Материалы для ламп")]
    [SerializeField] private Material onLampMaterial;
    [SerializeField] private Material offLampMaterial;

    [Header("Настройка света настольной лампы")]
    [SerializeField] private Light deskLampLight;
    [SerializeField] private Light deskLampSpotLight;
    [SerializeField] private GameObject deskVolumetricLight;

    [Header("Настройка одиночных источников")]
    [SerializeField] private Canvas pcInterface;

    [Header("Индикатор")]
    [SerializeField] private LightColorChanger indicatorLight;

    [Header("Задержка между этапами включения")]
    [SerializeField] private float delayBetweenSteps = 0.5f;

    private bool powerIsOn = false;

    void Start()
    {
        TurnOffAllLights();
        indicatorLight.SetColorZero(); // Красный индикатор изначально
    }

    void TurnOffAllLights()
    {
        foreach (var row in lightRows)
        {
            foreach (var light in row.pointLights)
                if (light != null)
                    light.enabled = false;

            foreach (var volumetric in row.volumetricLights)
                if (volumetric != null)
                    volumetric.SetActive(false);

            foreach (var matObj in row.materialObjects)
            {
                if (matObj == null) continue;
                var renderer = matObj.GetComponent<MeshRenderer>();
                if (renderer != null)
                    renderer.material = offLampMaterial;
            }
        }

        if (deskLampLight != null) deskLampLight.enabled = false;
        if (deskLampSpotLight != null) deskLampSpotLight.enabled = false;
        if (deskVolumetricLight != null) deskVolumetricLight.SetActive(false);

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
        foreach (var row in lightRows)
        {
            foreach (var light in row.pointLights)
                if (light != null)
                    light.enabled = true;

            foreach (var volumetric in row.volumetricLights)
                if (volumetric != null)
                    volumetric.SetActive(true);

            foreach (var matObj in row.materialObjects)
            {
                if (matObj == null) continue;
                var renderer = matObj.GetComponent<MeshRenderer>();
                if (renderer != null)
                    renderer.material = onLampMaterial;
            }

            if (sounds.Length > 0)
                PlaySound(sounds[1], volume: 0.5f, p1: 0.9f, p2: 1f);
            else
                Debug.LogWarning("Отсутсвует клип");

            yield return new WaitForSeconds(delayBetweenSteps);
        }

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
}
