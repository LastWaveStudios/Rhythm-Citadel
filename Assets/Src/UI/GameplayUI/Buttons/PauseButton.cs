using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.GameplayUI.Buttons
{
    public class PauseButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
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
}
