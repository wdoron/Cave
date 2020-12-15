using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dweiss.Msg
{
    

    public class OnMsgGeneric<T> : MonoBehaviour
    {
        public List<T> msgId;
        public float delay = -1;
        public bool runOnDisable = false;
        public enum EventType
        {
            None,Bool,String,Int,Float,Object,UnityObject,Component, GameObject
        }
        public EventType type;

        public SimpleEvent onEventVoid;
        public EventFloat onEventFloat;
        public EventInt onEventInt;
        public EventBool onEventBool;
        public EventString onEventString;
        public EventObject onEventObject;
        public EventUnityObject onEventUnityObject;
        public EventComponent onEventComponent;
        public EventGameObject onEventGameObject;

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
            if(on) foreach(var m in msgId) Register(m);
            else if(MsgSystem.S) foreach (var m in msgId) Unregister(m);
        }
        
        private void Register(T msg) {
            switch (type)
            {
                case EventType.None: MsgSystem.Get<T>().Register(msg, (System.Action)Action); break;
                case EventType.Bool: MsgSystem.Get<T>().Register(msg, (System.Action<bool>)Action); break;
                case EventType.Int: MsgSystem.Get<T>().Register(msg, (System.Action<int>)Action); ; break;
                case EventType.Float: MsgSystem.Get<T>().Register(msg, (System.Action<float>)Action); break;
                case EventType.String: MsgSystem.Get<T>().Register(msg, (System.Action<string>)Action); break;
                case EventType.Object: MsgSystem.Get<T>().Register<System.Object>(msg, (System.Action<System.Object>)Action); break;
                case EventType.UnityObject: MsgSystem.Get<T>().Register(msg, (System.Action<UnityEngine.Object>)Action); break;
                case EventType.Component: MsgSystem.Get<T>().Register(msg, (System.Action<Component>)Action); break;
                case EventType.GameObject: MsgSystem.Get<T>().Register(msg, (System.Action<GameObject>)Action); break;
                 
                default: throw new System.NotSupportedException(name + " Register Not supported " + type );
            }
        }
        private void Unregister(T msg)
        {
            switch (type)
            {
                case EventType.None: MsgSystem.Get<T>().Unregister(msg, (System.Action)Action); break;
                case EventType.Bool: MsgSystem.Get<T>().Unregister(msg, (System.Action<bool>)Action); break;
                case EventType.Int: MsgSystem.Get<T>().Unregister(msg, (System.Action<int>)Action); ; break;
                case EventType.Float: MsgSystem.Get<T>().Unregister(msg, (System.Action<float>)Action); break;
                case EventType.String: MsgSystem.Get<T>().Unregister(msg, (System.Action<string>)Action); break;
                case EventType.Object: MsgSystem.Get<T>().Unregister<System.Object>(msg, (System.Action<System.Object>)Action); break;
                case EventType.UnityObject: MsgSystem.Get<T>().Unregister(msg, (System.Action<UnityEngine.Object>)Action); break;
                case EventType.Component: MsgSystem.Get<T>().Unregister(msg, (System.Action<Component>)Action); break;
                case EventType.GameObject: MsgSystem.Get<T>().Unregister(msg, (System.Action<GameObject>)Action); break;
                    
                default: throw new System.NotSupportedException(name + " Unregister Not supported " + type);
            }
        }

        private void Action()
        {
            if (delay < 0)
            {
                onEventVoid.Invoke();
            }
            else
            {
                this.WaitForSeconds(delay, () => onEventVoid.Invoke());
            }
        }

        private void Invoke(object v) 
        {
            switch (type)
            {
                case EventType.Bool: onEventBool.Invoke((bool)v);break;
                case EventType.Int: onEventInt.Invoke((int)v); break;
                case EventType.Float: onEventFloat.Invoke((float)v); break;
                case EventType.String: onEventString.Invoke((string)v); break;
                //case EventType.None: onEventVoid.Invoke(); break;
                case EventType.Object: onEventObject.Invoke((System.Object)v); break;
                case EventType.UnityObject: onEventUnityObject.Invoke((UnityEngine.Object)v); break;
                case EventType.Component: onEventComponent.Invoke((Component)v); break;
                case EventType.GameObject: onEventGameObject.Invoke((GameObject)v); break;
                default: throw new System.NotSupportedException(name + " Invoke Not supported " + type + " for " + v);
            }
        }

        private void Action(bool v) { if (delay < 0) Invoke(v); else this.WaitForSeconds(delay, () => Invoke(v)); }
        private void Action(int v) { if (delay < 0) Invoke(v); else this.WaitForSeconds(delay, () => Invoke(v)); }
        private void Action(float v) { if (delay < 0) Invoke(v); else this.WaitForSeconds(delay, () => Invoke(v)); }
        private void Action(string v) { if (delay < 0) Invoke(v); else this.WaitForSeconds(delay, () => Invoke(v)); }
        private void Action(System.Object v) { if (delay < 0) Invoke(v); else this.WaitForSeconds(delay, () => Invoke(v)); }
        private void Action(UnityEngine.Object v) { if (delay < 0) Invoke(v); else this.WaitForSeconds(delay, () => Invoke(v)); }
        private void Action(Component v) { if (delay < 0) Invoke(v); else this.WaitForSeconds(delay, () => Invoke(v)); }
        private void Action(GameObject v) { if (delay < 0) Invoke(v); else this.WaitForSeconds(delay, () => Invoke(v)); }
    }
}