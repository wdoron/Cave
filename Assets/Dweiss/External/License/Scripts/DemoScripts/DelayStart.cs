using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss.Store
{
    public class DelayStart : MonoBehaviour
    {
        public bool runOnEnable = false;

        public DelayEvent[] delays;

        [System.Serializable]
        public class DelayEvent
        {
            public bool randomDelay;
            public float delay;
            public UnityEngine.Events.UnityEvent simpleEvent;
        }

        private void Awake()
        {
            if (runOnEnable == false && enabled)
                StartCoroutine(DelayCoroutine());

        }

      

        private void OnEnable()
        {
            if(runOnEnable) StartCoroutine(DelayCoroutine());
        }
        private void OnDisable()
        {
            if (runOnEnable) StopAllCoroutines();
        }

        public void RestartDelayCoroutines()
        {
            StopAllCoroutines();
            StartCoroutine(DelayCoroutine());
        }

        IEnumerator DelayCoroutine()
        {
            for (int i = 0; i < delays.Length; ++i)
            {
                if (delays[i].randomDelay)
                {
                    delays[i].delay = Random.Range(0, delays[i].delay);
                }
            }

            System.Array.Sort(delays, (a, b) => a.delay.CompareTo(b.delay));

            float totalWait = 0;
            for(int i=0; i < delays.Length; ++i)
            {
                
                var waitNow = delays[i].delay - totalWait;
                totalWait = delays[i].delay;
                if(delays[i].delay > 0 ) yield return new WaitForSeconds(waitNow);
                delays[i].simpleEvent.Invoke();
            }
            
        }
    }
}