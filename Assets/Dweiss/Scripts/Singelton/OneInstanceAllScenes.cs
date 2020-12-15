using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Dweiss;
using UnityEngine.SceneManagement;

namespace Dweiss
{

    [Dweiss.Order(short.MinValue)]
    public class OneInstanceAllScenes : MonoBehaviour
    {

        void Awake()
        {
            var similar = FindObjectsOfType<OneInstanceAllScenes>().Where(a => a.gameObject.name == gameObject.name).ToArray();
            var others = similar.Where(a => a.gameObject != gameObject).ToArray();
            if (others.Length > 0)
            {
                //Debug.LogWarning(gameObject.name + " Other similar " + similar.ToCommaString() + " instanstances exists. Destroying due to others: " + others.ToCommaString());
                foreach (Transform t in transform) t.gameObject.SetActive(false);
                Destroy(gameObject);
            }
            else
            {
               // SceneManager.sceneLoaded += OnSceneLoaded;

                DontDestroyOnLoad(gameObject);
                foreach (Transform t in transform) t.gameObject.SetActive(true);
               // Debug.Log(name + " keep all scenes");
            }
        }
        //private void OnSceneLoaded(Scene aScene, LoadSceneMode aMode)
        //{
        //    Awake();
        //}
        private void OnDestroy()
        {
           // Debug.LogWarning("Destroyed " + gameObject);
        }
    }
}