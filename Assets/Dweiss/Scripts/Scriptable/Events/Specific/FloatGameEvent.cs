using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    [CreateAssetMenu( fileName = "FloatGameEvent", menuName = "Events/FloatGameEvent", order = 1)]
    public class FloatGameEvent : ScriptableObject
    {
        private List<FloatGameEventListener> listeners = new List<FloatGameEventListener>();
        public UnityEngine.Events.UnityEvent onListenersEmpty;

        public void Raise(float value)
        {
            //TOPROFILE (negates read-ahead cache concpets ) but solve problme of remove during iteration
            for (var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(value);
            }
        }

        public void Register(FloatGameEventListener listener) {
            listeners.Add(listener);
        }
        public void Unregister(FloatGameEventListener listener) {
            listeners.Remove(listener);
            if (listeners.Count == 0) onListenersEmpty.Invoke();
            
        }
    }
}