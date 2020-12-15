using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class EventEnableMultiComponent : MonoBehaviour
    {
        public bool enableIt;

        public float delay = -1;
        public string[] strEvents;

        [Space(1.0f)]
        [Header("Components Affected")]
        public Renderer[] rndrs;
        [Space(3.0f)]
        public Collider[] cldrs;
        [Space(3.0f)]
        public GameObject[] gos;
        [Space(3.0f)]
        public MonoBehaviour[] monos;


        [ContextMenu("LoadAllRenders")]
        private void LoadAllRenders() { rndrs = GetComponentsInChildren<Renderer>(); }

        [ContextMenu("LoadAllCollider")]
        private void LoadAllCollider() { cldrs = GetComponentsInChildren<Collider>(); }

        [ContextMenu("LoadAllGameObject")]
        private void LoadAllGameObject() { gos = GetComponentsInChildren<GameObject>(); }

        [ContextMenu("LoadAllMonoBehaviour")]
        private void LoadAllMonoBehaviour() { monos = GetComponentsInChildren<MonoBehaviour>(); }

        private void OnEnable()
        {
            foreach (var str in strEvents)
                Dweiss.Msg.MsgSystem.MsgStr.Register(str, ChangeEnableWithDelay);
        }
        private void OnDisable()
        {
            if (Dweiss.Msg.MsgSystem.S != null)
            {
                foreach (var str in strEvents)
                    Dweiss.Msg.MsgSystem.MsgStr.Unregister(str, ChangeEnableWithDelay);
            }
        }

        public void ChangeEnableWithDelay()
        {
            if (delay < 0)
            {
                ChangeEnableNow();
            } else
            {
                this.WaitForSeconds(delay, ChangeEnableNow);
            }
        }


        public void ChangeEnableNow()
        {
            rndrs.Enable(enableIt);
            cldrs.Enable(enableIt);
            gos.SetActive(enableIt);
            monos.Enable(enableIt);
        }

        public void SetEnable(bool shouldBeEnabled)
        {
            rndrs.Enable(shouldBeEnabled);
            cldrs.Enable(shouldBeEnabled);
            gos.SetActive(shouldBeEnabled);
            monos.Enable(shouldBeEnabled);
        }
    }
}