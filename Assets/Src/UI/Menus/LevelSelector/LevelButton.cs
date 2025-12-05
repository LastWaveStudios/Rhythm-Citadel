using UI.Menus;
using UI.Menus.States;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] string _myScene;
    public void OnPointerClick(PointerEventData eventData)
    {
        MenuManager.Instance.ChangeSceneAndState(_myScene, new UI.Menus.States.Gameplay());
    }

}
