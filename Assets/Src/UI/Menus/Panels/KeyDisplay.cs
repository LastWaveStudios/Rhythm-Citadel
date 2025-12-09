using Input;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyDisplay : MonoBehaviour
{
    private TextMeshProUGUI _text;
    [SerializeField] private BuildActions _myGroup;
    private enum BuildActions { Group1, Group2, Group3, Group4, Group5, Group6 }
    void Start()
    {
     _text = GetComponent<TextMeshProUGUI>();
        InputAction myKey = null;
        switch (_myGroup)
        {
            case BuildActions.Group1:
                myKey = InputReader.Instance.actions.Battle.Group1;
                break;
            case BuildActions.Group2:
                myKey = InputReader.Instance.actions.Battle.Group2;
                break;
            case BuildActions.Group3:
                myKey = InputReader.Instance.actions.Battle.Group3;
                break;
            case BuildActions.Group4:
                myKey = InputReader.Instance.actions.Battle.Group4;
                break;
            case BuildActions.Group5:
                myKey = InputReader.Instance.actions.Battle.Group5;
                break;
            case BuildActions.Group6:
                myKey = InputReader.Instance.actions.Battle.Group6;
                break;
        }
        string key = myKey.GetBindingDisplayString();
        _text.text = ("- " + key);
    }


}
