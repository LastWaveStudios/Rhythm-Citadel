using Gameplay;
using Gameplay.World;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Menus.DificultySelector
{

    public class WheelButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _myArrow;
        [SerializeField] private Difficulty _myDifficulty;
        [SerializeField] private GameObject _textComponentParent;
        private DescriptionText _textComponent;

        void Start()
        {
            Image img = GetComponent<Image>();
            img.alphaHitTestMinimumThreshold = 0.1f;
            if (DifficultyManager.Instance.currentDifficulty == _myDifficulty) _myArrow.SetActive(true);

            DifficultyManager.Instance.OnDifficultyChange += HandleDifficultyChange;
            _textComponent = _textComponentParent.GetComponent<DescriptionText>();
        }
        public void OnPointerClick(PointerEventData eventData) // Change the level dificulty and pass to the next scene.
        {
            DifficultyManager.Instance.SetDifficulty(_myDifficulty);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _myArrow.SetActive(true);
            _textComponent.UpdateText(_myDifficulty);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (DifficultyManager.Instance.currentDifficulty != _myDifficulty)
            {
                _myArrow.SetActive(false);
                _textComponent.UpdateText(DifficultyManager.Instance.currentDifficulty);
            }

        }

        private void HandleDifficultyChange(Difficulty newDifficulty)
        {
            _myArrow.SetActive(newDifficulty == _myDifficulty);
        }

        private void OnDestroy()
        {
            DifficultyManager.Instance.OnDifficultyChange -= HandleDifficultyChange;
        }
    }
}
