using UnityEngine;

namespace Gameplay.RhythmSystem
{
    public class RhythmTest : MonoBehaviour
    {
        [SerializeField] private RhythmPattern pattern;
        [SerializeField] private Signature signatureToUseInTest;
        [SerializeField] private int BPM;
        [SerializeField] private double maxOffset;

        private int _noteCount;

        public void Start ()
        {
            RhythmManager.Instance.UseMeasure(signatureToUseInTest, BPM);
            RhythmManager.Instance.ResetCounts();
            RhythmManager.Instance.StartRhythm();

            _noteCount = -1;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                _noteCount = (_noteCount + 1) % pattern.patternNotes.Count;

                if (RhythmManager.Instance.IsInTime(pattern.patternNotes[_noteCount], pattern.GetIndexOfSixteenthOnMeasure(_noteCount), maxOffset))
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