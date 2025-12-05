using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Menus.DificultySelector
{

    public class WheelButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _myArrow; 
        [SerializeField] private int _dificultyLevel;

        void Start()
        {
            Image img = GetComponent<Image>();
            img.alphaHitTestMinimumThreshold = 0.1f;
        }
        public void OnPointerClick(PointerEventData eventData) // Change the level dificulty and pass to the next scene.
        {
            Debug.Log(_dificultyLevel);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _myArrow.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _myArrow.SetActive(false);
        }
    }
}
