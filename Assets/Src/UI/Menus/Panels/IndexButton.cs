using UnityEngine;
using UnityEngine.EventSystems;

public class IndexButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject _menuReference;
    [SerializeField] private int _idxToGo;
    private ChangePnaels _panelScript;

    public void OnPointerClick(PointerEventData eventData)
    {
        _panelScript.GoToPanel(_idxToGo);
    }

    void Start()
    {
        _panelScript = _menuReference.GetComponent<ChangePnaels>();
    }

}
