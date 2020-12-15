using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class TourTargets : MonoExtension
    {
        public Transform[] targets;
        public float deltaSqrFromTarget = .04f;

        [SerializeField] private float fullSpeedFactor, smallSpeedFactor, goodAngle = 10;


        private int targetIndex;

        private Rigidbody rb;

        private Transform GetCurrentTarget() {
            if((t.position - targets[targetIndex].position).sqrMagnitude < deltaSqrFromTarget)
            {
                targetIndex = ++targetIndex % targets.Length;
            }

            return targets[targetIndex];
        }

        public float rotFactor = 1, minRotation = 5f;

        private Vector3 lastPos = Vector3.zero;

        private void RotateToTarget(Transform target)
        {
            var delta = t.localPosition - lastPos;

            var maxRot = Mathf.Max(delta.magnitude * rotFactor, minRotation * Time.deltaTime);
            var targetForward = (target.position - t.position);
            targetForward.y = 0;
            var nextForward = Vector3.RotateTowards(t.forward, targetForward.normalized, maxRot, float.MaxValue);
            //Debug.DrawRay(t.position, t.forward * 10, Color.blue);
            //Debug.DrawRay(t.position, targetForward.normalized * 7, Color.cyan);
            //Debug.DrawRay(t.position, nextForward * 5, Color.black);

            t.forward = nextForward;

            lastPos = t.localPosition;
        }

        private void Update()
        {
            if (targets.Length < 2) return;

            var target = GetCurrentTarget();
            RotateToTarget(target);
        }

        private void Start()
        {
            rb = GetComponentInChildren<Rigidbody>();
        }
        private void FixedUpdate()
        {
            var target = GetCurrentTarget();
            MoveToTarget(target);
        }
        private void MoveToTarget(Transform target)
        {

            var targetForward = (target.position - t.position);
            var angle = Vector3.Angle(t.forward, targetForward.normalized);
            var distLeft = targetForward.magnitude;
            var moveDistance = t.forward * Mathf.Min(distLeft, (angle < goodAngle ? fullSpeedFactor : smallSpeedFactor) * Time.fixedDeltaTime);

            if (rb)
                rb.MovePosition(t.position + moveDistance);
            else
            {
                t.position = t.position + moveDistance;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (targets == null || targets.Length == 0) return;
            for(int i=0; i < targets.Length; ++i)
            {
                Gizmos.DrawSphere(targets[i].position, .2f);

                Gizmos.DrawLine(targets[(i-1) < 0 ?(targets.Length-1) : i-1].position, targets[i].position);
                Gizmos.DrawLine(targets[(i + 1) % targets.Length].position, targets[i].position);
            }
        }
    }
}
