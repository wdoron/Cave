using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class TranslateWithPart : MonoExtension
    {
        
        public Transform referene;
        public float moveFactor = 1, moveBackFactor;

        private Vector3 lastPos;

        private void Start()
        {
            lastPos = referene.position - t.position;
        }
        void Update()
        {
            var moveValue = referene.position - t.position;
            var delta = moveValue - lastPos;

            //var distOnFloor = Vector3.ProjectOnPlane(delta, Vector3.up);
            //t.position += distOnFloor * moveFactor;

            var forwardValue = Vector3.Dot(t.forward, delta);
            t.position += t.forward*forwardValue * (forwardValue > 0 ? moveFactor : moveBackFactor);


            lastPos = moveValue;
        }
    }
}