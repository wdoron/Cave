using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss.Msg
{
    public class RaiseMsgGeneric<T> : MonoBehaviour
    {

//#if UNITY_EDITOR
        [SerializeField] private bool debug;
//#endif

        public T defaultId;
        public float delay = -1;

        private void Awake()
        {
#if UNITY_EDITOR == false
            debug = false;
#endif
        }


        public void Raise()
        {
#if UNITY_EDITOR
            if (debug) Debug.LogFormat("{0}-{1}-{2}-({3})", name, "Raise", defaultId, "");
#endif
            RaiseMsg(defaultId);
        }

        public void RaiseOtherMessage(T id)
        {
#if UNITY_EDITOR
            if (debug) Debug.LogFormat("{0}-{1}-{2}-({3})", name, "RaiseOtherMessage", id, "");
#endif
            RaiseMsg(id);
        }

        public void Raise(float v) { RaiseMsg(defaultId, v); }
        public void RaiseOtherMessage(T msg, float v) { RaiseMsg(msg, v); }

        public void Raise(int v) { RaiseMsg(defaultId, v); }
        public void RaiseOtherMessage(T msg, int v) { RaiseMsg(msg, v); }

        public void Raise(string v)
        {
#if UNITY_EDITOR
            if (debug) Debug.LogFormat("{0}-{1}-{2}-({3})", name, "Raise", defaultId, v);
#endif
            RaiseMsg(defaultId, v);
        }

        public void RaiseOtherMessage(T msg, string v) { RaiseMsg(msg, v); }

        public void Raise(bool v) { RaiseMsg(defaultId, v); }
        public void RaiseOtherMessage(T msg, bool v) { RaiseMsg(msg, v); }

        public void Raise(System.Object obj) { RaiseMsg(defaultId, obj); }
        public void RaiseOtherMessage(T msg, System.Object obj) { RaiseMsg(msg, obj); }

        public void Raise(UnityEngine.Object v) { RaiseMsg(defaultId, v); }
        public void RaiseOtherMessage(T msg, UnityEngine.Object v) { RaiseMsg(msg, v); }

        public void Raise(Component v) { RaiseMsg(defaultId, v); }
        public void RaiseOtherMessage(T msg, Component v) { RaiseMsg(msg, v); }

        public void Raise(GameObject v) { RaiseMsg(defaultId, v); }
        public void RaiseOtherMessage(T msg, GameObject v) { RaiseMsg(msg, v); }

        protected void RaiseMsg<E>(T msg, E v)
        {
            if (delay < 0) MsgSystem.Get<T>().Raise<E>(msg, v);
            else this.WaitForSeconds(delay, () => MsgSystem.Get<T>().Raise<E>(msg, v));
        }

        protected void RaiseMsg(T msg, bool v)
        {
            if (delay < 0) MsgSystem.Get<T>().Raise(msg, v);
            else this.WaitForSeconds(delay, () => MsgSystem.Get<T>().Raise(msg, v));
        }
        protected void RaiseMsg(T msg, float v)
        {
            if (delay < 0) MsgSystem.Get<T>().Raise(msg, v);
            else this.WaitForSeconds(delay, () => MsgSystem.Get<T>().Raise(msg, v));
        }
        protected void RaiseMsg(T msg, int v)
        {
            if (delay < 0) MsgSystem.Get<T>().Raise(msg, v);
            else this.WaitForSeconds(delay, () => MsgSystem.Get<T>().Raise(msg, v));
        }
        protected void RaiseMsg(T msg, string v)
        {
            if (delay < 0) MsgSystem.Get<T>().Raise(msg, v);
            else this.WaitForSeconds(delay, () => MsgSystem.Get<T>().Raise(msg, v));
        }
        protected void RaiseMsg(T msg, Component v)
        {
            if (delay < 0) MsgSystem.Get<T>().Raise(msg, v);
            else this.WaitForSeconds(delay, () => MsgSystem.Get<T>().Raise(msg, v));
        }
        protected void RaiseMsg(T msg, GameObject v)
        {
            if (delay < 0) MsgSystem.Get<T>().Raise(msg, v);
            else this.WaitForSeconds(delay, () => MsgSystem.Get<T>().Raise(msg, v));
        }
        protected void RaiseMsg(T msg, UnityEngine.Object v)
        {
            if (delay < 0) MsgSystem.Get<T>().Raise(msg, v);
            else this.WaitForSeconds(delay, () => MsgSystem.Get<T>().Raise(msg, v));
        }

        protected void RaiseMsg(T id)
        {
            if (delay < 0)
            {
                MsgSystem.Get<T>().Raise(id);
            }
            else
            {
                this.WaitForSeconds(delay, () => MsgSystem.Get<T>().Raise(id));
            }
        }

    }
}