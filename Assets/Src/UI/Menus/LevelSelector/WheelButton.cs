using Gameplay;
using Gameplay.World;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Menus.DificultySelector
{

    public class WheelButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _myArrow;
        [SerializeField] private Difficulty _myDifficulty;
        void Start()
        {
            Image img = GetComponent<Image>();
            img.alphaHitTestMinimumThreshold = 0.1f;
            if (DifficultyManager.Instance.currentDifficulty == _myDifficulty) _myArrow.SetActive(true);

            DifficultyManager.Instance.OnDifficultyChange += HandleDifficultyChange;
        }
        public void OnPointerClick(PointerEventData eventData) // Change the level dificulty and pass to the next scene.
        {
            DifficultyManager.Instance.SetDifficulty(_myDifficulty);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _myArrow.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(DifficultyManager.Instance.currentDifficulty != _myDifficulty) _myArrow.SetActive(false);
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
