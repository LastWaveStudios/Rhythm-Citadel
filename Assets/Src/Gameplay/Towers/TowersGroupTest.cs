using UnityEngine;
using Gameplay.RhythmSystem;
using System.Collections.Generic;
using Gameplay.Waves;
using Utilities.ServiceLocator;
using System;

namespace Gameplay.Towers
{
    public class TowersGroupTest : MonoBehaviour
    {
        [SerializeField] private int groupIndex = 4; 
        [SerializeField] private RhythmPattern _pattern;

        private bool isAdded = false;

        private TowersManager _towersManager;

        private void Start()
        {
            ServiceLocatorSubsystem.SubscribeToInitialice(Init);
        }

        private void Init()
        {
            _towersManager = ServiceLocatorSubsystem.Instance.GetService<TowersManager>();
        }

        private void Update()
        {
            if (!isAdded && Input.GetKeyDown(KeyCode.A))
            {
                _towersManager.SetPatternGroup(_pattern, groupIndex); // 4 -> the J in the defaults values

                isAdded = true;
            }
        }
    }
}