using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Common
{
    public abstract class GenericGameEvent<T> : ScriptableObject
    {
        public int fillValue = -1;
        private List<GenericGameEventListener<T>> listeners = new List<GenericGameEventListener<T>>();
        public UnityEvent onListenersEmpty, onListenersFull;

        public List<GenericGameEventListener<T>> Listeners
        {
            get { return listeners; }
        }

        public void Raise()
        {
            //TOPROFILE (negates read-ahead cache concpets ) but solve problme of remove during iteration
            for (var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised();
            }
        }

        public void Raise(T obj)
        {
            //TOPROFILE (negates read-ahead cache concpets ) but solve problme of remove during iteration
            for (var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(obj);
            }
        }

        public void Register(GenericGameEventListener<T> listener)
        {
            listeners.Add(listener);
            if(Listeners.Count == fillValue)
            {
                onListenersFull.Invoke();
            }
            //Debug.Log(name + " Registered " + listeners.Count);
        }

        public void Unregister(GenericGameEventListener<T> listener)
        {
            listeners.Remove(listener);
            if (listeners.Count == 0) onListenersEmpty.Invoke();
        }


    }
}