using UnityEngine;
using UnityEngine.Events;

namespace Common
{
    public class GameEventListener : MonoBehaviour
    {

        public GameEvent gameEvent;
        public UnityEvent response;


        public Dweiss.EventObject objEvent;

        public EventSenderObject senderObjEvent;

        [System.Serializable]
        public class EventSenderObject : Dweiss.UnityEventWithCount<Common.GameEventListener, System.Object> { }

        public void OnEventRaised()
        {
            response.Invoke();
        }

        public void OnEventRaised(object obj)
        {
            //Debug.Log("OnEventRaised " + obj);
            objEvent.Invoke(obj);
        }

        public void OnEventRaised(GameEventListener sender, object obj)
        {
           // if(objEvent.ListenerCount > 0)
          //Debug.Log(name + " OnEventRaised by " + sender);
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