using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Common
{
    public class GameEventCollectorListener : MonoBehaviour
    {
       // public GameEventCollect gameEvent;

        public EventSenderObject senderObjEvent;

        [System.Serializable]
        public class EventSenderObject : Dweiss.UnityEventWithCount<Common.GameEventCollectorListener, System.Object> { }

        public void OnEventRaised()
        {
            OnEventRaised(null, null);
        }

        public void OnEventRaised(object obj)
        {
            //Debug.Log("OnEventRaised " + obj);
            OnEventRaised(null, obj);
        }

        public void OnEventRaised(GameEventCollectorListener sender, object obj)
        {
            // if(objEvent.ListenerCount > 0)
            //Debug.Log(name + " OnEventRaised by " + sender);
            senderObjEvent.Invoke(sender, obj);
        }

        //private void OnEnable()
        //{
        //    gameEvent.Register(this);
        //}
        //private void OnDisable()
        //{
        //    gameEvent.Unregister(this);
        //}
    }
}