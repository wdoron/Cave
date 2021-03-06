﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    [Order(-32768)]
    public class MsgSystemStr : AutoSingleton<MsgSystemStr>
    {
        public void Raise(string id)
        {
            Dweiss.Msg.MsgSystem.Get<string>().Raise(id);
        }
        public void Register(string id, Action newAction)
        {
            Dweiss.Msg.MsgSystem.Get<string>().Register(id, newAction);
        }

        public void Unregister(string id, Action newAction)
        {
            Dweiss.Msg.MsgSystem.Get<string>().Unregister(id, newAction);
        }
        public static new MsgSystemStr S
        {
             get { return Dweiss.Msg.MsgSystem.S != null ? Inst : null; } 
        }

        //private Dictionary<string, Action> msgSystem = new Dictionary<string, Action>();

        //public void Raise(string id)
        //{

        //    Action action;
        //    if (msgSystem.TryGetValue(id, out action))
        //    {
        //        action.Invoke();
        //    }
        //    else
        //    {
        //        Debug.LogWarning("No one register for " + id);
        //    }
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
        //public void Register(string id, Action newAction)
        //{

        //    Action action;
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
        //public void Unregister(string id, Action newAction)
        //{

        //    Action action;
        //    if (msgSystem.TryGetValue(id, out action))
        //    {
        //        action -= newAction;
        //        msgSystem[id] = action;
        //    }
        //}
    }


}