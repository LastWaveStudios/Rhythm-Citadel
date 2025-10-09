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

        #region Callbacks
        // Decide if delegates or other method for now delegates
        // The delegates are like that for avoid the check if unbound
        public Action onWhole = delegate { };
        public Action onHalf = delegate { };
        public Action<double> onQuarter = delegate { };
        public Action onEighth = delegate { };
        public Action onSixteenth = delegate { };
        #endregion

        #region CountBeats
        private int _measureCount = 0;
        private int _SixteenthCount = 0;
        #endregion

        #region TimesForBeats
        private TimesForBPMAndSignature _timesOfNotes;
        private double _timeSinceLastSixteenth = 0.0; // More small for do the others by module operations of this
        private double _timeSinceLastSixteenthdsp = 0.0;
        private double _startTime = 0.0;
        #endregion

        private bool _isPlaying = false;


        public void UseMeasure(Signature signatureToUse, int BPMToUse)
        {
            signature = signatureToUse;
            BPM = BPMToUse;

            _timesOfNotes = new TimesForBPMAndSignature(signature, BPM);
        }

        public void ResetCounts()
        {
            _measureCount = 0;
            _SixteenthCount = 0;
        }


        // Update is called once per frame
        public void Update()
        {
            if (!_isPlaying) return;
            // A test for this, is basically the same but in theory is better for the audio sync
            //_timeSinceLastSixteenth += Time.deltaTime * 1000.0; // ms
            _timeSinceLastSixteenthdsp = (AudioSettings.dspTime - _startTime) * 1000.0;
            if (/*_timeSinceLastSixteenth >= _timesOfNotes.Sixteenth*/ _timeSinceLastSixteenthdsp >= _timesOfNotes.Sixteenth)
            {
                _timeSinceLastSixteenth = 0.0;
                _startTime = AudioSettings.dspTime;
                _SixteenthCount = (_SixteenthCount + 1) % 16;

                // Callback for Sixteenth
                //Debug.Log("Sixteenth");
                onSixteenth.Invoke();

                // check others callback by count
                if (_SixteenthCount % 2 == 0)
                {
                    // Callback for Eighth
                    //Debug.Log("Eighth");
                    onEighth.Invoke();
                }
                if (_SixteenthCount % 4 == 0)
                {
                    // Callback for Quarter
                    Debug.Log("Quarter");
                    onQuarter.Invoke(Time.deltaTime);
                }
                if (_SixteenthCount % 8 == 0)
                {
                    // Callback for Half
                    //Debug.Log("Half");
                    onHalf.Invoke();
                }
                if (_SixteenthCount % 16 == 0)
                {
                    // Callback for Whole
                    Debug.Log("-------------------------------------------");
                    onWhole.Invoke();
                }
            }
        }


        public void StartRhythm()
        {
            _isPlaying = true;
            _startTime = AudioSettings.dspTime;
        }

        public void EndRhythm()
        {
            _isPlaying = false;
        }
    }
}

