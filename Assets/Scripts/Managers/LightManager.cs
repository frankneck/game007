using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
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
    [SerializeField] private List<LightGroupRow> lightRows = new List<LightGroupRow>();

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
    private Vector3 rowCenterPosition;

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
            AudioManager.instance.PlayOneShot("TurnOn", indicatorLight.transform.position); // звук переключателя

            StartCoroutine(TurnOnLightsSequence());
        }
        else
        {
            powerIsOn = false;
            StopAllCoroutines();
            TurnOffAllLights();
            indicatorLight.SetColorZero(); // Красный индикатор
            AudioManager.instance.PlayOneShot("TurnOff", indicatorLight.transform.position); // звук переключателя

            // if (rowCenterPosition != null)
            AudioManager.instance.PlayOneShot("TurnOffAllLights", rowCenterPosition); // звук выключения всего света
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

            rowCenterPosition = GetRowCenter(row); // записываем позицию при включении
            AudioManager.instance.PlayOneShot("TurnOnRow", rowCenterPosition, pitchMin: 0.9f, pitchMax: 1f, volume: 1f); // звук выключения ряда источников света

            yield return new WaitForSeconds(delayBetweenSteps);
        }

        if (deskLampLight != null)
        {
            AudioManager.instance.PlayOneShot("TurnOnTableLamp", deskLampLight.transform.position); // звук включения настольной лампы
            deskLampLight.enabled = true;
            deskLampSpotLight.enabled = true;
            deskVolumetricLight.SetActive(true);
        }

        yield return new WaitForSeconds(delayBetweenSteps);

        if (pcInterface != null)
        {
            AudioManager.instance.PlayOneShot("TurnOnPC", pcInterface.transform.position); // звук включения компьютера
            pcInterface.gameObject.SetActive(true);
        }
    }
    
    Vector3 GetRowCenter(LightGroupRow row)
    {
        List<Vector3> positions = new();

        foreach (var light in row.pointLights)
            if (light != null)
                positions.Add(light.transform.position);

        foreach (var obj in row.materialObjects)
            if (obj != null)
                positions.Add(obj.transform.position);

        if (positions.Count == 0)
            return Vector3.zero;

        Vector3 sum = Vector3.zero;
        foreach (var pos in positions)
            sum += pos;

        return sum / positions.Count;
    }
}
