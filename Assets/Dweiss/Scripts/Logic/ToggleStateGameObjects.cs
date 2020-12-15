using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dweiss
{
    public class ToggleStateGameObjects : MonoBehaviour
    {

        public EventString beforeChange, afterChanged;
        public EventInt afterChange;

        public float delayBeforeChange = .2f;

        private bool _isLoading;

        [ArrayElementTitle("Description")]
        [SerializeField]private State[] statesByOrder;

        [System.Serializable]
        public class State
        {

            public int qualitySettings = -1;

            public string Description()
            {
#if UNITY_EDITOR
                var scences = UnityEditor.EditorBuildSettings.scenes;
                if (SceneManager.sceneCountInBuildSettings <= sceneId)
                {
                    return "Scene NULL";
                }
                
                var scene = scences[sceneId];
                return string.Format("{1} (Build #{0}) Q:{2}", sceneId, scene.path.Substring(scene.path.LastIndexOf("/")+1), qualitySettings);
#else

                var scene = Scene;
                return scene.HasValue == false ? "Scene NULL": 
                    string.Format("{0}-{1} {2} Q:{3}", sceneId, scene.Value.path, (scene.Value.isLoaded ? " Loaded" : ""), qualitySettings);
#endif
            }

            public int sceneId;
            private Scene? _myScene;
            private Scene? Scene
            {
                get
                {
                    try
                    {

                        if (_myScene == null)
                            _myScene = SceneManager.GetSceneByBuildIndex(sceneId);
                        return _myScene;
                    }
                    catch (System.Exception)
                    {
                        return null;

                    }
                }
            }
            public Scene GetScene() { return Scene.Value; }

            public bool LoadedToScene
            {
                get { return Scene.Value.isLoaded; }
            }
            public AsyncOperation LoadToScene()
            {
                var ret =  SceneManager.LoadSceneAsync(sceneId, LoadSceneMode.Additive);
                return ret;
            }
            public bool Active
            {
                get;
                set;
            }
        }



        public int startingStateIndex = 10;

        private IEnumerator Start()
        {

            yield return new WaitForSecondsRealtime(0f);
            Init();

            yield return new WaitForSecondsRealtime(.5f);

            ActivateState(startingStateIndex);
        }

        private void Init()
        {
            UnloadScenes(-1);
        }

        public void UnloadScenes(int except)
        {
            Debug.Log("Unload scenes");
            
            for (int i = 0; i < statesByOrder.Length; ++i)
            {
                try
                {
                    if (i != except)
                    {
                        var scene = SceneManager.GetSceneByBuildIndex(statesByOrder[i].sceneId);
                        var go = scene.GetRootGameObjects();
                        foreach (var g in go)
                        {
                            g.SetActive(false);
                            //Destroy(g);
                        }
                        SceneManager.UnloadScene(statesByOrder[i].sceneId);
                    }
                }
                catch (System.Exception) { }
            }
        }

        public void ActivateState(int index)
        {
            if (_isLoading)
            {
                throw new System.InvalidOperationException("In loading scene");
            }

            StopAllCoroutines();

            SetState(index);
        }


        
        void SetState(int index)
        {
            //Debug.Log("Activate state " + index + " - " + statesByOrder[index].Description());
            beforeChange.Invoke(statesByOrder[index].Description());

            var activateStateDescription = statesByOrder[index].Description();
            this.WaitForSeconds(delayBeforeChange, () =>
            {
                _isLoading = true;
                

                if (statesByOrder[index].Active == false)
                {
                    var asyncOp = SceneManager.LoadSceneAsync(statesByOrder[index].sceneId, LoadSceneMode.Additive);
                    asyncOp.completed += (a) => AsyncCompletedLoadScene(index);

                }
                else
                {
                    _isLoading = false;
                    afterChanged.Invoke(activateStateDescription);
                    afterChange.Invoke(index);

                }
            }
            );
        }

        private void AsyncCompletedLoadScene(int indexInScenes)
        {
            for (int i = 0; i < statesByOrder.Length; ++i)
            {
                var oldScene = statesByOrder[i];
                //Debug.Log("Check scene " + scene.Description() + " active " + scene.Active);
                if (i != indexInScenes && oldScene.Active)
                {
                    SceneManager.UnloadSceneAsync(oldScene.sceneId);
                    oldScene.Active = false;
                }
            }
            var scene = statesByOrder[indexInScenes];
            scene.Active = true;
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(scene.sceneId));
            _isLoading = false;

            if(scene.qualitySettings >= 0 ) QualitySettings.SetQualityLevel(scene.qualitySettings);

            afterChanged.Invoke(scene.Description());
            afterChange.Invoke(indexInScenes);

        }

    }
}