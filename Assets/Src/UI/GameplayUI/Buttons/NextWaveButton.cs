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
        private bool _isActive;
        private Vector3 _scale;
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
            _scale = gameObject.transform.localScale;
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
                gameObject.transform.localScale = Vector3.zero;
            }
            //activeImage.sprite = inactiveSprite;

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
        public void WaveClear()
        {
            _isActive = true;
            gameObject.transform.localScale = _scale;
        }

    }
}
