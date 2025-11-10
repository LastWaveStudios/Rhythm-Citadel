using UnityEngine;


namespace Utilities
{
    public class Subsystem<T> : MonoBehaviour where T : Component
    {
        public static T Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
            else
            {
                Debug.LogError("Must have just one instance of this subsytem");
                Destroy(this.gameObject);
                return;
            }
        }
    }
}