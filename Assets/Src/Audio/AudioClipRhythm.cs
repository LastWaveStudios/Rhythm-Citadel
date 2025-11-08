using UnityEngine;
using Gameplay.RhythmSystem;

namespace Audio
{
    [System.Serializable]
    public class AudioClipRhythm
    {
        [HideInInspector] public AudioSource[] audioSources = new AudioSource[2];
        [HideInInspector] public int audioSourceIndex = 0;
        public AudioClip audioClip;
        public int measuresDuration;

        public AudioClipRhythm(AudioClip audioClip, int measuresDuration)
        {
            this.audioSources = new AudioSource[2];
            this.audioSourceIndex = 0;
            this.audioClip = audioClip;
            this.measuresDuration = measuresDuration;
        }

        public void SwitchAudioSource()
        {
            audioSourceIndex = 1 - audioSourceIndex;
        }
    }
}