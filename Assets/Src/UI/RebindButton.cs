using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Input;

namespace UI
{
   public class RebindButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        #region Variables
        private Image activeImage;
        [SerializeField] private Sprite normalSprite;
        [SerializeField] private Sprite hoverSprite;
        private bool _isActive;
        private InputAction actionToRebind;
        [SerializeField] private TextMeshProUGUI keybindText;
    
        public enum BuildActions {Group1, Group2, Group3, Group4, Group5, Group6}
        [SerializeField] private BuildActions actionToRebindEnum;
        #endregion
    
        #region Monobehaviour
        void Start()
        {
            activeImage = GetComponent<Image>();
            activeImage.sprite = normalSprite;
            _isActive = false;
    
            switch (actionToRebindEnum)
            {
                case BuildActions.Group1:
                    actionToRebind = InputReader.Instance.actions.Battle.Group1;
                    break;
                case BuildActions.Group2:
                    actionToRebind = InputReader.Instance.actions.Battle.Group2;
                    break;
                case BuildActions.Group3:
                    actionToRebind = InputReader.Instance.actions.Battle.Group3;
                    break;
                case BuildActions.Group4:
                    actionToRebind = InputReader.Instance.actions.Battle.Group4;
                    break;
                case BuildActions.Group5:
                    actionToRebind = InputReader.Instance.actions.Battle.Group5;
                    break;
                case BuildActions.Group6:
                    actionToRebind = InputReader.Instance.actions.Battle.Group6;
                    break;
            }
            UpdateKeybindText();
        }
    
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_isActive)
            {
                _isActive = true;
                activeImage.sprite = normalSprite;
                Rebind();
            }
        }
    
        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isActive)
                activeImage.sprite = normalSprite;
        }
    
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isActive)
                activeImage.sprite = hoverSprite;
        }
        #endregion
    
        private void Rebind()
        {
            actionToRebind.Disable();
    
            actionToRebind.PerformInteractiveRebinding().WithControlsExcluding("<Mouse>").OnComplete(operation =>
            {
                actionToRebind.Enable();
                operation.Dispose();
                _isActive = false;
                UpdateKeybindText();
                Debug.Log($"{actionToRebind.name} rebount to: {actionToRebind.bindings[0].effectivePath}");
            }).Start();
        }
        private void UpdateKeybindText()
        {
            string displayString = actionToRebind.GetBindingDisplayString();
            keybindText.text = displayString;
        }
    }
}
