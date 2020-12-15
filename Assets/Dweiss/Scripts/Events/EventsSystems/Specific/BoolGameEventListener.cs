using UnityEngine;
using UnityEngine.Events;

namespace Common
{
    public class BoolGameEventListener : MonoBehaviour
    {
        public BoolGameEvent gameEvent;
        public Common.EventBool response;

        public void OnEventRaised(bool value)
        {
            response.Invoke(value);
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