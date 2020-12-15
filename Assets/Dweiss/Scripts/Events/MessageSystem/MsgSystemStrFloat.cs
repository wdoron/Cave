﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    [Order(-32768)]
    public class MsgSystemStrFloat : AutoSingleton<MsgSystemStrFloat>
    {
        public void Raise(string id, float data)
        {
            Dweiss.Msg.MsgSystem.Get<string>().Raise(id, data);
        }
        public void Register(string id, Action<float> newAction)
        {
            Dweiss.Msg.MsgSystem.Get<string>().Register(id, newAction);
        }
        public void Unregister(string id, Action<float> newAction)
        {
            Dweiss.Msg.MsgSystem.Get<string>().Unregister(id, newAction);
        }

        public static new MsgSystemStrFloat S
        {
            get { return Dweiss.Msg.MsgSystem.S != null ? Inst : null; }
        }
        //private Dictionary<string, Action<float>> msgSystem = new Dictionary<string, Action<float>>();

        //public void Raise(string id, float data)
        //{

        //    Action<float> action;
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
        //public void Register(string id, Action<float> newAction)
        //{

        //    Action<float> action;
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
        //public void Unregister(string id, Action<float> newAction)
        //{

        //    Action<float> action;
        //    if (msgSystem.TryGetValue(id, out action))
        //    {
        //        action -= newAction;
        //        msgSystem[id] = action;
        //    }
        //}
    }



}