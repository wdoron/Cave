using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class DelayEvent : MonoBehaviour
    {
        public SimpleEvent afterDelay;


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
    }
}