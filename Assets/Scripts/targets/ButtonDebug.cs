using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonDebug : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Ћуч наведЄн на кнопку!");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Ћуч ушЄл с кнопки!");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(" нопка нажата контроллером!");
    }
}