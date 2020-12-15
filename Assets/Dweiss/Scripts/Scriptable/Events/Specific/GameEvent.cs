using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    [CreateAssetMenu( fileName = "SImpleGameEvent", menuName = "Events/SImpleGameEvent", order = 1)]
    public class GameEvent : ScriptableObject
    {
        private List<GameEventListener> listeners = new List<GameEventListener>();
        public UnityEngine.Events.UnityEvent onListenersEmpty;
        public OnItemEvent onItemRegister, onItemUnregister;

        [System.Serializable]
        public class OnItemEvent : UnityEngine.Events.UnityEvent<GameEventListener> { }

        public void Raise()
        {
            //TOPROFILE (negates read-ahead cache concpets ) but solve problme of remove during iteration
            for (var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised();
            }
        }

        public void Raise(object obj)
        {
            //TOPROFILE (negates read-ahead cache concpets ) but solve problme of remove during iteration
            for (var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(obj);
            }
        }

        public void Raise(GameEventListener sender, object obj)
        {
            //TOPROFILE (negates read-ahead cache concpets ) but solve problme of remove during iteration
            for (var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(sender, obj);
            }
        }

        public void Register(GameEventListener listener) {
            listeners.Add(listener);
            onItemRegister.Invoke(listener);
        }

        public void Unregister(GameEventListener listener) {
            listeners.Remove(listener);
            if (listeners.Count == 0) onListenersEmpty.Invoke();

            onItemUnregister.Invoke(listener);
        }
        

    }
}