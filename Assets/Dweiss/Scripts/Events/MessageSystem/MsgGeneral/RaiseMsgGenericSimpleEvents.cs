using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss.Msg
{
    public class RaiseMsgGenericSimpleEvents<T> : RaiseMsgGeneric<T>
    {
        public enum SimpleEventEnum
        {
            None, Awake,Start,Enable,Disable,Destroy,
        }

        public SimpleEventEnum raiseOn;

        public int maxEvents = -1;

        public new void Raise()
        {
            --maxEvents;
            if (maxEvents != 0)
                base.Raise();
        }

    private void Awake()
        {
            if (raiseOn == SimpleEventEnum.Awake) Raise();
        }
        void Start()
        {
            if (raiseOn == SimpleEventEnum.Start) Raise();
        }
        private void OnEnable()
        {
            if (raiseOn == SimpleEventEnum.Enable) Raise();
        }
        private void OnDisable()
        {
            if (raiseOn == SimpleEventEnum.Disable) Raise();
        }
        private void OnDestroy()
        {
            if (raiseOn == SimpleEventEnum.Destroy) Raise();
        }
    }
} 