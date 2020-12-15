using UnityEngine;
using UnityEngine.Events;

namespace Common
{
    public abstract class GenericGameEventListener<T> : MonoBehaviour
    {
        [SerializeField]private T attached;
        public T Attached { get { return attached; } }

        public abstract GenericGameEvent<T> GameEvent { get; }
        public UnityEvent response;
        public System.Action<T> specificRespone;

        public void OnEventRaised()
        {
            response.Invoke();
        }

        public void OnEventRaised(T evn)
        {
            response.Invoke();
            specificRespone.Invoke(evn);
        }


        private void OnEnable()
        {
            GameEvent.Register(this);
        }
        private void OnDisable()
        {
            GameEvent.Unregister(this);
        }
    }
}