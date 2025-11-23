using UnityEngine;
using UnityEngine.EventSystems;

public class BallerinaController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator _animator;
    private float lastPoint = 0f;
    public void OnPointerEnter(PointerEventData eventData)
    {
        _animator.speed = 1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _animator.speed = 0f;
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.speed = 0f;
    }

}
