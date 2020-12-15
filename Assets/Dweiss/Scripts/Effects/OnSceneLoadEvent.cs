using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dweiss
{
    public class OnSceneLoadEvent : MonoBehaviour
    {
        public SceneEvent onSceneLoad;
        
        [System.Serializable]
        public class SceneEvent : UnityEngine.Events.UnityEvent<Scene> { }
        // Use this for initialization
        void Start()
        {
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            onSceneLoad.Invoke(SceneManager.GetActiveScene());
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            onSceneLoad.Invoke(arg0);
        }

    }
}