using UnityEngine;


namespace Dweiss
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public bool dontDestroyOnLoad = false;
        private static T _instance;
        public static T S { get { return Inst; } }
        public static T Inst { get { return Instance; } }

        private static bool _init = false;
        public static T Instance { get {

                if(_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<T>();
                    if(_instance == null && _init == false)
                    {

                       new GameObject(typeof(T).Name, typeof(T));

                    }
                    _init = true;
                }
                return _instance;
            } }

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this) throw new System.NotSupportedException("Multiple singelton instances " + this + ", " + _instance);
            _instance = this as T;
            _init = true;

            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(_instance);
            }
        }
        protected virtual void OnDestroy() { _instance = null; }

    }

    public class AutoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static bool autoInit = true;
        public static T S { get { return Inst; } }
        public static T Inst { get {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<T>();
                    if (_instance == null && autoInit)
                    {
                        var createdGo = new GameObject(typeof(T).Name);
                        _instance = createdGo.AddComponent<T>();
                    }
                }
                autoInit = false;

                return _instance;
            }
        }

        public static T Instance{ get { return Inst; } }

        protected virtual void Awake()
        {
            if (_instance != null && _instance.Equals(this as T) == false) throw new System.NotSupportedException(typeof(T)+" Multiple singelton instances " + this + ", " + 
                _instance);
            _instance = this as T;
            autoInit = false;

        }
        
        protected virtual void OnDestroy()
        {
            _instance = null;
        }
       
    }
}