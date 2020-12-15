using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dweiss
{
    public class AttachOnStart : MonoBehaviour
    {
        public bool setParent = false;
        public string tagToAttachTo;
        public bool resetTransform = true;

        public bool destroyOnSceneChange;
        public bool persistantStick;

        public bool resetOnSceneLoad;
        private Transform other;

        private void Start()
        {
            ResetSettings();
            SceneManager.sceneLoaded += OnSceneLoaded;

        }

        private void ResetSettings()
        {
            other = GameObject.FindGameObjectWithTag(tagToAttachTo).transform;

            if(setParent)
                transform.SetParent(other.transform);

            if (resetTransform)
            {
                Stick();
            }

        }
        private void Stick()
        {
       //     Debug.Log(name + " reset pos");
            transform.position = other.transform.position;
            transform.rotation = other.transform.rotation;
        }
        private void OnSceneLoaded(Scene aScene, LoadSceneMode aMode)
        {
            if(destroyOnSceneChange)
                Destroy(gameObject);
            if (resetOnSceneLoad)
            {
                ResetSettings();
            }
        }

        private void Update()
        {
            if (persistantStick) Stick();
        }
    }
}