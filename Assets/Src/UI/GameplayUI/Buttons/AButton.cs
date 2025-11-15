using Gameplay;
using Gameplay.World;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities.ServiceLocator;

namespace UI.GameplayUI.Buttons
{
    public abstract class AButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public string name;
        public TextMeshProUGUI text;
        private Animator _animator;

        protected EconomyManager _economyManager;

        void Start()
        {
            _animator = GetComponent<Animator>();
            ServiceLocatorSubsystem.SubscribeToInitialice(TakeReferences);
        }

        private void TakeReferences()
        {
            _economyManager = ServiceLocatorSubsystem.Instance.GetService<EconomyManager>();
            if (_economyManager == null)
            {
                Debug.LogError("AButton::TakeReferences: The EconomyManager was null");
            }
        }

        public abstract void OnPointerClick(PointerEventData eventData);

        public void OnPointerEnter(PointerEventData eventData)
        {
            _animator.SetBool("hover", true);
            text.text = name;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _animator.SetBool("hover", false);
            text.text = "";
        }
    }
}
