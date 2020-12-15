using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class RepetitiveEvent : MonoBehaviour
    {
        [SerializeField] private RepetitiveConfiguration[] events;

        [System.Serializable]
        public class RepetitiveConfiguration
        {
            public Vector2 waitForNextEventRange;
            public Dweiss.EventEmpty onEvent;
        }

        void Run()
        {
            for (int i = 0; i < events.Length; i++)
            {
                StartCoroutine(SingleEvent(events[i]));
            }
        }

        IEnumerator SingleEvent(RepetitiveConfiguration ev)
        {
            while (true)
            {
                var rndWait = Random.Range(ev.waitForNextEventRange.x, ev.waitForNextEventRange.y);
                yield return new WaitForSecondsRealtime(rndWait);
                ev.onEvent.Invoke();
            }
        }


        private void OnEnable()
        {
            Run();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}