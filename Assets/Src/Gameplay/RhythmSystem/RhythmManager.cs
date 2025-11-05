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
        // If one enemy moves in wholes but the signature is not x / x and is y / x with y < x,
        // the whole callback won't be called, the same is true by the others similar cases in small top signatures
        public Action onWhole = delegate { };
        public Action onHalf = delegate { };
        public Action onQuarter = delegate { };
        public Action onEighth = delegate { };
        public Action onSixteenth = delegate { };
        #endregion

        #region CountBeats
        public int MeasureCount {get; private set;}
        public int SixteenthCount {get; private set;}
        public int SixteenthCountGlobal { get; private set; }
        #endregion

        #region TimesForBeats
        private TimesForBPMAndSignature _timesOfNotes;
        private double _timeSinceLastSixteenth; // More small for do the others by module operations of this
        private double _lastSixteenth;
        private double _startTime;
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
            MeasureCount = -1;
            SixteenthCount = -1;
            SixteenthCountGlobal = -1;
        }


        // Update is called once per frame
        public void Update()
        {
            if (!_isPlaying) return;

            _timeSinceLastSixteenth = (AudioSettings.dspTime - _lastSixteenth) * 1000.0; // ms
            if (_timeSinceLastSixteenth >= _timesOfNotes.Sixteenth)
            {
                _lastSixteenth = AudioSettings.dspTime - (_timeSinceLastSixteenth - _timesOfNotes.Sixteenth) / 1000; // again to seconds
                _timeSinceLastSixteenth = 0.0;
                SixteenthCount = (SixteenthCount + 1) % (int)signature.maxSixteenthsOnOneMeasure;
                SixteenthCountGlobal++;

                if (SixteenthCount == 0)
                {
                    Debug.Log("-------------------------------------------");
                    MeasureCount++;
                }

                // Callback for Sixteenth
                //Debug.Log("Sixteenth");
                onSixteenth.Invoke();

                // check others callback by count
                if (SixteenthCountGlobal % 2 == 0)
                {
                    // Callback for Eighth
                    Debug.Log("Eighth");
                    onEighth.Invoke();
                }
                if (SixteenthCountGlobal % 4 == 0)
                {
                    // Callback for Quarter
                    //Debug.Log("Quarter");
                    onQuarter.Invoke();
                }
                if (SixteenthCountGlobal % 8 == 0)
                {
                    // Callback for Half
                    //Debug.Log("Half");
                    onHalf.Invoke();
                }
                if (SixteenthCountGlobal % 16 == 0)
                {
                    // Callback for Whole
                    //Debug.Log("Whole");
                    onWhole.Invoke();
                }
            }
        }

        public bool IsInTime(Note note, uint indexOfSixteenthOnMeasure, double maxOffset)
        {
            double timeSinceStart = (AudioSettings.dspTime - _startTime) * 1000.0;

            double timeOfLastMeasureSinceStart = MeasureCount * signature.maxSixteenthsOnOneMeasure * _timesOfNotes.Sixteenth;
            
            if (SixteenthCount == 0 && indexOfSixteenthOnMeasure == signature.maxSixteenthsOnOneMeasure - 1)
            {
                Debug.Log($"TIME: 1 -> SixteenthCount: {SixteenthCount}; TargetSixteenth: {indexOfSixteenthOnMeasure};" +
                $" timeOfLastMeasureSinceStart: {timeOfLastMeasureSinceStart}; timeSinceStart: {timeSinceStart}; rawOffsetTime: {timeOfLastMeasureSinceStart - timeSinceStart}");
                return (Math.Abs(timeOfLastMeasureSinceStart - timeSinceStart) <= maxOffset) ? true : false;
            }

            if (SixteenthCount == signature.maxSixteenthsOnOneMeasure - 1 && indexOfSixteenthOnMeasure == 0)
            {
                double timeOfTargetSinceStartSpecialCase = timeOfLastMeasureSinceStart + signature.maxSixteenthsOnOneMeasure * _timesOfNotes.Sixteenth;
                Debug.Log($"TIME: 2 -> SixteenthCount: {SixteenthCount}; TargetSixteenth: {indexOfSixteenthOnMeasure};" +
               $" timeOfLastMeasureSinceStart: {timeOfLastMeasureSinceStart}; timeSinceStart: {timeSinceStart}; rawOffsetTime: {timeOfTargetSinceStartSpecialCase - timeSinceStart}");
                return (Math.Abs(timeOfTargetSinceStartSpecialCase - timeSinceStart) <= maxOffset) ? true : false;
            }

            double timeOfTargetSinceStart = timeOfLastMeasureSinceStart + indexOfSixteenthOnMeasure * _timesOfNotes.Sixteenth;
            Debug.Log($"TIME: 3 -> SixteenthCount: {SixteenthCount}; TargetSixteenth: {indexOfSixteenthOnMeasure};" +
                $" timeOfLastMeasureSinceStart: {timeOfLastMeasureSinceStart}; timeSinceStart: {timeSinceStart}; rawOffsetTime: {timeOfTargetSinceStart - timeSinceStart}");
            return (Math.Abs(timeOfTargetSinceStart - timeSinceStart) <= maxOffset) ? true : false;
        }

        public void StartRhythm()
        {
            _isPlaying = true;
            _lastSixteenth = AudioSettings.dspTime - (_timesOfNotes.Sixteenth / 1000); // Pass to seconds again
            _startTime = AudioSettings.dspTime;
            ResetCounts();
        }

        public void EndRhythm()
        {
            _isPlaying = false;
        }
    }
}

