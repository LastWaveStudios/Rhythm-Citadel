using Gameplay;
using TMPro;
using UnityEngine;

public class BaseButton : MonoBehaviour
{
    private Animator _animator;
    private bool _selected;
    [SerializeField] private GameObject _towerToInstance = null;

    public string name;
    public TextMeshProUGUI text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BuildTower()
    {
        Debug.Log("Selected");
        EconomyManager.Instance.TryBuyTower(_towerToInstance);
    }

    public void HoverEnter()
    {
        _animator.SetBool("hover", true);
        text.text = name;
    }

    public void HoverExit()
    {
        _animator.SetBool("hover", false);
        text.text = "";
    }

    public void UpdateTower()
    {
        EconomyManager.Instance.UpdateTower();
    }

    public void SellTower()
    {
        EconomyManager.Instance.SellTower();
    }
}
