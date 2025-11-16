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

            //DontDestroyOnLoad(Instance);
        }
       /* protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning($"{typeof(T).Name}: más de una instancia en escena. Eliminando duplicado.");
                Destroy(gameObject);
                return;
            }

            Instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }*/
    }
}
