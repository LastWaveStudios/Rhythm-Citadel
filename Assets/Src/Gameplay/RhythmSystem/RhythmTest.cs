using System;
using UnityEngine;

namespace Gameplay.RhythmSystem
{
    public class RhythmTest : MonoBehaviour
    {
        [SerializeField] private RhythmPattern pattern;
        [SerializeField] private Signature signatureToUseInTest;
        [SerializeField] private int BPM;
        [SerializeField] private double maxOffset;

        private double _timeOfLastJ = 0.0;

        public void Start ()
        {
            RhythmManager.Instance.UseMeasure(signatureToUseInTest, BPM);
            RhythmManager.Instance.ResetCounts();
            RhythmManager.Instance.StartRhythm();

            RhythmManager.Instance.onQuarter += OnQuarter;

        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                _timeOfLastJ = AudioSettings.dspTime;
            }
        }

        private void OnQuarter(double deltaTime)
        {
            double timeSinceLastJ = (AudioSettings.dspTime - _timeOfLastJ) * 1000.0;
            if (timeSinceLastJ <= maxOffset)
            {
                Debug.Log($"Tapped Correctly: {timeSinceLastJ}");
            }
            else
            {
                Debug.Log($"Tapped Incorrectly: {timeSinceLastJ}");
            }
        }
    }
}