using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class AButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public string name;
    public TextMeshProUGUI text;
    public abstract void OnPointerClick(PointerEventData eventData);

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.text = name;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.text = "";
    }
}
