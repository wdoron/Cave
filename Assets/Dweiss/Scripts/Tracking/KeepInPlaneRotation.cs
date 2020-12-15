using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class KeepInPlaneRotation : MonoBehaviour
    {
        public Transform target;

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
            Quaternion targetRot;
            if (plane.sqrMagnitude == 0)
            {
                targetRot = target.rotation;
            }
            else
            {
                targetRot = Quaternion.LookRotation(Vector3.ProjectOnPlane(target.forward, plane), plane);
            }
            
            t.rotation = Quaternion.Lerp(t.rotation, targetRot, lerp * Time.deltaTime);


        }


    }
}
