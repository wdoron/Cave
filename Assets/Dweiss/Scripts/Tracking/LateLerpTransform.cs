using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class LateLerpTransform : MonoBehaviour
    {
        public Transform target;
        public float lerpFactor = 50;

        private Transform t;
        private void Awake()
        {
            t = transform;
        }

        void LateUpdate()
        {
            t.position = Vector3.Lerp(t.position, target.position, lerpFactor * Time.deltaTime);
            t.rotation = Quaternion.Lerp(t.rotation, target.rotation, lerpFactor * Time.deltaTime);
        }
    }
}