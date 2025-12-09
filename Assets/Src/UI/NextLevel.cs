using Gameplay.World;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using Utilities.ServiceLocator;

namespace UI
{
    public class NextLevel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private VideoPlayer _giftVideoPlayer;
    [SerializeField] private VideoClip _lowHpVictoryClip;
    [SerializeField] private VideoClip _midHpVictoryClip;
    [SerializeField] private VideoClip _highHpVictoryClip;
    [SerializeField] private VideoClip _veryHighHpVictoryClip;

    private Dancer _dancer;
    
    void Start()
    {
        ServiceLocatorSubsystem.SubscribeToInitialice(Init);
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if (currentScene + 1 >= SceneManager.sceneCountInBuildSettings)
        {
            gameObject.SetActive(false);
            _giftVideoPlayer.gameObject.SetActive(false);
        }
    }

    private void Init()
    {
        _dancer = ServiceLocatorSubsystem.Instance.GetService<Dancer>();
        if (_dancer == null)
        {
            Debug.LogError("NextLevel::Init: Dancer not found");
            return;
        }
        
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if (currentScene + 1 >= SceneManager.sceneCountInBuildSettings)
        {
            gameObject.SetActive(false);
            if (_videoPlayer != null)
            {
                if (_dancer.Health < 25.0f)
                {
                    _videoPlayer.clip =_lowHpVictoryClip;
                }
                else if (_dancer.Health < 50.0f)
                {
                    _videoPlayer.clip = _midHpVictoryClip;
                }
                else if (_dancer.Health < 75.0f)
                {
                    _videoPlayer.clip = _highHpVictoryClip;
                }
                else if (_dancer.Health >= 75.0f)
                {
                    _videoPlayer.clip = _veryHighHpVictoryClip;
                }
                
                _videoPlayer.loopPointReached += OnVideoEnd;
                _videoPlayer.prepareCompleted += (VideoPlayer source) => source.Play();
                _videoPlayer.Prepare();
            }
        }
        else
        {
            _videoPlayer.gameObject.SetActive(false);
            _giftVideoPlayer.gameObject.SetActive(true);
        }
    }

    private void OnVideoEnd(VideoPlayer source)
    {
        source.Stop();
        source.loopPointReached -= OnVideoEnd;
        source.gameObject.SetActive(false);
        _giftVideoPlayer.gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        loadNextLevel();
        
    }

    public void loadNextLevel()
    {
        int currentScene= SceneManager.GetActiveScene().buildIndex;
        Debug.Log(currentScene);
        SceneManager.LoadScene(currentScene + 1);
        /*
        if (currentScene + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(currentScene+1);
        }
        else
        {
            gameObject.SetActive(false);
        }*/
    }
}
}