using UnityEngine.Events;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss.Common
{
    [CreateAssetMenu(fileName = "CollectEvent", menuName = "Events/CollectEvent", order = 1)]
    public class CollectEvent : ScriptableObject
    {
        private List<CollectEventListener> listeners = new List<CollectEventListener>();
        public OnItemEvent onItemRegister, onItemUnregister;

        [System.Serializable]
        public class OnItemEvent : UnityEvent<CollectEventListener> { }

        //public void Raise()
        //{
        //    //TOPROFILE (negates read-ahead cache concpets ) but solve problme of remove during iteration
        //    for (var i = listeners.Count - 1; i >= 0; i--)
        //    {
        //        listeners[i].OnEventRaised();
        //    }
        //}

        //public void Raise(object obj)
        //{
        //    //TOPROFILE (negates read-ahead cache concpets ) but solve problme of remove during iteration
        //    for (var i = listeners.Count - 1; i >= 0; i--)
        //    {
        //        listeners[i].OnEventRaised(obj);
        //    }
        //}

        //public void Raise(CollectEventListener sender, object obj)
        //{
        //    //TOPROFILE (negates read-ahead cache concpets ) but solve problme of remove during iteration
        //    for (var i = listeners.Count - 1; i >= 0; i--)
        //    {
        //        listeners[i].OnEventRaised(sender, obj);
        //    }
        //}
        private int registeredCount = -1;

        public void Register(CollectEventListener listener)
        {
            listeners.Add(listener);
            onItemRegister.Invoke(listener);
        }

        public void Unregister(CollectEventListener listener)
        {
            listeners.Remove(listener);
            onItemUnregister.Invoke(listener);
        }

        private List<Collectable> collected = new List<Collectable>();
        private List<Collectable> registeredCollectable = new List<Collectable>();
        public void CollectRegister(Collectable collectable)
        {
            registeredCollectable.Add(collectable);
            registeredCount = listeners.Count;

        }

        public void CollectUnregister(Collectable collectable)
        {
            registeredCollectable.Remove(collectable);
        }

        public enum CollectType
        {
            Collected, Restore, Reset
        }

        public struct CollectInfo
        {
            public CollectType reason;
            public Collectable obj;
            public List<Collectable> newList;
            public int registeredCount;

            public CollectInfo(CollectType reason, Collectable obj, List<Collectable> newList, 
                int registeredCount)
            {
                this.registeredCount = registeredCount;
                this.reason = reason;
                this.obj = obj;
                this.newList = newList;
            }

            public override string ToString()
            {
                return string.Format("{0}/{1} ({2} - {3})", newList.Count, registeredCount, reason, obj);
            }
        }

        [System.Serializable]
        public class OnCollectEvent : UnityEvent<CollectInfo> { }

        private void RaiseCollectState(CollectType reason, Collectable listener)
        {
            for (var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnCollectionChangedRaised(
                    new CollectInfo(reason, listener, 
                    collected, registeredCount)
                    );
            }
        }
      

        public void Collect(Collectable listener)
        {
            collected.Add(listener);
            RaiseCollectState(CollectType.Collected, listener);


            //onCollectChange.Invoke(new CollectInfo(CollectType.Collected, listener, collected));
            //onCollectionChange.Invoke(collected);
            //onCollect.Invoke(listener);
        }

        public void Restore(Collectable listener)
        {
            collected.Remove(listener);
            RaiseCollectState(CollectType.Restore, listener);
            //onCollectionChange.Invoke(collected);
            //onRestore.Invoke(listener);
        }
        public bool saveCountOnReset = true;
        public void ResetCollection()
        {
            collected.Clear();
            if (saveCountOnReset)
            {
                registeredCount = registeredCollectable.Count;
            }
            RaiseCollectState(CollectType.Reset, null);
            //onCollectionChange.Invoke(collected);
            //onResetCollection.Invoke();
        }
    }
}