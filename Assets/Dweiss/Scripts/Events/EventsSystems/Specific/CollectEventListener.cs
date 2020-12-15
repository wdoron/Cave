
using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dweiss.Common
{
    public class CollectEventListener : MonoBehaviour
    {
        public CollectEvent collectEventSystem;

        public Dweiss.SimpleEvent onCollectAll;
        
        public CollectEvent.OnCollectEvent onCollectionChange;
        public Dweiss.SimpleEvent onResetCollection;
        public OnItemEvent onCollect, onRestore;

        [System.Serializable]
        public class OnItemEvent : UnityEvent<Collectable> { }

        public void OnCollectionChangedRaised(CollectEvent.CollectInfo collectInfo)
        {
            switch (collectInfo.reason)
            {
                case CollectEvent.CollectType.Collected:
                    onCollect.Invoke(collectInfo.obj);
                    onCollectionChange.Invoke(collectInfo);break;
                case CollectEvent.CollectType.Restore:
                    onCollect.Invoke(collectInfo.obj);
                    onCollectionChange.Invoke(collectInfo); break;
                case CollectEvent.CollectType.Reset:
                    onResetCollection.Invoke();
                    onCollectionChange.Invoke(collectInfo); break;
                default: throw new System.NotSupportedException("Collection type not esupported " + collectInfo.reason + ": " + collectInfo);
            }
            //Debug.Log("collected " + collectInfo);
            if(collectInfo.registeredCount != 0 &&  collectInfo.newList.Count == collectInfo.registeredCount)
            {
                onCollectAll.Invoke();
            }
        }



        private void OnEnable()
        {

            collectEventSystem.Register(this);
        }

        private void OnDisable()
        {
            collectEventSystem.Unregister(this);
        }

    }
}