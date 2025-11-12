using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class AButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public string name;
    public TextMeshProUGUI text;
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
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
