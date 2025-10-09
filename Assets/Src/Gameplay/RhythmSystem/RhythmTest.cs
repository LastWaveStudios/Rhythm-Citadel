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

        private double timeSinceStart = 0.0;
        private double timeOfLastJ = 0.0;

        public void Start ()
        {
            RhythmManager.Instance.UseMeasure(signatureToUseInTest, BPM);
            RhythmManager.Instance.ResetCounts();
            RhythmManager.Instance.StartRhythm();

            RhythmManager.Instance.onQuarter += OnQuarter;

            timeSinceStart = AudioSettings.dspTime;
        }

        public void Update()
        {
            
        }

        private void OnQuarter(double deltaTime)
        {
            double timeSinceLastJ = timeOfLastJ * 1000.0;
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