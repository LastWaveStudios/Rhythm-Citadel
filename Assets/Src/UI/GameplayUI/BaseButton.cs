using Gameplay;
using System;
using TMPro;
using UnityEngine;
using Utilities.ServiceLocator;

public class BaseButton : MonoBehaviour
{
    private Animator _animator;
    private bool _selected;
    [SerializeField] private GameObject _towerToInstance = null;

    public string name;
    public TextMeshProUGUI text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private EconomyManager _economyManager;

    void Start()
    {
        _animator = GetComponent<Animator>();

        ServiceLocatorSubsystem.SubscribeToInitialice(Init);
    }

    private void Init()
    {
        _economyManager = ServiceLocatorSubsystem.Instance.GetService<EconomyManager>();
        if (_economyManager == null)
        {
            Debug.LogError("BaseButton::Init: The EconomyManager is null");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BuildTower()
    {
        Debug.Log("Selected");
        _economyManager.TryBuyTower(_towerToInstance);
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
        _economyManager.UpdateTower();
    }

    public void SellTower()
    {
        _economyManager.SellTower();
    }
}
