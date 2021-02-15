using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class RaiseMessage : MonoBehaviour
    {
        public string defaultId = "";
        public float delay = -1;
        private void Reset() {
            defaultId = name;
        }
        [ContextMenu("Raise")]
        public void Raise()
        {
            RaiseMsg(defaultId);
            
        }
        [ContextMenu("RaiseNow")]
        public void RaiseNow()
        {
            MsgSystemStr.S.Raise(defaultId);
        }
        void RaiseMsg(string msg)
        {
            if (delay < 0)
            {
                Dweiss.Msg.MsgSystem.Raise(msg);
            } else
            {
                this.WaitForSeconds(delay, () => MsgSystemStr.S.Raise(msg));
            }
        }
        [ContextMenu("TestRaiseNow float ")]
        void TestRaiseNow() {
            RaiseMsgWithFloat(.5f);
        }
        public void RaiseMsgWithFloat(float v) {
            if (delay < 0) {
                Dweiss.Msg.MsgSystem.Raise(defaultId, v);
            } else {
                this.WaitForSeconds(delay, () => Dweiss.Msg.MsgSystem.Raise(defaultId, v));
            }
        }

        public void RaiseOtherMessage(string msg)
        {
            RaiseMsg(msg);
        }
        public void RaiseOtherMessageWithNoDelay(string msg)
        {
            MsgSystemStr.S.Raise(msg);
        }
    }
}