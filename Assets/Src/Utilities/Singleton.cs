using UnityEngine;


namespace Utilities
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance { get; private set; }

        protected void Awake()
        {
            Debug.Log("Singleton Awake ejecutado");
            if (Instance == null)
            {
                Instance = this as T;
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }

            DontDestroyOnLoad(Instance);
        }
    }
}
