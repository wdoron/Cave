using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dweiss
{

    public class OnMsgStrFloat : MonoBehaviour
    {
        [SerializeField] private string msgId;
        [SerializeField] private float delay = -1;
        [SerializeField] private bool runOnDisable = false;
        [SerializeField] private bool debug = false;
        public Dweiss.EventFloat onEvent;

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
            if(on) Dweiss.Msg.MsgSystem.Register(msgId, (System.Action<float>)Action);
            else if(MsgSystemStrFloat.S) Dweiss.Msg.MsgSystem.Unregister(msgId, (System.Action<float>)Action);
        }
        

        private void Action(float f)
        {
            if(debug) Debug.Log("Event catch " + msgId + " v " + f);
            if (delay < 0)
            {
                onEvent.Invoke(f);
            }
            else
            {
                this.WaitForSeconds(delay, () => onEvent.Invoke(f));
            }
        }
    }
}