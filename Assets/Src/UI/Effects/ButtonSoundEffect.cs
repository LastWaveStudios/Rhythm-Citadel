using Audio;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Effects
{
    public class ButtonSoundEffect : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [SerializeField] private AudioClip _enterHoverSound;
        [SerializeField] private AudioClip _clickSound;
        [SerializeField] private AudioClip _exitHoverSound;


        public void OnPointerEnter(PointerEventData eventData)
        {
            AudioManager.Instance.PlayOneShot2D(_enterHoverSound);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            AudioManager.Instance.PlayOneShot2D(_clickSound);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            AudioManager.Instance.PlayOneShot2D(_exitHoverSound);
        }
    }
}