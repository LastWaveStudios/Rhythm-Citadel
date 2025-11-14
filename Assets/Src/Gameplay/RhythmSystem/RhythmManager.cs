using System;
using UnityEngine;
using Utilities.ServiceLocator;

namespace Gameplay.RhythmSystem
{
    public class RhythmManager : Utilities.ServiceLocator.AService
    {
        #region MusicUnits
        [SerializeField] private Signature _signature;
        [SerializeField] private int _BPM;
        public Signature Signature { get { return _signature; } }
        public int BPM { get { return _BPM; } }
        #endregion

        #region Callbacks
        // Decide if delegates or other method for now delegates
        // The delegates are like that for avoid the check if unbound
        // If one enemy moves in wholes but the signature is not x / x and is y / x with y < x,
        // the whole callback won't be called, the same is true by the others similar cases in small top signatures
        public Action<bool> onBeat = delegate { }; // The param is true if is the first beat of the measure and false otherwise
        public Action onMeasure = delegate { };
        public Action onWhole = delegate { };
        public Action onHalf = delegate { };
        public Action onQuarter = delegate { };
        public Action onEighth = delegate { };
        public Action onSixteenth = delegate { };

        public Action onRhythmStart = delegate { };
        public Action onFinishRhythmNextMeasure = delegate { };
        public Action onEndRhythm = delegate { };
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

        #region Pause variables
        private bool _isPlaying = false;
        private double _timeSinceLastSixteenthWhenPaused = 0.0;

        private int _beatCountSinceStopRequest = 0;
        private bool _isFinishRequest = false;
        private int _beatsUntilStop = 0;
        private int _sixteenthsOnOneBeat = 0;
        #endregion

        #region Services references
        private GameplayManager _gameplayManager;
        #endregion

        public override void Init()
        {
            _timesOfNotes = new TimesForBPMAndSignature(Signature, BPM);

            _gameplayManager = ServiceLocatorSubsystem.Instance.GetService<GameplayManager>();
            _gameplayManager.onFightStateStart += StartRhythm;
            _gameplayManager.onFinishRhythmStateStart += FinishRhythmNextFullMeasure;
            _gameplayManager.onPause += OnPause;
            _gameplayManager.onResume += OnResume;
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
                SixteenthCount = (SixteenthCount + 1) % (int)Signature.maxSixteenthsOnOneMeasure;
                SixteenthCountGlobal++;

                if (SixteenthCount == 0)
                {
                    Debug.Log("-------------------------------------------");
                    MeasureCount++;
                    onMeasure.Invoke(); // Important do the callback after the count increase for the logic of GetNextMeasureTime
                }

                
                if (SixteenthCount % _sixteenthsOnOneBeat == 0)
                {
                    onBeat.Invoke(SixteenthCount == 0);
                    if (_isFinishRequest)
                    {
                        _beatCountSinceStopRequest++;
                        if (_beatCountSinceStopRequest == _beatsUntilStop)
                        {
                            EndRhythm();
                        }
                    }
                }

                // Callback for Sixteenth
                //Debug.Log("Sixteenth");
                onSixteenth.Invoke();

                // check others callback by count
                if (SixteenthCountGlobal % 2 == 0)
                {
                    // Callback for Eighth
                    //Debug.Log("Eighth");
                    onEighth.Invoke();
                }
                if (SixteenthCountGlobal % 4 == 0)
                {
                    // Callback for Quarter
                    Debug.Log("Quarter");
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

        /// <summary>
        /// Gets the AudioSetting time for the next measure in the AudioSettings timeline (seconds)
        /// </summary>
        public double GetNextMeasureTime()
        {
            double timePassedSinceLastMeasure = _timeSinceLastSixteenth + SixteenthCount * _timesOfNotes.Sixteenth;
            double timeOfOneMeasure = Signature.maxSixteenthsOnOneMeasure * _timesOfNotes.Sixteenth;
            return ((AudioSettings.dspTime * 1000.0) + (timeOfOneMeasure - timePassedSinceLastMeasure)) / 1000.0;
        }

        public bool IsInTime(Note note, uint indexOfSixteenthOnMeasure, double maxOffset)
        {
            double timeSinceStart = (AudioSettings.dspTime - _startTime) * 1000.0;

            double timeOfLastMeasureSinceStart = MeasureCount * Signature.maxSixteenthsOnOneMeasure * _timesOfNotes.Sixteenth;
            
            if (SixteenthCount == 0 && indexOfSixteenthOnMeasure == Signature.maxSixteenthsOnOneMeasure - 1)
            {
                Debug.Log($"TIME: 1 -> SixteenthCount: {SixteenthCount}; TargetSixteenth: {indexOfSixteenthOnMeasure};" +
                $" timeOfLastMeasureSinceStart: {timeOfLastMeasureSinceStart}; timeSinceStart: {timeSinceStart}; rawOffsetTime: {timeOfLastMeasureSinceStart - timeSinceStart}");
                return (Math.Abs(timeOfLastMeasureSinceStart - timeSinceStart) <= maxOffset) ? true : false;
            }

            if (SixteenthCount == Signature.maxSixteenthsOnOneMeasure - 1 && indexOfSixteenthOnMeasure == 0)
            {
                double timeOfTargetSinceStartSpecialCase = timeOfLastMeasureSinceStart + Signature.maxSixteenthsOnOneMeasure * _timesOfNotes.Sixteenth;
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
            _isFinishRequest = false;
            _sixteenthsOnOneBeat = 16 / (int)Signature.bottom;
            _lastSixteenth = AudioSettings.dspTime - (_timesOfNotes.Sixteenth / 1000); // Pass to seconds again
            _startTime = AudioSettings.dspTime;
            ResetCounts();

            onRhythmStart.Invoke();
        }

        private void FinishRhythmNextFullMeasure()
        {
            // TODO: Stop the manager in the next measure + this or this if this measure don't give any sixteenth
            _beatsUntilStop = (int)Signature.top;
            if (SixteenthCount > 0)
            {
                _beatsUntilStop += (int)Signature.top - (SixteenthCount / _sixteenthsOnOneBeat);
            }
            _beatCountSinceStopRequest = 0;
            _isFinishRequest = true;

            onFinishRhythmNextMeasure.Invoke();
        }

        public void Pause()
        {
            //if (GameplayManager.Instance.Currentstate == GameplayState.Build) return;

            _isPlaying = false;
            _timeSinceLastSixteenthWhenPaused = AudioSettings.dspTime - _lastSixteenth;
        }

        public void Resume()
        {
            //if (GameplayManager.Instance.Currentstate == GameplayState.Build) return;

            _isPlaying = true;
            _lastSixteenth = AudioSettings.dspTime - _timeSinceLastSixteenthWhenPaused;
        }

        private void OnResume(GameplayState state)
        {
            if (state == GameplayState.Build) return;

            Resume();
        }

        private void OnPause(GameplayState state)
        {
            if (state == GameplayState.Build) return;

            Pause();
        }

        public void EndRhythm()
        {
            _isPlaying = false;
            onEndRhythm.Invoke();
        }

        private void OnDestroy()
        {
            _gameplayManager.onFightStateStart -= StartRhythm;
            _gameplayManager.onFightStateEnd -= EndRhythm;
            _gameplayManager.onPause -= OnPause;
            _gameplayManager.onResume -= OnResume;
        }
    }
}

