using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dweiss
{
    [System.Serializable] //Depricated
    public class SimpleEvent : EventEmpty { }


    [System.Serializable]
    public class EventEmpty : UnityEvent
    {
        public System.Action action;
        private int _listenerCount;

        public int ListenerCount { get { return _listenerCount; } }

        public EventEmpty()
        {
            _listenerCount = 0;
        }

        new public void Invoke()
        {
            base.Invoke();
            if (action != null) action();
        }

        new public void AddListener(UnityAction call)
        {
            base.AddListener(call);
            _listenerCount++;
        }

        new public void RemoveListener(UnityAction call)
        {
            base.RemoveListener(call);
            _listenerCount--;
        }

        new public void RemoveAllListeners()
        {
            base.RemoveAllListeners();
            action = null;
            _listenerCount = 0;
        }

        public void AddAction(System.Action call)
        {
            action += call;
            _listenerCount++;
        }

        public void RemoveAction(System.Action call)
        {
            action -= call;
            _listenerCount--;
        }
    }

    [System.Serializable]
    public class UnityEventWithCount<T> : UnityEvent<T>
    {
        public System.Action<T> action;
        public System.Action simpleAction;
        private int _listenerCount;

        public int ListenerCount { get { return _listenerCount; } }

        public UnityEventWithCount()
        {
            _listenerCount = 0;
        }

        new public void Invoke(T data)
        {
            base.Invoke(data);
            if (action != null) action(data);
            if (simpleAction != null) simpleAction();
        }

        new public void AddListener(UnityAction<T> call)
        {
            base.AddListener(call);
            _listenerCount++;
        }

        new public void RemoveListener(UnityAction<T> call)
        {
            base.RemoveListener(call);
            _listenerCount--;
        }

        new public void RemoveAllListeners()
        {
            base.RemoveAllListeners();
            action = null;
            simpleAction = null;
            _listenerCount = 0;
        }
        public void AddAction(System.Action call)
        {
            simpleAction += call;
            _listenerCount++;
        }

        public void RemoveAction(System.Action call)
        {
            simpleAction -= call;
            _listenerCount--;
        }

        public void AddAction(System.Action<T> call)
        {
            action += call;
            _listenerCount++;
        }

        public void RemoveAction(System.Action<T> call)
        {
            action -= call;
            _listenerCount--;
        }
    }

    [System.Serializable]
    public class UnityEventWithCount<T,E> : UnityEvent<T,E>
    {
        public System.Action<T, E> action;
        private int _listenerCount;

        public int ListenerCount { get { return _listenerCount; } }

        public UnityEventWithCount()
        {
            _listenerCount = 0;
        }

        new public void AddListener(UnityAction<T, E> call)
        {
            base.AddListener(call);
            _listenerCount++;
        }

        new public void RemoveListener(UnityAction<T, E> call)
        {
            base.RemoveListener(call);
            _listenerCount--;
        }

        new public void RemoveAllListeners()
        {
            base.RemoveAllListeners();
            action = null;
            _listenerCount = 0;
        }

        public void AddAction(System.Action<T, E> call)
        {
            action += call;
            _listenerCount++;
        }

        public void RemoveAction(System.Action<T, E> call)
        {
            action -= call;
            _listenerCount--;
        }
    }

    [System.Serializable]
    public class EventString : UnityEventWithCount<string> { }

    [System.Serializable]
    public class EventBool : UnityEventWithCount<bool> { }

    [System.Serializable]
    public class EventFloat : UnityEventWithCount<float> { }

    [System.Serializable]
    public class EventInt : UnityEventWithCount<int> { }

    [System.Serializable]
    public class EventVector3 : UnityEventWithCount<Vector3> { }


    [System.Serializable]
    public class EventVector3List : UnityEventWithCount<List<Vector3>> { }

    [System.Serializable]
    public class EventVector2List : UnityEngine.Events.UnityEvent<List<Vector2>> { };

    [System.Serializable]
    public class EventVector2Array : UnityEngine.Events.UnityEvent<Vector2[]> { };



    [System.Serializable]
    public class EventScriptableObject : UnityEventWithCount<ScriptableObject> { }

    [System.Serializable]
    public class EventMonoBehaviour : UnityEventWithCount<MonoBehaviour> { }

    [System.Serializable]
    public class EventObject : UnityEventWithCount<System.Object> { }

    [System.Serializable]
    public class EventUnityObject : UnityEventWithCount<UnityEngine.Object> { }


    [System.Serializable]
    public class EventGameObject : UnityEventWithCount<GameObject> { }

    [System.Serializable]
    public class EventCollider : UnityEventWithCount<Collider> { }

    [System.Serializable]
    public class EventMonoSenderObject : UnityEventWithCount<MonoBehaviour, System.Object> { }

    [System.Serializable]
    public class EventComponent : UnityEventWithCount<Component> { }


}