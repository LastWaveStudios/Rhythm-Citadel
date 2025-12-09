using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace UI
{
    public class NextLevel : MonoBehaviour, IPointerClickHandler
    {
        void Start()
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            if (currentScene + 1 >= SceneManager.sceneCountInBuildSettings)
            {
                gameObject.SetActive(false);
            }
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