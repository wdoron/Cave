using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class KeepInPlaneDistance : MonoBehaviour
    {
        public Transform target;
        public Vector3 distance = Vector3.forward;

        public Vector3 plane = Vector3.up;
        public float lerp = 1;

        private Transform t;
        private void Awake()
        {
            t = transform;
        }
        void Start()
        {
            if (target == null) target = Camera.main.transform;
            plane.Normalize();
        }

        void Update()
        {
            var targetShift = target.rotation * distance;
            if(plane.sqrMagnitude > 0)
            {
                targetShift = Vector3.ProjectOnPlane(targetShift, plane);
            }
            t.position =  Vector3.Lerp(t.position, target.position + targetShift, lerp * Time.deltaTime);

           
        }


    }
}