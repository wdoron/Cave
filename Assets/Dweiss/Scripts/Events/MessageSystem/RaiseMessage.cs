using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class RaiseMessage : MonoBehaviour
    {
        public string defaultId = "";
        public float delay = -1;
        public void Raise()
        {
            RaiseMsg(defaultId);
        }
        public void RaiseNow()
        {
            MsgSystemStr.S.Raise(defaultId);
        }
        void RaiseMsg(string msg)
        {
            if (delay < 0)
            {
                MsgSystemStr.S.Raise(msg);
            } else
            {
                this.WaitForSeconds(delay, () => MsgSystemStr.S.Raise(msg));
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