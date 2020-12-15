using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss.Store
{
    public class DelayEvent : MonoBehaviour
    {
        public UnityEngine.Events.UnityEvent afterDelay;


        public void Raise(float delay)
        {
            if(delay < 0)
            {
                afterDelay.Invoke();
            }else
            {
                this.WaitForSeconds(delay, afterDelay.Invoke);
            }
        }

        void WaitForSeconds(float delay, System.Action action)
        {
            StartCoroutine(CoroutineWaitForSeconds(delay, action));
        }

        IEnumerator CoroutineWaitForSeconds(float delay, System.Action action)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }
    }
}