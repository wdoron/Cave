using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    [CreateAssetMenu(fileName = "GameEventSender", menuName = "Events/GameEventSender", order = 2)]
    public class GameEventSender : ScriptableObject
    {
        private List<GameEventSenderListener> listeners = new List<GameEventSenderListener>();
        public UnityEngine.Events.UnityEvent onListenersEmpty;
        public OnItemEvent onItemRegister, onItemUnregister;

        [System.Serializable]
        public class OnItemEvent : UnityEngine.Events.UnityEvent<GameEventSenderListener> { }

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

        public void Raise(GameEventSenderListener sender, object obj)
        {
            //TOPROFILE (negates read-ahead cache concpets ) but solve problme of remove during iteration
            for (var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(sender, obj);
            }
        }

        public void Register(GameEventSenderListener listener)
        {
            listeners.Add(listener);
            onItemRegister.Invoke(listener);
        }

        public void Unregister(GameEventSenderListener listener)
        {
            listeners.Remove(listener);
            if (listeners.Count == 0) onListenersEmpty.Invoke();

            onItemUnregister.Invoke(listener);
        }

    }
}
