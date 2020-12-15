using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dweiss
{
    public class FollowAtSpeed : MonoExtension
    {

        [SerializeField] private Transform target;
        [SerializeField] private float maxAnglePerSec = 75, maxSpeedPerSec = .5f;

        public float posFractionOnRotate = .3f;

        void Update()
        {
            var angle = RotateTowards();
            if(angle <= .1f * maxAnglePerSec * Time.deltaTime)
            {
                MoveTowards(1f);
            } else
            {
                MoveTowards(posFractionOnRotate);
            }
        }
        void MoveTowards(float factor)
        {
            t.position = Vector3.MoveTowards(t.position, target.position, factor * maxSpeedPerSec * Time.deltaTime);
        }

        float RotateTowards()
        {
            var newForward = (target.position - t.position).normalized;
            newForward = Vector3.ProjectOnPlane(newForward, t.up);


            if (newForward.sqrMagnitude < .0025f)
            {
                var angleLeft = Quaternion.Angle(t.rotation, target.rotation);
                t.rotation = Quaternion.RotateTowards(t.rotation, target.rotation, maxAnglePerSec * Time.deltaTime);
                return angleLeft;
            }
            else
            {

                //var targetRot = Quaternion.FromToRotation(t.forward, newForward);
                
                var angle = Vector3.SignedAngle(t.forward, newForward, t.up);
                var angleToMove = Mathf.Min(Mathf.Abs(angle), maxAnglePerSec * Time.deltaTime) * Mathf.Sign(angle);


                var targetRot = Quaternion.LookRotation(newForward, t.up);
                DebugExt.DrawRays(t.position, Color.white, Time.deltaTime
                    , t.forward * 10
                    , newForward
                    , targetRot * Vector3.forward);

                //var newRot = Quaternion.RotateTowards(t.rotation, targetRot, maxAnglePerSec * Time.deltaTime);
                //t.rotation = newRot;

                var angleLeft = Quaternion.Angle(t.rotation, targetRot);

                t.Rotate(t.up, angleToMove);


                return angleLeft;
            }
        }
    }
}