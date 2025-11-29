using System.Collections;
using Gameplay;
using Gameplay.RhythmSystem;
using UI.Menus;
using UI.Menus.States;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utilities.ServiceLocator;

namespace UI.GameplayUI.Buttons
{
    public class NextWaveButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        #region Variables
        private Image activeImage;
        public Sprite normalSprite;
        public Sprite hoverSprite;
        public Sprite inactiveSprite;
        public Sprite inactiveHoverSprite;
        private bool _isActive;
        #endregion

        #region Services
        private GameplayManager _gameplayManager;
        private RhythmManager _rhythmManager;
        #endregion

        #region Monobehaviour
        void Start()
        {
            activeImage = GetComponent<Image>();
            activeImage.sprite = normalSprite;

            _isActive = true;

            _gameplayManager = ServiceLocatorSubsystem.Instance.GetService<GameplayManager>();
            if (_gameplayManager == null)
            {
                Debug.LogError("NextWaveButton::Start: The GameplayManager is null");
                return;
            }

            _rhythmManager = ServiceLocatorSubsystem.Instance.GetService<RhythmManager>();
            if (_rhythmManager == null)
            {
                Debug.LogError("NextWaveButton::Start: The RhythmManager is null");
                return;
            }
            _rhythmManager.onEndRhythm += WaveClear;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isActive)
            {
                _gameplayManager.ChangeFightState();
                _isActive = false;
            }
            activeImage.sprite = inactiveSprite;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isActive)
                activeImage.sprite = hoverSprite;
            else
                activeImage.sprite = inactiveHoverSprite;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isActive)
                activeImage.sprite = normalSprite;  
            else
                activeImage.sprite = inactiveSprite;    
            
        }
        #endregion
        public void WaveClear()
        {
            activeImage.sprite = normalSprite;
            _isActive = true;
        }

    }
}
