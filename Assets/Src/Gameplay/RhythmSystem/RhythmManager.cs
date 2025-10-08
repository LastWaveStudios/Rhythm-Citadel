using System;
using UnityEngine;

namespace Gameplay.RhythmSystem
{
    public class RhythmManager : Utilities.Singleton<RhythmManager>
    {
        #region MusicUnits
        public Signature signature { get; private set; }
        public int BPM { get; private set; }
        #endregion

        #region TimesForBeats
        private TimesForBPMAndSignature _times;
        private int _measureCount = 0;
        private int _beatCount = 0;
        private int _currentBeatOnMeasure = 0;
        private int _currentHalfbeatOnMeasure = 0;

        private double _timeSinceLastHalfbeat = 0.0;
        #endregion

        private bool _isPlaying = false;


        public void UseMeasure(Signature signatureToUse, int BPMToUse)
        {
            signature = signatureToUse;
            BPM = BPMToUse;

            _times = new TimesForBPMAndSignature(signature, BPM);
        }


        // Update is called once per frame
        public void Update()
        {
            if (!_isPlaying) return;
            

        }


        public void StartRhythm()
        {
            _isPlaying = true;
        }

        public void EndRhythm()
        {
            _isPlaying = false;
        }
    }
}

