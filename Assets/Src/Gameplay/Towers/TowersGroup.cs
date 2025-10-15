using Gameplay.RhythmSystem;
using System.Collections.Generic;


namespace Gameplay.Towers
{
    public class TowersGroup
    {
        private RhythmPattern _pattern;
        private List<ATower> _towers;

        public double timeOfDisable; // Just base time for the disbale duration in the group, the timing thing is controlled by the manager
        public bool isEnabled;

        private int _indexOfNoteInPattern;

        public TowersGroup(RhythmPattern pattern, double timeOfDisable)
        {
            _pattern = pattern;
            _towers = new List<ATower>();
            _indexOfNoteInPattern = -1;
            this.timeOfDisable = timeOfDisable;
            isEnabled = true;
        }

        public bool CheckRhythmForGroup(double CurrentTime)
        {
            if (!isEnabled) return false;

            _indexOfNoteInPattern = (_indexOfNoteInPattern + 1) % _pattern.patternNotes.Count;

            if (RhythmManager.Instance.IsInTime(_pattern.patternNotes[_indexOfNoteInPattern], _pattern.GetIndexOfSixteenthOnMeasure(_indexOfNoteInPattern), _maxOffset))
            {
                UnityEngine.Debug.Log("Tapped GOOD");
                foreach (ATower tower in _towers)
                {
                    tower.OnRhythmHit();
                }
                return true;
            }

            UnityEngine.Debug.Log("Tapped BAD");
            _indexOfNoteInPattern = -1;
            DisableGroup();
            return false;
        }

        public void DisableGroup()
        {
            if (!isEnabled) return;

            isEnabled = false;

            foreach(ATower tower in _towers)
            {
                tower.Disable();
            }
        }

        public void EnableGroup()
        {
            if (isEnabled) return;

            isEnabled = true;

            foreach(ATower tower in _towers)
            {
                tower.Enable();
            }
        }
    }
}