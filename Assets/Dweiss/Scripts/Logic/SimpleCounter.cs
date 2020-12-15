using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class SimpleCounter : MonoBehaviour
    {
        public string msgSystemRegisterId = "CountAdd", msgSystemUnregisterId = "CountReduce";
        public string onCountChangeInfoId = "SimpleCount";
        [SerializeField] private int raiseOnReachCount;
        public bool resetOnReachCount;

        public Dweiss.SimpleEvent onReachCount = new SimpleEvent();
        public Dweiss.EventInt onCountChange = new EventInt();

        private int _count;
        public int Count { get { return _count; } set { _count = value; TestCount(); } }
        public void SetReachCount(float newCount) { raiseOnReachCount = (int)newCount; }

        private void Awake()
        {
            if (string.IsNullOrEmpty(msgSystemRegisterId) == false)
            {
                Msg.MsgSystem.MsgStr.Register(msgSystemRegisterId, Register);
            }

            if(string.IsNullOrEmpty(msgSystemUnregisterId) == false)
            {
                Msg.MsgSystem.MsgStr.Register(msgSystemUnregisterId, Unregister);
            }

            if (string.IsNullOrEmpty(onCountChangeInfoId) == false)
            {
                Debug.Log(name + " Registered to raise " + onCountChangeInfoId);
                onReachCount.AddAction(() => { RaiseCountChange(raiseOnReachCount); });
                onCountChange.AddAction(RaiseCountChange);
            }else
            {

                Debug.LogError(name + " Registered to raise " + onCountChangeInfoId);
            }
        }

        private void Start()//Allow enable
        {
           
        }

        private void RaiseCountChange(int v)
        {
            Debug.Log(name + " Raise count " + onCountChange + " v " + v + " other " + Msg.MsgSystem.MsgStr);
            if(Msg.MsgSystem.S)
                Msg.MsgSystem.MsgStr.Raise(onCountChangeInfoId, v);
        }

        private void OnDestroy()
        {
            if (Msg.MsgSystem.S)
            {
                Msg.MsgSystem.MsgStr.Unregister(msgSystemRegisterId, Register);
                Msg.MsgSystem.MsgStr.Unregister(msgSystemUnregisterId, Unregister);
            }
        }

        public void ResetCount()
        {
            _count = 0;
            onCountChange.Invoke(_count);
        }
        public void Increament()
        {
            Register();
        }
        public void Register()
        {
            if (enabled == false) return;

            _count++;
            TestCount();
        }

        private void TestCount()
        {
            if (_count == raiseOnReachCount)
            {
                onReachCount.Invoke();
                if (resetOnReachCount)
                {
                    _count = 0;
                }
            }
            onCountChange.Invoke(_count);
        }

        public void Unregister()
        {
            if (enabled == false) return;

            _count--;
            TestCount();
        }
    }
}