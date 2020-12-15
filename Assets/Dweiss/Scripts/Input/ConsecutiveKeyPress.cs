using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Dweiss
{
    
    public class ConsecutiveKeyPress
    {

        private Func<bool> isPressedDownFunc;

        private float _lastEventTime = float.NegativeInfinity;

        private float _waitBetweenEvents;

        public ConsecutiveKeyPress(Func<bool> isPressedDown, float waitBetweenEvents)
        {
            isPressedDownFunc = isPressedDown;
            _waitBetweenEvents = waitBetweenEvents;
        }

       // private float _diactivateTime = float.NegativeInfinity;
        //public void StopCurrentEvent()
        //{
        //    ConsecutiveEventCount = 0;
        //    _lastEventTime = float.NegativeInfinity;
        //    _diactivateTime = Time.time;
        //    IsConsecutiveEventRaisedNow = false;
        //}

        public int ConsecutiveEventCount { get; private set; }
        public bool IsConsecutiveEventRaisedNow { get; private set; }
        public void UpdateFrame()
        {
            //if (Time.time < _diactivateTime + _waitBetweenEvents)//Wait after eventFinished unexpectedly
            //    return;

            if (isPressedDownFunc())
            {
                if(Time.time > _lastEventTime + _waitBetweenEvents)
                {
                    ConsecutiveEventCount = 0;
                }
                ++ConsecutiveEventCount;
                _lastEventTime = Time.time;
            }
            else if (Time.time > _lastEventTime + _waitBetweenEvents)
            {
                IsConsecutiveEventRaisedNow = Time.time - Time.deltaTime < _lastEventTime + _waitBetweenEvents;
                
            }else
            {
                IsConsecutiveEventRaisedNow = false;
            }
            
        }

    }
}