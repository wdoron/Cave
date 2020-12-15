using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class MessageOnEnableChange : MonoBehaviour
    {
        public enum InvokeAt
        {
            Enable,Disable,OnChange,None

        }

        public InvokeAt invokeAt;
        public string msgRegistrationStr = "MessageToMe";

        [Tooltip("To wait one frame use -1")]
        public float delayStart = 0;

        private void Reset()
        {
            msgRegistrationStr = this.name;
        }

        public void Raise()
        {
            if (delayStart == 0)
            {
                MsgSystemStr.S.Raise(msgRegistrationStr);
            }
            else
            {
                this.WaitForSeconds(Mathf.Max(delayStart, 0), () => MsgSystemStr.S.Raise(msgRegistrationStr));
            }
        }

        public void Raise(string str)
        {
            if (delayStart == 0)
            {
                MsgSystemStrStr.S.Raise(msgRegistrationStr, str);
            }
            else
            {
                this.WaitForSeconds(Mathf.Max(delayStart, 0), () => MsgSystemStrStr.S.Raise(msgRegistrationStr, str));
            }
        }


        public void Raise(float v)
        {
            if (delayStart == 0)
            {
                MsgSystemStrFloat.S.Raise(msgRegistrationStr, v);
            }
            else
            {
                this.WaitForSeconds(Mathf.Max(delayStart, 0), () => MsgSystemStrFloat.S.Raise(msgRegistrationStr, v));
            }
        }

        public void Raise(bool v)
        {
            if (delayStart == 0)
            {
                MsgSystemStrBool.S.Raise(msgRegistrationStr, v);
            }
            else
            {
                this.WaitForSeconds(Mathf.Max(delayStart, 0), () => MsgSystemStrBool.S.Raise(msgRegistrationStr, v));
            }
        }

        private void OnEnable()
        {
            if(invokeAt == InvokeAt.Enable || invokeAt == InvokeAt.OnChange) MsgSystemStr.S.Raise(msgRegistrationStr);
        }

        private void OnDisable()
        {
            if (invokeAt == InvokeAt.Disable || invokeAt == InvokeAt.OnChange) MsgSystemStr.S.Raise(msgRegistrationStr);
        }
    }
}