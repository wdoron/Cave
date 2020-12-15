using UnityEngine;
using UnityEngine.Events;

namespace Common
{
    public class FloatGameEventListener : MonoBehaviour
    {
        public FloatGameEvent gameEvent;
        public Common.EventFloat response;

        public void OnEventRaised(float value)
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