using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Dweiss {
    public class AreaBox : MonoBehaviour {


        public Dweiss.EventBool onAreaEnableChanged;
        public UnityEvent onEnabled;
        public UnityEvent onDisabled;


        public void SetEnable(bool isEnabled)
        {
            this.enabled = isEnabled;
        }

        public void Hide()
        {
            enabled = false;
        }

        public void Show()
        {
            enabled = true;
        }

        private void OnEnable()
        {
            onEnabled.Invoke();
            onAreaEnableChanged.Invoke(true);
        }

        private void OnDisable()
        {
            onDisabled.Invoke();
            onAreaEnableChanged.Invoke(false);
        }
    }
}
