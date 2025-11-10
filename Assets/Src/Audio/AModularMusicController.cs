using Gameplay.RhythmSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public abstract class AModularMusicController : MonoBehaviour
    {

        [SerializeField] protected List<AudioClipRhythm> _clipsRhythms;

        protected int[] _currentClipsRhythms;
        protected int[] _nextClipsRhythms;

        protected bool _changeOnNextMeasure = false;
        protected int _currentMeasure = 0;
        
        void Start()
        {
            for (int i = 0; i < _clipsRhythms.Count; ++i)
            {
                _clipsRhythms[i].audioSources = new AudioSource[2];
                _clipsRhythms[i].audioSourceIndex = 0;
                for (int j = 0; j < 2; ++j)
                {
                    _clipsRhythms[i].audioSources[j] = gameObject.AddComponent<AudioSource>();
                    _clipsRhythms[i].audioSources[j].loop = false;
                    _clipsRhythms[i].audioSources[j].playOnAwake = false;
                    _clipsRhythms[i].audioSources[j].spatialBlend = 0.0f;
                    _clipsRhythms[i].audioSources[j].outputAudioMixerGroup = AudioManager.Instance.audioControl.FindMatchingGroups("Music")[0];
                    _clipsRhythms[i].audioSources[j].clip = _clipsRhythms[i].audioClip;
                }
            }

            _currentClipsRhythms = InitClips();

            RhythmManager.Instance.onMeasure += OnMeasure;
        }

        /// <summary>
        /// Called on measure, virtual but normaly you will not want to override
        /// </summary>
        virtual protected void OnMeasure()
        {
            _currentMeasure++;
            if (_changeOnNextMeasure)
            {
                _currentMeasure = 1;
                _currentClipsRhythms = _nextClipsRhythms;
                _changeOnNextMeasure = false;
                Debug.Log("Change the clips now");
            }

            List<int> clipsToEnd = new List<int>();
            for (int i = 0; i < _currentClipsRhythms.Length; ++i)
            {
                if (_currentMeasure == _clipsRhythms[_currentClipsRhythms[i]].measuresDuration)
                {
                    clipsToEnd.Add(_currentClipsRhythms[i]);
                }
            }

            if (clipsToEnd.Count > 0)
            {
                _changeOnNextMeasure = true;
                _nextClipsRhythms = SelectNextClips(clipsToEnd.ToArray());

                for (int i = 0; i < _nextClipsRhythms.Length; ++i)
                {
                    _clipsRhythms[_nextClipsRhythms[i]].SwitchAudioSource();
                    Debug.Log($"{_nextClipsRhythms[i]} : Next index of music = {_clipsRhythms[_nextClipsRhythms[i]].audioSourceIndex}");
                    _clipsRhythms[_nextClipsRhythms[i]].audioSources[_clipsRhythms[_nextClipsRhythms[i]].audioSourceIndex].PlayScheduled(RhythmManager.Instance.GetNextMeasureTime());
                }
            }

            // if (_currentMeasure == _clipsRhythms[_currentClipRhythm].measuresDuration) // next measure the clip ends so schedule the next clip for that next measure
            // {
            //     _changeOnNextMeasure = true;
            //     _nextClipRhythm = SelectNextClip(_currentClipRhythm);
            //     _clipsRhythms[_nextClipRhythm].SwitchAudioSource();
            //     Debug.Log($"{_nextClipRhythm} : Next index of music = {_clipsRhythms[_nextClipRhythm].audioSourceIndex}");
            //     _clipsRhythms[_nextClipRhythm].audioSources[_clipsRhythms[_nextClipRhythm].audioSourceIndex].PlayScheduled(RhythmManager.Instance.GetNextMeasureTime());
            // }
        }
        
        
        /// <summary>
        /// Init the clips that will sound, similar to the SelectNextClip
        /// </summary>
        /// <returns>
        /// Must return one Array with the index on the clip array of the songParts, that will be played at the start
        /// </returns>
        protected abstract int[] InitClips();

        /// <summary>
        /// Select the next clips to play
        /// </summary>
        /// <returns>
        /// Must return one Array with the index on the clip array of the songParts, that will be played next
        /// </returns>
        protected abstract int[] SelectNextClips(int[] songPartThatWillEnd);

        /// <summary>
        /// Virtual just in case, first play to do (TODO)
        /// </summary>
        public virtual void Play()
        {

        }
    }
}

