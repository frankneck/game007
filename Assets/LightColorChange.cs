using UnityEngine;

public class LightColorChanger : MonoBehaviour
{
    [SerializeField] private Light targetLight;

    [SerializeField] private Color colorWhenZero = Color.red;
    [SerializeField] private Color colorWhenOne = Color.green;

    public void SetColorZero()
    {
        if (targetLight != null)
            targetLight.color = colorWhenZero;
    }

    public void SetColorOne()
    {
        if (targetLight != null)
            targetLight.color = colorWhenOne;
    }
}