using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    [CreateAssetMenu( fileName = "BoolGameEvent", menuName = "Events/BoolGameEvent", order = 1)]
    public class BoolGameEvent : ScriptableObject
    {
        private List<BoolGameEventListener> listeners = new List<BoolGameEventListener>();
        public UnityEngine.Events.UnityEvent onListenersEmpty;

        public void Raise(bool value)
        {
            //TOPROFILE (negates read-ahead cache concpets ) but solve problme of remove during iteration
            for (var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(value);
            }
        }

        public void Register(BoolGameEventListener listener) {
            listeners.Add(listener);
        }
        public void Unregister(BoolGameEventListener listener) {
            listeners.Remove(listener);
            if (listeners.Count == 0) onListenersEmpty.Invoke();
            
        }
    }
}