using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    [Order(-32768)]
    public class MsgSystemStInt : AutoSingleton<MsgSystemStInt>
    {
        public void Raise(string id, int data)
        {
            Dweiss.Msg.MsgSystem.Get<string>().Raise(id, data);
        }
        public void Register(string id, Action<int> newAction)
        {
            Dweiss.Msg.MsgSystem.Get<string>().Register(id, newAction);
        }
        public void Unregister(string id, Action<int> newAction)
        {
            Dweiss.Msg.MsgSystem.Get<string>().Unregister(id, newAction);
        }
        public static new MsgSystemStInt S
        {
            get { return Dweiss.Msg.MsgSystem.S != null ? Inst : null; }
        }
        //private Dictionary<string, Action<int>> msgSystem = new Dictionary<string, Action<int>>();

        //public void Raise(string id, int data)
        //{

        //    Action<int> action;
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
        //public void Register(string id, Action<int> newAction)
        //{

        //    Action<int> action;
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
        //public void Unregister(string id, Action<int> newAction)
        //{

        //    Action<int> action;
        //    if (msgSystem.TryGetValue(id, out action))
        //    {
        //        action -= newAction;
        //        msgSystem[id] = action;
        //    }
        //}
    }



}