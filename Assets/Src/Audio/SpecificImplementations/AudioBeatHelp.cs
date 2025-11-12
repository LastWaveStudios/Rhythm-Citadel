using UnityEngine;
using Gameplay.RhythmSystem;
using Utilities.ServiceLocator;

namespace Audio.SpecificImplementations
{
    public class AudioBeatHelp : MonoBehaviour
    {
        [SerializeField] private AudioClip _beatClip;
        [SerializeField] private AudioClip _measureBeatClip;
        private AudioSource _source;

        private RhythmManager _rhythmManager;

        private void Start()
        {
            ServiceLocatorSubsystem.SubscribeToInitialice(Init);
        }

        private void Init()
        {
            _rhythmManager = ServiceLocatorSubsystem.Instance.GetService<RhythmManager>();
            if (_rhythmManager == null)
            {
                Debug.LogError("AudioBeatHelp::Init: The RhythmManager is null");
                return;
            }

            _source = gameObject.AddComponent<AudioSource>();
            _source.loop = false;
            _source.playOnAwake = false;
            _source.spatialBlend = 0.0f;
            _source.outputAudioMixerGroup = AudioManager.Instance.audioControl.FindMatchingGroups("SFX")[0];

            _rhythmManager.onBeat += OnBeat;
        }

        private void OnBeat(bool isMeasure)
        {
            if (isMeasure)
            {
                _source.PlayOneShot(_measureBeatClip);
                return;
            }
            
            _source.PlayOneShot(_beatClip);
        }

        private void OnDestroy()
        {
            if (_rhythmManager != null )
            {
                _rhythmManager.onBeat -= OnBeat;
            }
        }
    }
}