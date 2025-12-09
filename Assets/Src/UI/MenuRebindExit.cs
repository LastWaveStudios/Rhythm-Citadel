using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class MenuRebindExit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        #region Variables
        private Image activeImage;
        public Sprite normalSprite;
        public Sprite hoverSprite;
        [SerializeField] private GameObject rebindMenu;
        #endregion

        #region Monobehaviour
        void Start()
        {
            activeImage = GetComponent<Image>();
            activeImage.sprite = normalSprite;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (rebindMenu.activeSelf)
            {
                rebindMenu.SetActive(false);
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

