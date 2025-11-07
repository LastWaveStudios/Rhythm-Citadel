using System.Collections.Generic;
using UnityEngine;

namespace Audio.SpecificImplementations
{
    public class CitadelMarchController : MonoBehaviour
    {
        [SerializeField] private List<AudioClipRhythm> _clipsRhythms;
        private int _currentClipRhythm = 0;

        void Start()
        {
            for (int i = 0; i < _clipsRhythms.Count; ++i)
            {
                AudioClipRhythm clipRhythm = _clipsRhythms[i];
                clipRhythm.audioSource = gameObject.AddComponent<AudioSource>();
                clipRhythm.audioSource.loop = false;
                clipRhythm.audioSource.spatialBlend = 0.0f;
                clipRhythm.audioSource.outputAudioMixerGroup = AudioManager.Instance.audioControl.FindMatchingGroups("Music")[0];
                Debug.Log(clipRhythm.audioSource.outputAudioMixerGroup.name);
                clipRhythm.audioSource.clip = clipRhythm.audioClip;
                _clipsRhythms[i] = clipRhythm;
            }
        }

        public void Play()
        {
            _currentClipRhythm = 0;
            _clipsRhythms[_currentClipRhythm].audioSource.Play();
        }
    }
}

