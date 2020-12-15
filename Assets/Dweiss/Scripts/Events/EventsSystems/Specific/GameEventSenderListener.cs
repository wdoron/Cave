using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class GameEventSenderListener : MonoBehaviour
    {

        public GameEventSender gameEvent;
        public Dweiss.EventMonoSenderObject senderObjEvent;


        public void OnEventRaised()
        {
            OnEventRaised(null, null);
        }

        public void OnEventRaised(object obj)
        {
            //Debug.Log("OnEventRaised " + obj);
            OnEventRaised(null, obj);
        }

        public void OnEventRaised(MonoBehaviour sender, object obj)
        {
            senderObjEvent.Invoke(sender, obj);
        }

        private void OnEnable()
        {
            gameEvent.Register(this);
        }
        private void OnDisable()
        {
            gameEvent.Unregister(this);
        }
    }
}