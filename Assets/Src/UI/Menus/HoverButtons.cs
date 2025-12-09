using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image activeImage;
    [SerializeField] private Sprite _normalSprite;
    [SerializeField] private Sprite _hoverSprite;
    void Start()
    {
        activeImage = GetComponent<Image>();
        activeImage.sprite = _normalSprite;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        activeImage.sprite = _hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        activeImage.sprite = _normalSprite;
    }



}
