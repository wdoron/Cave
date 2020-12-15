using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss.Msg {
    [DefaultExecutionOrder(-1)]
    public class MsgCacheGeneric<T> : Singleton<MsgCacheGeneric<T>>
    {

        public T[] idsToRegister;

        private Dictionary<T, System.Object> _cache = new Dictionary<T, System.Object>();


        private System.Tuple<T, System.Action<object>>[] actions;
        new void Awake()
        {
            base.Awake();
            actions = new System.Tuple<T, System.Action<object>>[idsToRegister.Length];
            for (int i = 0; i < idsToRegister.Length; ++i)
            {
                var id = idsToRegister[i];
                actions[i] = System.Tuple.Create<T, System.Action<object>>(id, (obj) => _cache[id] = obj);
                MsgSystem.Get<T>().Register<System.Object>(id, actions[i].Item2);
            }
        }

        protected override void OnDestroy()
        {
            if (MsgSystem.S)
            {
                for (int i = 0; i < actions.Length; ++i)
                {
                    MsgSystem.Get<T>().Unregister<System.Object>(actions[i].Item1, actions[i].Item2);
                }
            }
        }

        public System.Object Get(T id)
        {
            object outVal;
            if(_cache.TryGetValue(id, out outVal))
            {
                return outVal;
            }
            return null;
        }

        public void Register(T id, System.Action<object> action)
        {
            MsgSystem.Get<T>().Register<object>(id, action);
            var val = Get(id);
            if(val != null) action(val);
        }
        public void Unregister(T id, System.Action<object> action)
        {
            if(MsgSystem.S)
                MsgSystem.Get<T>().Unregister<object>(id, action);
        }
    }
}