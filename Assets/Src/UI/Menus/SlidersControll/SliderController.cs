using Audio;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Menus.SlidersControll
{
    [RequireComponent(typeof(UnityEngine.UI.Slider))]
    public class SliderController : MonoBehaviour, IDragHandler, IDropHandler
    {
        [SerializeField] private AudioChannel _audioChannel;

        private UnityEngine.UI.Slider _slider;

        private void Start()
        {
            _slider = GetComponent<UnityEngine.UI.Slider>();

            _slider.value = AudioManager.Instance.GetVolumeByChannel(_audioChannel);
        }

        private void SetVolume()
        {
            AudioManager.Instance.SetVolume(_audioChannel, _slider.value);
        }

        public void OnDrag(PointerEventData eventData)
        {
            SetVolume();
        }

        public void OnDrop(PointerEventData eventData)
        {
            SetVolume();
        }
    }
}