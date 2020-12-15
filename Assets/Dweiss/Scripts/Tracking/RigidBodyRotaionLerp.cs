using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class RigidBodyRotaionLerp : MonoBehaviour
    {
        public Transform target;
        public float lerp = 1;

        private Transform t;
        private Rigidbody rb;

        public Vector3 rotAxis;


        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        void Start()
        {
            if (target == null) target = Camera.main.transform;
            t = transform;
        }

        void FixedUpdate()
        {
            if (rotAxis == Vector3.zero)
            {
                var newRot = Quaternion.Lerp(t.rotation, target.rotation, lerp * Time.fixedDeltaTime);
                rb.rotation = newRot;
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