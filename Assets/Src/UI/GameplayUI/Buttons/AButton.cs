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
        [SerializeField] protected GameObject _towerInfo;

        public new string name;
        private Animator _animator;

        protected EconomyManager _economyManager;

        protected virtual void Start()
        {
            _animator = GetComponent<Animator>();
            _animator.speed = 0;
            ServiceLocatorSubsystem.SubscribeToInitialice(TakeReferences);
            TakeTextReferences();
        }

        protected virtual void TakeReferences()
        {
            _economyManager = ServiceLocatorSubsystem.Instance.GetService<EconomyManager>();
            if (_economyManager == null) Debug.LogError("AButton::TakeReferences: The EconomyManager was null");

        }

        public abstract void OnPointerClick(PointerEventData eventData);

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            _animator.speed = 1;
            _towerInfo.SetActive(true);
            UpdateTowerInfo();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            _animator.speed = 0;
            _towerInfo.SetActive(false);
        }

        protected abstract void TakeTextReferences();
        protected abstract void UpdateTowerInfo();

    }
}
