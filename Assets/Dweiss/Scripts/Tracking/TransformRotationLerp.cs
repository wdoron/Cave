using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class TransformRotationLerp : MonoBehaviour
    {
        public Transform target;
        private Rigidbody rb;
        public float lerp = 1;

        public Vector3 rotAxis;

        private Transform t;
        private void Awake()
        {
            t = transform;

            rb = GetComponent<Rigidbody>();
        }
        void Start()
        {
            if (target == null) target = Camera.main.transform;
        }


        void Update()
        {
            if (rb) return;
            
            //No ridigbody
            if (rotAxis == Vector3.zero)
            {
                var newRot = Quaternion.Lerp(t.rotation, target.rotation, lerp * Time.fixedDeltaTime);
                t.rotation = (newRot);
            }
            else
            {
                var projectedForward = Vector3.ProjectOnPlane(target.forward, rotAxis);
                var newFrwd = Vector3.Lerp(t.forward, projectedForward, lerp * Time.fixedDeltaTime);
                var newRot = Quaternion.FromToRotation(Vector3.forward, newFrwd);

                t.rotation = newRot;
            }
        }

        private void FixedUpdate()
        {
            //with ridigbody
            if (rb == false) return;

            if (rotAxis == Vector3.zero)
            {
                var newRot = Quaternion.Lerp(t.rotation, target.rotation, lerp * Time.fixedDeltaTime);
                rb.rotation = (newRot);
            }
            else
            {
                var projectedForward = Vector3.ProjectOnPlane(target.forward, rotAxis);
                var newFrwd = Vector3.Lerp(t.forward, projectedForward, lerp * Time.fixedDeltaTime);
                var newRot = Quaternion.FromToRotation(Vector3.forward, newFrwd);

                rb.rotation = newRot;
            }
        }
    }
}