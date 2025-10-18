using UnityEngine;
using Gameplay.RhythmSystem;
using System.Collections.Generic;

namespace Gameplay.Towers
{
    public class TowersGroupTest : MonoBehaviour
    {
        [SerializeField] private int groupIndex = 4; 
        [SerializeField] List<TestTower> _towers;
        [SerializeField] private RhythmPattern _pattern;
        [SerializeField] private int BPM = 120;

        private bool isAdded = false;

        private void Update()
        {
            if (!isAdded && Input.GetKeyDown(KeyCode.A))
            {
                TowersManager.Instance.SetPatternGroup(_pattern, groupIndex); // 4 -> the J in the defaults values
                foreach (TestTower tower in _towers)
                {
                    TowersManager.Instance.AddTower(tower, groupIndex);
                }

                isAdded = true;

                RhythmManager.Instance.UseMeasure(_pattern.signature, BPM);
                RhythmManager.Instance.ResetCounts();
                RhythmManager.Instance.StartRhythm();
            }
        }
    }
}