using Gameplay.RhythmSystem;
using System.Collections.Generic;


namespace Gameplay.Towers
{
    public enum CheckRhythmStatus
    {
        None = 0,
        Good,
        Bad,
        SkipActions
    }

    public class TowersGroup
    {
        private RhythmPattern _pattern;
        private List<ATower> _towers;

        public double timeOfDisable; // Just base time for the disbale duration in the group, the timing thing is controlled by the manager
        public bool isEnabled;

        private int _indexOfNoteInPattern;
        private double _maxOffset;

        public TowersGroup(RhythmPattern pattern, double timeOfDisable, double maxOffset)
        {
            _pattern = pattern;
            _towers = new List<ATower>();
            _indexOfNoteInPattern = -1;
            this.timeOfDisable = timeOfDisable;
            isEnabled = true;
            _maxOffset = maxOffset;
        }

        public TowersGroup(double timeOfDisable, double maxOffset)
        {
            _pattern = new RhythmPattern(new Signature(4, 4));
            for (int i = 0; i < 4; ++i)
            {
                _pattern.AddNote(new Note(NoteDuration.Quarter));
            }

            _towers = new List<ATower>();
            _indexOfNoteInPattern = -1;
            this.timeOfDisable = timeOfDisable;
            isEnabled = true;
            _maxOffset = maxOffset;
        }

        public void AddTower(ATower tower)
        {
            _towers.Add(tower);
        }

        public void RemoveTower(ATower tower)
        {
            _towers.Remove(tower);
        }

        public void SetPattern(RhythmPattern pattern) { _pattern = pattern; }
        public void SetMaxOffset(double maxOffset) { _maxOffset = maxOffset; }

        public CheckRhythmStatus CheckRhythmForGroup(double CurrentTime)
        {
            if (!isEnabled) return CheckRhythmStatus.SkipActions;
            if (_towers.Count == 0) return CheckRhythmStatus.SkipActions;

            _indexOfNoteInPattern = (_indexOfNoteInPattern + 1) % _pattern.patternNotes.Count;

            if (RhythmManager.Instance.IsInTime(_pattern.patternNotes[_indexOfNoteInPattern], _pattern.GetIndexOfSixteenthOnMeasure(_indexOfNoteInPattern), _maxOffset))
            {
                UnityEngine.Debug.Log("Tapped GOOD");
                foreach (ATower tower in _towers)
                {
                    tower.OnRhythmHit();
                }
                return CheckRhythmStatus.Good;
            }

            UnityEngine.Debug.Log("Tapped BAD");
            _indexOfNoteInPattern = -1;
            return CheckRhythmStatus.Bad;
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