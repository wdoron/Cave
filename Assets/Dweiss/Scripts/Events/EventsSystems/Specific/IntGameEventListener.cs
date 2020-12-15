using UnityEngine;
using UnityEngine.Events;

namespace Common
{
    public class IntGameEventListener : MonoBehaviour
    {
        public IntGameEvent gameEvent;
        public Common.EventInt response;

        public void OnEventRaised(int value)
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