using UI.Menus;
using UI.Menus.States;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Image activeImage;
    [SerializeField] private Sprite _normalSprite;
    [SerializeField] private Sprite _hoverSprite;

    [SerializeField] string _myScene;
    void Start()
    {
        activeImage = GetComponent<Image>();
        activeImage.sprite = _normalSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        activeImage.sprite = _normalSprite;
        transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        MenuManager.Instance.ChangeSceneAndState(_myScene, new UI.Menus.States.Gameplay());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        activeImage.sprite = _hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        activeImage.sprite = _normalSprite;
    }
}
