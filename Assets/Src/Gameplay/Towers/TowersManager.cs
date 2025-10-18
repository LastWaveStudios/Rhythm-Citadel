using UnityEngine;
using System.Collections.Generic;
using Gameplay.RhythmSystem;
using System.Collections;
using System;
using Unity.VisualScripting;

namespace Gameplay.Towers
{
    public class TowersManager : Utilities.Singleton<TowersManager>
    {
        // used like a dictionary by keys 0-5 id of input group
        private List<TowersGroup> _towersGroups;

        [SerializeField] private const int NUMBER_OF_GROUPS = 6;
        [SerializeField] private const double DEFAULT_MAX_OFFSET = 200;
        [SerializeField] private const double DEFAULT_TIME_OF_DISABLE = 5000;

        public void Start()
        {
            _towersGroups = new List<TowersGroup>();
            _towersGroups.Capacity = NUMBER_OF_GROUPS;
            for (int i = 0; i < NUMBER_OF_GROUPS; ++i)
            {
                _towersGroups.Add(new TowersGroup(DEFAULT_TIME_OF_DISABLE, DEFAULT_MAX_OFFSET));
            }

            GameInput.InputReader.Instance.onTapGroup += OnTapGroup;
        }

        private void OnTapGroup(int groupIndex)
        {
            if (!IsValidGroupIndex(groupIndex, "OnTapGroup")) return;

            if (!_towersGroups[groupIndex].isEnabled) return;

            CheckRhythmStatus status = _towersGroups[groupIndex].CheckRhythmForGroup(AudioSettings.dspTime);
            switch (status)
            {
                case CheckRhythmStatus.Bad:
                    _towersGroups[groupIndex].DisableGroup();
                    StartCoroutine(EnableGroup(groupIndex));
                    break;
                case CheckRhythmStatus.Good:
                case CheckRhythmStatus.SkipActions:
                    return;
                default:
                    Debug.LogError($"TowersManger::OnTapGroup the CheckRhythmStatus is Unkown: {status}");
                    return;
            }
        }

        private IEnumerator EnableGroup(int groupIndex)
        {
            double startTime = AudioSettings.dspTime;
            while (true)
            {
                if ((AudioSettings.dspTime - startTime) * 1000 >= _towersGroups[groupIndex].timeOfDisable) break;
                yield return null; 
            }

            _towersGroups[groupIndex].EnableGroup();
        }

        public void AddTower(ATower tower, int groupIndex)
        {
            if (!IsValidGroupIndex(groupIndex, "AddTower")) return;

            _towersGroups[groupIndex].AddTower(tower);
        }

        public void RemoveTower(ATower tower, int groupIndex)
        {
            if (!IsValidGroupIndex(groupIndex, "RemoveTower")) return;

            _towersGroups[groupIndex].RemoveTower(tower);
        }

        public void SetPatternGroup(RhythmPattern pattern,  int groupIndex)
        {
            if (!IsValidGroupIndex(groupIndex, "SetPatternGroup")) return;

            _towersGroups[groupIndex].SetPattern(pattern);
        }

        public void SetTimeOfDisableGroup(double timeOfDisable,  int groupIndex)
        {
            if (!IsValidGroupIndex(groupIndex, "SetTimeOfDisableGroup")) return;

            _towersGroups[groupIndex].timeOfDisable = timeOfDisable;
        }

        public void SetMaxOffset(double maxOffset, int groupIndex)
        {
            if (!IsValidGroupIndex(groupIndex, "SetMaxOffset")) return;

            _towersGroups[groupIndex].SetMaxOffset(maxOffset);
        }

        #region Inputs
        

        #endregion

        private bool IsValidGroupIndex(int groupIndex, string methodNameForDebug = "")
        {
            if (groupIndex >= NUMBER_OF_GROUPS && groupIndex < 0)
            {
                Debug.Log($"TowersManager::{methodNameForDebug} the groupIndex is not valid: {groupIndex}; maxIndexAllow: {NUMBER_OF_GROUPS - 1}");
                return false;
            }
            return true;
        }
    }
}