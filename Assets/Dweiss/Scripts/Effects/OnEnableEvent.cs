using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss { 
public class OnEnableEvent : MonoBehaviour {
        public UnityEngine.Events.UnityEvent onEnable, onDisable;
        public Dweiss.EventBool onChangeEvent, onReverseChangeEvent;

        void OnEnable() {
            onEnable.Invoke();
            onChangeEvent.Invoke(true);
            onReverseChangeEvent.Invoke(false);
        }

        void OnDisable()
        {
            onDisable.Invoke();
            onChangeEvent.Invoke(false);
            onReverseChangeEvent.Invoke(true);
        }

    }
}