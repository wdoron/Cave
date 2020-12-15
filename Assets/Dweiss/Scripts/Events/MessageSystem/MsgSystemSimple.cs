using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss.Deprecated
{


    public class MsgSystemSimple<T> : AutoSingleton<MsgSystemSimple<T>>
    {
        private Dictionary<T, Action> msgSystem = new Dictionary<T, Action>();


        public void Raise(T id)
        {

            Action action ;
            if (msgSystem.TryGetValue(id, out action))
            {
                action.Invoke();
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


        public void Register(T id, Action newAction)
        {

            Action action;
            if(msgSystem.TryGetValue(id, out action) == false)
            {
                msgSystem.Add(id, newAction);
            }else
            {
                action += newAction;
                msgSystem[id] = action;
            }
        }

       

        public void Unregister(T id, Action newAction)
        {

            Action action;
            if (msgSystem.TryGetValue(id, out action))
            {
                action -= newAction;
                msgSystem[id] = action;
            }
        }
    }

}