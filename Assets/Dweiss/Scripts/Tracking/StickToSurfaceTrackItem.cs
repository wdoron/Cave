using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class StickToSurfaceTrackItem : MonoBehaviour
    {
        public Transform target;
        public float lerp = 3;
        public Collider surface;
        public float dist;
        public Vector3 shiftTargetPos;
        private Transform t;
        private void Awake()
        {
            t = transform;
            if (target == null) target = Camera.main.transform;
        }

        void Update()
        {
            var pointOnBounds = surface.ClosestPointOnBounds(target.position + shiftTargetPos);
            pointOnBounds = pointOnBounds + (target.position - pointOnBounds).normalized * dist;
            t.position = Vector3.Lerp(t.position, pointOnBounds, lerp * Time.deltaTime);
        }
    }
}