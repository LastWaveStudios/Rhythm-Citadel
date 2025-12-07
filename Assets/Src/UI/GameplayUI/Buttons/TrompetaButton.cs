using Input;
using UI.Menus;
using UI.Menus.States;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.AudioSettings;

public class TrompetaButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image activeImage;
    public Sprite normalSprite;
    public Sprite hoverSprite;

    public Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1.1f);
    public float duration = 0.2f;

    void Start()
    {
        activeImage = GetComponent<Image>();
        activeImage.sprite = normalSprite;
        bool mobile = Application.isMobilePlatform || SystemInfo.deviceType == DeviceType.Handheld;
#if UNITY_EDITOR
            mobile = true;
#endif
        gameObject.SetActive(mobile);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        InputReader.Instance.MobileGroup5();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        activeImage.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        activeImage.sprite = normalSprite;
    }
}
