using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class RigidBodyLerp : MonoBehaviour
    {
        public Transform target;
        public float lerp = 1;

        private Transform t;
        private Rigidbody rb;

        public Vector3 shift;


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
            var newPos = Vector3.Lerp(t.position, target.position + shift, lerp * Time.fixedDeltaTime);
            rb.position = (newPos);
        }
    }
}