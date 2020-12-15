using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    [CreateAssetMenu( fileName = "IntGameEvent", menuName = "Events/IntGameEvent", order = 1)]
    public class IntGameEvent : ScriptableObject
    {
        private List<IntGameEventListener> listeners = new List<IntGameEventListener>();
        public UnityEngine.Events.UnityEvent onListenersEmpty;

        public void Raise(int value)
        {
            //TOPROFILE (negates read-ahead cache concpets ) but solve problme of remove during iteration
            for (var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(value);
            }
        }

        public void Register(IntGameEventListener listener) {
            listeners.Add(listener);
        }
        public void Unregister(IntGameEventListener listener) {
            listeners.Remove(listener);
            if (listeners.Count == 0) onListenersEmpty.Invoke();
            
        }
    }
}