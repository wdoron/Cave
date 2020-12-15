using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class RigidbodyForceTrack : MonoBehaviour
    {
        public Transform target;
        public float lerpFactor = 1f, lerpPrevSpeed = 0f;
        public float minVelocity = .00f;
        public bool trackRotation = false;
        private Rigidbody rb;

        private Transform t;
        private void Awake()
        {
            t = transform;

            rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            var point = target.position- t.position;
            //point = point * posFactor;
            var newV = point.normalized * Mathf.Max(point.magnitude / Time.fixedDeltaTime, minVelocity);
            // rb.position = target.position;

            rb.AddForce(newV* lerpFactor + (lerpPrevSpeed - 1) * rb.velocity, ForceMode.VelocityChange);
            if(rb.useGravity)
                rb.AddForce(-Physics.gravity * Time.fixedDeltaTime, ForceMode.VelocityChange);
            if (trackRotation) rb.rotation = target.rotation;
        }
    }
}