using UnityEngine;

namespace Gameplay.RhythmSystem
{
    public class RhythmTest : MonoBehaviour
    {
        [SerializeField] private RhythmPattern _pattern;
        [SerializeField] private Signature _signatureToUseInTest;
        [SerializeField] private int _BPM;
        [SerializeField] private double _maxOffset;

        private int _noteCount;

        public void Start ()
        {
            RhythmManager.Instance.UseMeasure(_signatureToUseInTest, _BPM);
            RhythmManager.Instance.ResetCounts();
            RhythmManager.Instance.StartRhythm();

            _noteCount = -1;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                _noteCount = (_noteCount + 1) % _pattern.patternNotes.Count;

                if (RhythmManager.Instance.IsInTime(_pattern.patternNotes[_noteCount], _pattern.GetIndexOfSixteenthOnMeasure(_noteCount), _maxOffset))
                {
                    Debug.Log("Tapped GOOD");
                }
                else
                {
                    Debug.Log("Tapped BAD");
                    _noteCount = -1;
                }
            }
        }
    }
}