using Audio;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Menus.SlidersControll
{
    [RequireComponent(typeof(UnityEngine.UI.Slider))]
    public class SliderController : MonoBehaviour, IDragHandler, IDropHandler, IPointerDownHandler
    {
        [SerializeField] private AudioChannel _audioChannel;
        public float SliderValue { get { return _slider.value; } }

        private UnityEngine.UI.Slider _slider;

        public Action<float> onSliderChange = delegate { };

        private Action<float> onInitialize = delegate { };
        private bool _isInitialize = false;

        private void Start()
        {
            _slider = GetComponent<UnityEngine.UI.Slider>();

            _slider.value = AudioManager.Instance.GetVolumeByChannel(_audioChannel);

            _isInitialize = true;
            onInitialize.Invoke(_slider.value);
            onInitialize = null;
        }

        public void SubscribeToInitialize(Action<float> callback)
        {
            if (_isInitialize)
            {
                callback(SliderValue);
            }
            else
            {
                onInitialize += callback;
            }
        }

        private void SetVolume()
        {
            AudioManager.Instance.SetVolume(_audioChannel, _slider.value);
        }

        public void OnDrag(PointerEventData eventData)
        {
            SetVolume();
            onSliderChange.Invoke(_slider.value);
        }

        public void OnDrop(PointerEventData eventData)
        {
            SetVolume();
            onSliderChange.Invoke(_slider.value);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SetVolume();
            onSliderChange.Invoke(_slider.value);
        }
    }
}