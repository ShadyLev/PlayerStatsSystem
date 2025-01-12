using UnityEditor.Embree;
using UnityEngine;

namespace Code.Singletons
{
    /// <summary>
    /// Persistent Singleton, will destroy any other older versions of same type component it finds on awake.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RegulatorSingleton<T> : MonoBehaviour where T : Component
    {
        protected static T _instance;
        
        public static bool HasInstance => _instance != null;

        public float InitializationTime { get; private set; }
        
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<T>();
                    if (_instance == null)
                    {
                        var go = new GameObject(typeof(T).Name + "Auto-Generated");
                        go.hideFlags = HideFlags.HideAndDontSave;
                        _instance = go.AddComponent<T>();
                    }
                }
                
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            InitializeSingleton();
        }

        protected virtual void InitializeSingleton()
        {
            if(Application.isPlaying == false)
                return;
            
            InitializationTime = Time.time;
            DontDestroyOnLoad(gameObject);

            var oldInstances = FindObjectsByType<T>(FindObjectsSortMode.None);
            foreach (var oldInstance in oldInstances)
            {
                if(oldInstance.GetComponent<RegulatorSingleton<T>>().InitializationTime < InitializationTime)
                {
                    Destroy(oldInstance.gameObject);
                }
            }
            
            if (_instance == null)
            {
                _instance = this as T;
            }
        }
    }
}