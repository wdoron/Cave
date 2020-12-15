using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dweiss
{

    public class UseLegToTranslate : MonoExtension
    {
        public Transform[] legs;
        public Transform referenceMove;
        public float moveFactor = 1, moveBackFactor = 1;

        private Vector3 lastPos;
        private int lastLegOnFloorId = -1;

        private int GetLegOnFloorId()
        {
            int ret = 0;
            for(int i=1; i < legs.Length; ++i)
            {
                if(legs[ret].position.y > legs[i].position.y)
                {
                    ret = i;
                }
            }
            return ret;
        }


        public System.Action<Vector3> onMove;

        void Update()
        {
            var legId = GetLegOnFloorId();
            var legPos = legs[legId].position - referenceMove.position;
            if (legId == lastLegOnFloorId)
            {
                var delta = legPos - lastPos;
                var distOnFloor = Vector3.ProjectOnPlane(delta, Vector3.up);
                var forwardValue = Vector3.Dot(t.forward, delta);
                var moveValue = distOnFloor * (forwardValue > 0 ? moveBackFactor : moveFactor); ;
                t.localPosition += moveValue;
                if (onMove != null) onMove(moveValue);
            }
            lastPos = legPos;
            lastLegOnFloorId = legId;

            //var moveValue = referene.position - t.position;
            //var delta = moveValue - lastPos;

            ////var distOnFloor = Vector3.ProjectOnPlane(delta, Vector3.up);
            ////t.position += distOnFloor * moveFactor;

            //var forwardValue = Vector3.Dot(t.forward, delta);
            //t.position += t.forward * forwardValue * (forwardValue > 0 ? moveFactor : moveBackFactor);


            //lastPos = moveValue;
        }
    }
}