using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.GameplayUI.Buttons
{
    public class MenuRebindButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        #region Variables
        private Image activeImage;
        public Sprite normalSprite;
        public Sprite hoverSprite;
        [SerializeField] private GameObject rebindMenu;
        private bool useMobileInput = false;
        #endregion

        #region Monobehaviour
        void Start()
        {
            gameObject.SetActive(true);
            activeImage = GetComponent<Image>();
            activeImage.sprite = normalSprite;
            useMobileInput = Application.isMobilePlatform
                         || SystemInfo.deviceType == DeviceType.Handheld;
            if (useMobileInput)
            {
                gameObject.SetActive(false);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!rebindMenu.activeSelf)
            {
                rebindMenu.SetActive(true);
            }

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            activeImage.sprite = hoverSprite;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            activeImage.sprite = normalSprite;  
        }
        #endregion

    }
}
