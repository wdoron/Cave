using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class CopyMovementRelativeToPivot : MonoBehaviour
    {
        public Transform reference, target;

        private Transform t;
        private void Awake()
        {
            t = transform;
        }

        void ReferenceCopy()
        {
            var dirToTarget = target.position - reference.position;

            var floorForward = Vector3.ProjectOnPlane(dirToTarget, reference.up);
            var reflectOnFloor = Vector3.Reflect(-floorForward, reference.forward);

            var heightDiff = dirToTarget - floorForward;
            var endDir = reflectOnFloor + heightDiff;

            //var angleOnForward = Vector3.SignedAngle(dirToTarget, reference.forward, reference.up);

            t.position = reference.position + endDir;

            //t.position = Vector3.Reflect(dirToTarget, reference.right) - dirToTarget;




        }

        void Update()
        {
            ReferenceCopy();
        }
    }
}