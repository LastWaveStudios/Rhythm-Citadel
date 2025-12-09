using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class IndexButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _menuReference;
    [SerializeField] private int _idxToGo;
    private ChangePnaels _panelScript;

    public Color hoverColor = Color.yellow;
    private Color _originalColor = Color.white;

    private TextMeshProUGUI _textReference;

    void Start()
    {
        _panelScript = _menuReference.GetComponent<ChangePnaels>();
        _textReference = GetComponent<TextMeshProUGUI>();
        _originalColor = _textReference.color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _panelScript.GoToPanel(_idxToGo);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _textReference.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _textReference.color = _originalColor;
    }
}
