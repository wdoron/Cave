using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dweiss.Common
{
    public class Collectable : MonoBehaviour
    {
        public CollectOption collectAt;
        public enum CollectOption
        {
            NoAutoCollect,
            CollectOnDisable,
            CollectAndRestoreOnEnableChange,
        }

        public CollectEvent collectEventSystem;
        

        public void Collect()
        {
            collectEventSystem.Collect(this);
        }
        public void Restore()
        {
            collectEventSystem.Restore(this);
        }

        private void OnEnable()
        {

            if (collectAt == CollectOption.CollectAndRestoreOnEnableChange) collectEventSystem.Restore(this);
            collectEventSystem.CollectRegister(this);
        }

        private void OnDisable()
        {
            if (collectAt == CollectOption.CollectOnDisable || collectAt == CollectOption.CollectAndRestoreOnEnableChange) collectEventSystem.Collect(this);

            collectEventSystem.CollectUnregister(this);
        }
        
    }
}