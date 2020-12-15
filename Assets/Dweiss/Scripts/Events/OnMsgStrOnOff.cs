using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dweiss
{

    public class OnMsgStrOnOff : MonoBehaviour
    {
        [SerializeField] private string msgOn, msgOff;
        [SerializeField] private bool runOnDisable;

        public Dweiss.EventBool onEvent;

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
            if(on) MsgSystemStr.S.Register(msgOn, ActionOn);
            else if(MsgSystemStr.S) MsgSystemStr.S.Unregister(msgOn, ActionOn);

            if (on) MsgSystemStr.S.Register(msgOff, ActionOff);
            else if (MsgSystemStr.S) MsgSystemStr.S.Unregister(msgOff, ActionOff);
        }
        

        private void ActionOn()
        {
            onEvent.Invoke(true);
        }
        private void ActionOff()
        {
            onEvent.Invoke(false);
        }
    }
}
