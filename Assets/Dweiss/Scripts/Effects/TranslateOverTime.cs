using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class TranslateOverTime : MonoExtension
    {
        public Transform target;
        [Tooltip("Moves to (x*pos.x,y*pos.y,z*pos.z) ")]
        public Vector3 posFactor = Vector3.one;

        public float length;
        public AnimationCurve normalizedCurve;
        private float startTime;
        private Vector3 startPos;

        public Dweiss.SimpleEvent onFinish;
        public bool resetPosOnEnable;
        private void Awake()
        {
            startPos = t.position;
        }

        private void OnEnable()
        {
            startTime = Time.time;
            if(resetPosOnEnable)startPos = t.position;
        }

        private void OnDisable()
        {
            if (resetPosOnEnable == false) t.position = startPos;
        }
        //public EventFloat onPercentChange;
        void Update()
        {
            var percent = (Time.time - startTime) / length;
            var targetPos = target.position.PointMul(posFactor) + startPos.PointMul(Vector3.one - posFactor);
            t.position = Vector3.Lerp(startPos, targetPos, normalizedCurve.Evaluate(percent));
            //onPercentChange.Invoke(percent);
            if (percent >= 1)
            {
                onFinish.Invoke();
            }

        }
    }
}