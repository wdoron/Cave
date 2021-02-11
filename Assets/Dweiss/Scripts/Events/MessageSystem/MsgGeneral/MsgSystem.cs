using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Dweiss.Msg
{
    [DefaultExecutionOrder(-1000)]
    public class MsgSystem : MonoBehaviour
    {
        #region Static Singelton
        public static MsgSystemGeneric<T> Get<T>()
        {
            var key = typeof(T);

            object outV = null;

            if (S.msgSystemDictionary.TryGetValue(key, out outV) == false)
            {
                if (key == typeof(string)) outV = MsgStr;
                else if (key == typeof(int)) outV = MsgInt;
                else outV = new MsgSystemGeneric<T>();

                S.msgSystemDictionary[key] = outV;
            }
            return outV as MsgSystemGeneric<T>;
        }

        public static MsgSystemGeneric<string> MsgStr
        {
            get { return S.msgSystemStr; }
        }



        #region Fast call

        public static void Raise<T>(params T[] ids)
        { var inst = Get<T>(); for (int i = 0; i < ids.Length; i++) inst.Raise(ids[i]); }

        public static void Register<T>(T id1, Action action, params T[] ids)
        { var inst = Get<T>(); for (int i = 0; i < ids.Length; i++) inst.Register(ids[i], action); 
            inst.Register(id1, action);
        }


    public static void Unregister<T>(T id1, Action action, params T[] ids)
        { var inst = Get<T>(); for (int i = 0; i < ids.Length; i++) inst.Unregister(ids[i], action);
            inst.Unregister(id1, action);
        }

        public static void TryUnregister<T>(T id1, Action action, params T[] ids)
        { if (S) {
                var inst = Get<T>(); for (int i = 0; i < ids.Length; i++) inst.Unregister(ids[i], action);
                inst.Unregister(id1, action);
            } }

        //public static void Raise<T>(T id) {  }
        //public static void Register<T>(T id, Action action) { Get<T>().Register(id, action); }
        //public static void Unregister<T>(T id, Action action) { Get<T>().Unregister(id, action); }
        //public static void TryUnregister<T>(T id, Action action) { if(S) Get<T>().Unregister(id, action); }

        public static void Raise<T,V>(T id, System.Object v) { Get<T>().Raise<V>(id, v); }
        public static void Register<T, V>(T id, Action<System.Object> action) { Get<T>().Register<V>(id, action); }
        public static void Unregister<T, V>(T id, Action<System.Object> action) { Get<T>().Unregister<V>(id, action); }
        public static void TryUnregister<T, V>(T id, Action<System.Object> action) { if (S) Get<T>().Unregister<V>(id, action); }

        public static void Raise<T>(T id, float v) { Get<T>().Raise(id,v);
            Debug.Log("Raised " + id + " :f: " + v); }
        public static void Register<T>(T id, Action<float> action) { Get<T>().Register(id, action); }
        public static void Unregister<T>(T id, Action<float> action) { Get<T>().Unregister(id, action); }
        public static void TryUnregister<T>(T id, Action<float> action) { if (S) Get<T>().Unregister(id, action); }

        public static void Raise<T>(T id, int v) { Get<T>().Raise(id); }
        public static void Register<T>(T id, Action<int> action) { Get<T>().Register(id, action); }
        public static void Unregister<T>(T id, Action<int> action) { Get<T>().Unregister(id, action); }
        public static void TryUnregister<T>(T id, Action<int> action) { if (S) Get<T>().Unregister(id, action); }

        public static void Raise<T>(T id, string v) { Get<T>().Raise(id);  }
        public static void Register<T>(T id, Action<string> action) { Get<T>().Register(id, action); }
        public static void Unregister<T>(T id, Action<string> action) { Get<T>().Unregister(id, action); }
        public static void TryUnregister<T>(T id, Action<string> action) { if (S) Get<T>().Unregister(id, action); }

        public static void Raise<T>(T id, bool v) { Get<T>().Raise(id);  }
        public static void Register<T>(T id, Action<bool> action) { Get<T>().Register(id, action); }
        public static void Unregister<T>(T id, Action<bool> action) { Get<T>().Unregister(id, action); }
        public static void TryUnregister<T>(T id, Action<bool> action) { if (S) Get<T>().Unregister(id, action); }

        public static void Raise<T>(T id, UnityEngine.Object v) { Get<T>().Raise(id);  }
        public static void Register<T>(T id, Action<UnityEngine.Object> action) { Get<T>().Register(id, action); }
        public static void Unregister<T>(T id, Action<UnityEngine.Object> action) { Get<T>().Unregister(id, action); }
        public static void TryUnregister<T>(T id, Action<UnityEngine.Object> action) { if (S) Get<T>().Unregister(id, action); }

        public static void Raise<T>(T id, Component v) { Get<T>().Raise(id);  }
        public static void Register<T>(T id, Action<Component> action) { Get<T>().Register(id, action); }
        public static void Unregister<T>(T id, Action<Component> action) { Get<T>().Unregister(id, action); }
        public static void TryUnregister<T>(T id, Action<Component> action) { if (S) Get<T>().Unregister(id, action); }

        public static void Raise<T>(T id, GameObject v) { Get<T>().Raise(id);  }
        public static void Register<T>(T id, Action<GameObject> action) { Get<T>().Register(id, action); }
        public static void Unregister<T>(T id, Action<GameObject> action) { Get<T>().Unregister(id, action); }
        public static void TryUnregister<T>(T id, Action<GameObject> action) { if (S) Get<T>().Unregister(id, action); }
        #endregion

        public static MsgSystemGeneric<int> MsgInt
        {
            get { return S.msgSystemInt; }
        }

        private static bool autoInit = false;//set true to be automatically created
        private static MsgSystem s;
        public static MsgSystem S
        {
            get
            {
                if (s == null)
                {
                    s = GameObject.FindObjectOfType<MsgSystem>();
                    if (autoInit && s == null)
                    {
                        var go = new GameObject(typeof(MsgSystem).Name, typeof(MsgSystem));
                        s = go.GetComponent<MsgSystem>();
                        DontDestroyOnLoad(go);
                    }
                    autoInit = false;
                }
                return s;
            }
        }

        #endregion

        //#region SimpleRedirect Func
        //public static void Raise<T>(T id)
        //{
        //    Get<T>().Raise(id);
        //}
        
        //#endregion


        private Dictionary<System.Type, object> msgSystemDictionary = new Dictionary<Type, object>();
        private MsgSystemGeneric<string> msgSystemStr = new MsgSystemGeneric<string>();
        private MsgSystemGeneric<int> msgSystemInt = new MsgSystemGeneric<int>();



        private void Awake()
        {
            if (S != null && S != this)
            {
                Destroy(gameObject);
                throw new System.Exception("Singelton error. Already set " + s + " cant init " + this);
            }
            s = this;
            autoInit = false;
        }

        private void OnDestroy()
        {
            s = null;

        }

        #region Generic Msg system
        public class MsgSystemGeneric<T>
        {
            private static bool debug = true;

            internal MsgGenericKey msgSimple = new MsgGenericKey();
            internal MsgGenericKeyGenericAction<float> msgFloat = new MsgGenericKeyGenericAction<float>();
            internal MsgGenericKeyGenericAction<int> msgInt = new MsgGenericKeyGenericAction<int>();
            internal MsgGenericKeyGenericAction<string> msgString = new MsgGenericKeyGenericAction<string>();
            internal MsgGenericKeyGenericAction<bool> msgBool = new MsgGenericKeyGenericAction<bool>();
            internal MsgGenericKeyGenericAction<UnityEngine.Object> msgUnityObj = new MsgGenericKeyGenericAction<UnityEngine.Object>();
            internal MsgGenericKeyGenericAction<System.Object> msgObj = new MsgGenericKeyGenericAction<System.Object>();
            internal MsgGenericKeyGenericAction<Component> msgComp = new MsgGenericKeyGenericAction<Component>();
            internal MsgGenericKeyGenericAction<GameObject> msgGo = new MsgGenericKeyGenericAction<GameObject>();



            public MsgSystemGeneric()
            {
                if (debug)
                {
                    msgSimple.onEveryEvent += (t) => Debug.Log("Event " + t);
                }
            }

            public void RegisterAll(Action<T> action) { msgSimple.onEveryEvent += action; }
            public void UnregisterAll(Action<T> action) { msgSimple.onEveryEvent -= action; }


            /**Generic to allow specific targeting of object instead of accidently**/
            public void Raise<V>(T id, System.Object v) { msgSimple.Raise(id); msgObj.Raise(id, v); /*msgObj.Raise(id, v);*/ }
            public void Register<V>(T id, Action<System.Object> action) { msgObj.Register(id, action); if (debug) { Debug.Log("Registered on " + id); } }
            public void Unregister<V>(T id, Action<System.Object> action) { msgObj.Unregister(id, action); if (debug) { Debug.Log("Unregistered on " + id); } }

            public void Raise(T id) { msgSimple.Raise(id); }
            public void Register(T id, Action action) { msgSimple.Register(id, action); if (debug) { Debug.Log("Registered on " + id); } }
            public void Unregister(T id, Action action) { msgSimple.Unregister(id, action); if (debug) { Debug.Log("Unregistered on " + id); } }


            public void Raise(T id, float v) { msgSimple.Raise(id); msgFloat.Raise(id, v); /*msgObj.Raise(id, v);*/ }
            public void Register(T id, Action<float> action) { msgFloat.Register(id, action); if (debug) { Debug.Log("Registered on " + id); } }
            public void Unregister(T id, Action<float> action) { msgFloat.Unregister(id, action); if (debug) { Debug.Log("Unregistered on " + id); } }

            public void Raise(T id, int v) { msgSimple.Raise(id); msgInt.Raise(id, v);/*msgObj.Raise(id, v);*/ }
            public void Register(T id, Action<int> action) { msgInt.Register(id, action); if (debug) { Debug.Log("Registered on " + id); } }
            public void Unregister(T id, Action<int> action) { msgInt.Unregister(id, action); if (debug) { Debug.Log("Unregistered on " + id); } }

            public void Raise(T id, string v) { msgSimple.Raise(id); msgString.Raise(id, v); /*msgObj.Raise(id, v);*/ }
            public void Register(T id, Action<string> action) {  msgString.Register(id, action); if (debug) { Debug.Log("Registered on " + id); } }
            public void Unregister(T id, Action<string> action) {  msgString.Unregister(id, action); if (debug) { Debug.Log("Unregistered on " + id); } }

            public void Raise(T id, bool v) { msgSimple.Raise(id); msgBool.Raise(id, v); /*msgObj.Raise(id, v);*/ }
            public void Register(T id, Action<bool> action) { msgBool.Register(id, action); }
            public void Unregister(T id, Action<bool> action) { msgBool.Unregister(id, action); }

            public void Raise(T id, UnityEngine.Object v) { msgSimple.Raise(id); msgUnityObj.Raise(id, v); /*msgObj.Raise(id, v);*/ }
            public void Register(T id, Action<UnityEngine.Object> action) { msgUnityObj.Register(id, action); }
            public void Unregister(T id, Action<UnityEngine.Object> action) { msgUnityObj.Unregister(id, action); }

            public void Raise(T id, Component v) { msgSimple.Raise(id); msgComp.Raise(id, v); /*msgObj.Raise(id, v);*/ }
            public void Register(T id, Action<Component> action) { msgComp.Register(id, action); }
            public void Unregister(T id, Action<Component> action) { msgComp.Unregister(id, action); }

            public void Raise(T id, GameObject v) { msgSimple.Raise(id); msgGo.Raise(id, v); /*msgObj.Raise(id, v);*/ }
            public void Register(T id, Action<GameObject> action) { msgGo.Register(id, action); }
            public void Unregister(T id, Action<GameObject> action) { msgGo.Unregister(id, action); }

            public class MsgGenericKey : System.IDisposable
            {
                private Dictionary<T, Action> msgSystem = new Dictionary<T, Action>();
                public Action<T> onEveryEvent;

                public void Raise(T id)
                {

                    Action action;
                    if (msgSystem.TryGetValue(id, out action))
                    {
                        if (action == null)
                        {
                            Debug.LogError("NULL registeration action for " + id);
                        }
                        else
                        {
                            action.Invoke();
                        }
                    }
                    else
                    {
                        //Debug.LogWarning("No one register for " + id);
                    }
                    if (onEveryEvent != null) onEveryEvent.Invoke(id);

                }

                void Clear()
                {
                    msgSystem.Clear();
                }


                public void Register(T id, Action newAction)
                {

                    Action action;
                    if (msgSystem.TryGetValue(id, out action) == false)
                    {
                        msgSystem.Add(id, newAction);
                    }
                    else
                    {
                        action += newAction;
                        msgSystem[id] = action;
                    }
                    //if (id.ToString().ToLower() == "levelfinish")
                    //    Debug.LogFormat("--Register {0} #{1}", id, msgSystem[id].GetInvocationList().Length);
                }



                public void Unregister(T id, Action newAction)
                {

                    Action action;
                    if (msgSystem.TryGetValue(id, out action))
                    {
                        action -= newAction;
                        msgSystem[id] = action;

                        if (action == null || msgSystem[id].GetInvocationList().Length == 0)
                        {
                            msgSystem.Remove(id);
                        }
                    }
                    //if(id.ToString().ToLower() == "levelfinish")
                    //    Debug.LogFormat("--Unregister {0} #{1}", id, (msgSystem.ContainsKey(id)? (msgSystem[id] == null? "-0" :msgSystem[id].GetInvocationList().Length.ToString()) : "+0"));
                }

                #region IDisposable Support
                private bool disposedValue = false; // To detect redundant calls

                protected virtual void Dispose(bool disposing)
                {
                    if (!disposedValue)
                    {
                        if (disposing)
                        {
                            Clear();
                            // TODO: dispose managed state (managed objects).
                        }

                        // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                        // TODO: set large fields to null.

                        disposedValue = true;
                    }
                }

                // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
                // ~MsgSystemSimple() {
                //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                //   Dispose(false);
                // }

                // This code added to correctly implement the disposable pattern.
                public void Dispose()
                {
                    // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                    Dispose(true);
                    // TODO: uncomment the following line if the finalizer is overridden above.
                    // GC.SuppressFinalize(this);
                }
                #endregion
            }

            public class MsgGenericKeyGenericAction<E> : System.IDisposable
            {
                private Dictionary<T, Action<E>> msgSystem = new Dictionary<T, Action<E>>();

                public void Raise(T id, E v)
                {

                    Action<E> action;
                    if (msgSystem.TryGetValue(id, out action))
                    {
                        if(action == null) Debug.LogError("NULL action registration for " + id + " with input " + v);
                        else action.Invoke(v);
                    }
                    else
                    {
                        //Debug.LogWarning("No one register for " + id);
                    }
                }

                void Clear()
                {
                    msgSystem.Clear();
                }



                public void Register(T id, Action<E> newAction)
                {

                    Action<E> action;
                    if (msgSystem.TryGetValue(id, out action) == false)
                    {
                        msgSystem.Add(id, newAction);
                    }
                    else
                    {
                        action += newAction;
                        msgSystem[id] = action;
                    }
                }



                public void Unregister(T id, Action<E> newAction)
                {

                    Action<E> action;
                    if (msgSystem.TryGetValue(id, out action))
                    {
                        action -= newAction;
                        msgSystem[id] = action;
                        if (action == null || msgSystem[id].GetInvocationList().Length == 0)
                        {
                            msgSystem.Remove(id);
                        }
                    }
                }

                #region IDisposable Support
                private bool disposedValue = false; // To detect redundant calls

                protected virtual void Dispose(bool disposing)
                {
                    if (!disposedValue)
                    {
                        if (disposing)
                        {
                            Clear();
                            // TODO: dispose managed state (managed objects).
                        }

                        // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                        // TODO: set large fields to null.

                        disposedValue = true;
                    }
                }

                // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
                // ~MsgSystemSimple() {
                //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                //   Dispose(false);
                // }

                // This code added to correctly implement the disposable pattern.
                public void Dispose()
                {
                    // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                    Dispose(true);
                    // TODO: uncomment the following line if the finalizer is overridden above.
                    // GC.SuppressFinalize(this);
                }
                #endregion
            }

        }

        #endregion
    }
}