using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss.Deprecated
{

    public class MessageEventSystem<E,T> : AutoSingleton<MessageEventSystem<E,T>>
    {
        private Dictionary<E, Action<T>> msgSystem = new Dictionary<E, Action<T>>();

        public void Raise(E id, T val)
        {

            Action<T> action;
            if (msgSystem.TryGetValue(id, out action))
            {
                action.Invoke(val);
            }
            else
            {
                Debug.LogWarning("No one register for " + id);
            }
        }

        void OnDisable()
        {
            msgSystem.Clear();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            msgSystem.Clear();
        }


        public void Register(E id, Action<T> newAction)
        {

            Action<T> action;
            if (msgSystem.TryGetValue(id, out action) == false)
            {
                msgSystem.Add(id, newAction);
            }
            else
            {
                action += newAction;
                msgSystem[id] = action;
            }
        }



        public void Unregister(E id, Action<T> newAction)
        {

            Action<T> action;
            if (msgSystem.TryGetValue(id, out action))
            {
                action -= newAction;
                msgSystem[id] = action;
            }
        }
    }

}