using UnityEngine;
using Gameplay.RhythmSystem;

namespace Audio
{
    [System.Serializable]
    public struct AudioClipRhythm
    {
        [HideInInspector] public AudioSource audioSource;
        public AudioClip audioClip;
        public Signature signature;
        public int BPM;
        public int measuresDuration;

        public AudioClipRhythm(AudioSource audioSource, AudioClip audioClip, Signature signature, int BPM, int measuresDuration)
        {
            this.audioSource = audioSource;
            this.audioClip = audioClip;
            this.signature = signature;
            this.BPM = BPM;
            this.measuresDuration = measuresDuration;
        }
    }
}