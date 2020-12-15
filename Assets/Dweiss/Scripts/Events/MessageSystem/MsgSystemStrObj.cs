using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    [Order(-32768)]
    public class MsgSystemStrObj : AutoSingleton<MsgSystemStrObj>
    {
        public void Raise(string id, object data)
        {
            Dweiss.Msg.MsgSystem.Get<string>().Raise<System.Object>(id, data);
        }
        public void Register(string id, Action<object> newAction)
        {
            Dweiss.Msg.MsgSystem.Get<string>().Register<System.Object>(id, newAction);
        }
        public void Unregister(string id, Action<object> newAction)
        {
            Dweiss.Msg.MsgSystem.Get<string>().Unregister<System.Object>(id, newAction);
        }

        public static new MsgSystemStrObj S
        {
            get { return Dweiss.Msg.MsgSystem.S != null ? Inst : null; }
        }
        //private Dictionary<string, Action<object>> msgSystem = new Dictionary<string, Action<object>>();

        //public void Raise(string id, object data)
        //{

        //    Action<object> action;
        //    if (msgSystem.TryGetValue(id, out action))
        //    {
        //        action.Invoke(data);
        //    }
        //    else
        //    {
        //        Debug.LogWarning("No one register for " + id);
        //    }

        //    if(MsgSystemStr.S) MsgSystemStr.S.Raise(id);
        //}

        //void OnDisable()
        //{
        //    msgSystem.Clear();
        //}

        //protected override void OnDestroy()
        //{
        //    base.OnDestroy();
        //    msgSystem.Clear();
        //}
        //public void Register(string id, Action<object> newAction)
        //{

        //    Action<object> action;
        //    if (msgSystem.TryGetValue(id, out action) == false)
        //    {
        //        msgSystem.Add(id, newAction);
        //    }
        //    else
        //    {
        //        action += newAction;
        //        msgSystem[id] = action;
        //    }


        //}
        //public void Unregister(string id, Action<object> newAction)
        //{

        //    Action<object> action;
        //    if (msgSystem.TryGetValue(id, out action))
        //    {
        //        action -= newAction;
        //        msgSystem[id] = action;
        //    }
        //}
    }



}