using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dweiss
{

    public class OnMsgStrStr : MonoBehaviour
    {
        [SerializeField] private string msgId;
        [Tooltip("Will raise only if message = msgInfo (use Empty string to raise anyway)")]
        [SerializeField] private string msgInfo;
        [SerializeField] private float delay = -1;
        [SerializeField] private bool runOnDisable = false;

        public Dweiss.EventString onEvent;

        private void Start()
        {
            if (runOnDisable) Register(true);
        }
        private void OnDestroy()
        {
            if (runOnDisable) Register(false);
        }

        private void OnEnable()
        {
            if (runOnDisable == false) Register(true);
        }
        private void OnDisable()
        {
            if (runOnDisable == false) Register(false);
        }

        private void Register(bool on)
        {
            if(on) MsgSystemStrStr.S.Register(msgId, Action);
            else if(MsgSystemStrStr.S) MsgSystemStrStr.S.Unregister(msgId, Action);
        }
        

        private void Action(string str)
        {
            

            if (string.IsNullOrEmpty(msgInfo) || str == msgInfo)
            {
                if (delay < 0)
                {
                    onEvent.Invoke(str);
                }
                else
                {
                    this.WaitForSeconds(delay, () => onEvent.Invoke(str));
                }
            }
        }
    }
}