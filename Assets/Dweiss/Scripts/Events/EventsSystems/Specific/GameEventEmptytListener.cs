using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class GameEventEmptytListener : MonoBehaviour
    {
        public Common.GameEvent onEmpty;
        public UnityEngine.Events.UnityEvent onCollectAll;

        private bool quit = false;


        private void OnEnable()
        {
            onEmpty.onListenersEmpty.AddListener(AllListenersRemoved);
        }

        private void OnDisable()
        {
            onEmpty.onListenersEmpty.RemoveListener(AllListenersRemoved);
        }

        void OnApplicationQuit()
        {
            quit = true;
        }


        void AllListenersRemoved()
        {
            if(quit == false)
            {
                onCollectAll.Invoke();
            }
        }
    }
}