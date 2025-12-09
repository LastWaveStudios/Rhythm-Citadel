using System;
using Gameplay.World;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using Utilities.ServiceLocator;

namespace UI
{
    public class DefeatAnimationController : MonoBehaviour
    {
        private VideoPlayer _videoPlayer;
        [SerializeField] private VideoPlayer _giftVideoPlayer;
        [SerializeField] private VideoClip _defeatVideoClip;
        
        private Dancer _dancer;
        private void Start()
        {
            _videoPlayer = GetComponent<VideoPlayer>();
            _giftVideoPlayer.gameObject.SetActive(false);
            ServiceLocatorSubsystem.SubscribeToInitialice(Init);
        }

        private void Init()
        {
            _dancer = ServiceLocatorSubsystem.Instance.GetService<Dancer>();
            if (_dancer == null)
            {
                Debug.LogError("DefeatAnimationController::Init: Dancer not found");
                return;
            }
            
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            if (currentScene + 1 >= SceneManager.sceneCountInBuildSettings)
            {
                if (_videoPlayer != null)
                {
                    _videoPlayer.clip = _defeatVideoClip;
                    _videoPlayer.loopPointReached += OnVideoEnd;
                    _videoPlayer.prepareCompleted += (VideoPlayer source) => source.Play();
                    _videoPlayer.Prepare();
                }
            }
            else
            {
                _giftVideoPlayer.gameObject.SetActive(true);
                _videoPlayer.gameObject.SetActive(false);
            }
        }
        
        private void OnVideoEnd(VideoPlayer source)
        {
            source.Stop();
            source.loopPointReached -= OnVideoEnd;
            source.gameObject.SetActive(false);
            _giftVideoPlayer.gameObject.SetActive(true);
        }
    }
}