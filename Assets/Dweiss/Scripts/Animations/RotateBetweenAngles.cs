using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class RotateBetweenAngles : MonoBehaviour
    {
        private const float AngleEpsilon = .1f;

        public float anglePerSec;

        public float minAngle = 20, maxAngle = 20;
        private Vector3 startForward;
        //private Quaternion startRot;

        private Vector3 MinForward
        {
            get
            {
                //var targetRot = startRot *= Quaternion.Euler(transform.up * 20);
                return Quaternion.AngleAxis(minAngle, transform.up) * startForward;
            }
        }

        private Vector3 MaxForward
        {
            get
            {
                //var targetRot = startRot *= Quaternion.Euler(transform.up * 20);
                return Quaternion.AngleAxis(maxAngle, transform.up) * startForward;
            }
        }

        void Awake()
        {
            startForward = transform.forward;

            GetTargetForward = () => MaxForward;
        }
        private bool aimingAtMax = true;
        private System.Func<Vector3> GetTargetForward;

        private void OnDrawGizmosSelected()
        {
            if (startForward.sqrMagnitude == 0) startForward = transform.forward;
            Gizmos.color = Color.white;
            Gizmos.DrawRay(transform.position, -transform.forward *10);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, -MaxForward * 10);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, -MinForward * 10);

        }

        void Update()
        {
            transform.forward = Vector3.RotateTowards(transform.forward, GetTargetForward(), Mathf.Deg2Rad * (anglePerSec) * Time.deltaTime, float.PositiveInfinity);

            var diff = Vector3.Angle(transform.forward, GetTargetForward());
            if (Mathf.Abs(diff) < AngleEpsilon)
            {
                if (aimingAtMax)
                    GetTargetForward = () => MinForward;
                else
                    GetTargetForward = () => MaxForward;

                aimingAtMax = !aimingAtMax;
            }
        }
    }
}