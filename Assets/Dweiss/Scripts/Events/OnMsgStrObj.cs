using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dweiss
{

    public class OnMsgStrObj : MonoBehaviour
    {
        [SerializeField] private string msgId;
        [SerializeField] private float delay = -1;
        [SerializeField] private bool runOnDisable = false;

        public Dweiss.EventObject onEvent;

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
            if(on) MsgSystemStrObj.S.Register(msgId, Action);
            else if(MsgSystemStrObj.S) MsgSystemStrObj.S.Unregister(msgId, Action);
        }
        

        private void Action(object obj)
        {
            if (delay < 0)
            {
                onEvent.Invoke(obj);
            }
            else
            {
                this.WaitForSeconds(delay, () => onEvent.Invoke(obj));
            }

        }
    }
}