using UnityEngine;
using UnityEngine.Audio;


namespace Audio
{
    public class AudioManager : Utilities.Singleton<AudioManager>
    {
        #region VolumeControl
        public AudioMixer audioControl;

        private float _masterVol = 0.5f;
        private float _musicVol = 0.5f;
        private float _SFXVol = 0.5f;

        public float MasterVol { get { return _masterVol; } private set { _masterVol = value; } }
        public float MusicVol { get { return _musicVol; } private set { _musicVol = value; } }
        public float SFXVol { get { return _SFXVol; } private set { _SFXVol = value; } }

        private void Start()
        {
            audioControl.SetFloat("Master", ConvertToLogValue(_masterVol));
            audioControl.SetFloat("Music", ConvertToLogValue(_musicVol));
            audioControl.SetFloat("SFX", ConvertToLogValue(_SFXVol));
        }

        public void SetVolume(AudioChannel channel, float value)
        {
            switch (channel)
            {
                case AudioChannel.Master:
                    audioControl.SetFloat("Master", ConvertToLogValue(value));
                    _masterVol = value;
                    break;
                case AudioChannel.Music:
                    audioControl.SetFloat("Music", ConvertToLogValue(value));
                    _musicVol = value;
                    break;
                case AudioChannel.SFX:
                    audioControl.SetFloat("SFX", ConvertToLogValue(value));
                    _SFXVol = value;
                    break;
                default:
                    Debug.Log("AudioManager::SetVolume: ERROR IN SET VOLUME UNKOWN AUDIO CHANNEL: " + channel);
                    return;
            }
        }

        public float GetVolumeByChannel(AudioChannel channel)
        {
            switch (channel)
            {
                case AudioChannel.Master:
                    return MasterVol;
                case AudioChannel.Music:
                    return MusicVol;
                case AudioChannel.SFX:
                    return SFXVol;
                default:
                    Debug.LogError($"ERROR_UNKNOWN_CHANNEL: {channel}");
                    return 0;
            }
        }

        private float ConvertToLogValue(float value)
        {
            return Mathf.Log10(value) * 20;
        }
        #endregion
    }
}