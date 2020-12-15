using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class DontGoThroughCollider : MonoExtension
    {
        private Rigidbody rb;
        private Collider myCollider;
        public LayerMask layerMask;

        [SerializeField] private List<Track> toTrack;

        [System.Serializable]
        public class Track
        {
            public Collider cldr;
            [HideInInspector]public Vector3 lastPos;
        }

        private List<Collider> collided = new List<Collider>();

        private void Awake()
        {
            rb = GetComponentInParent<Rigidbody>();
            myCollider = GetComponentInChildren<Collider>();

        }

        private void OnEnable()
        {
            StartCoroutine(CoroutineAfterFixedUpdate());
        }
        private Vector3 lastPoint;
        
        IEnumerator CoroutineAfterFixedUpdate()
        {
            lastPoint = t.position;
            for (int i = 0; i < toTrack.Count; ++i)
            {
                toTrack[i].lastPos = toTrack[i].cldr.transform.position;
            }

            while (true)
            {
                yield return new WaitForFixedUpdate();

                //This will miss objects that moved faster then this one
                if(toTrack.Count > 0)
                    CheckTracked();
                else 
                    CheckRaycast();

                lastPoint = t.position;
            }
        }

        private void CheckRaycast()
        {
            var distV = t.position - lastPoint;
            var hits = Physics.RaycastAll(lastPoint, distV, distV.magnitude, layerMask.value);
            for (int i = 0; i < hits.Length; ++i)
            {
                if (hits[i].collider == myCollider) continue;

                if (hits[i].collider.isTrigger == false &&
                    collided.Contains(hits[i].collider) == false)
                {
                    ColliderMissed(hits[i].collider, hits[i].point);
                }
            }
        }

        private Bounds MoveArea(Vector3 p, Vector3 dir, Collider cldr)
        {
            var size = cldr.bounds.size;
            size = new Vector3(Mathf.Max(size.x, dir.x), Mathf.Max(size.y, dir.y), Mathf.Max(size.z, dir.z));
            var newBounds = new Bounds(p + dir / 2, size);//cldr.bounds
            DebugExt.DrawBox(newBounds.center, newBounds.extents, Color.cyan, Time.fixedDeltaTime);
            return newBounds;
        }

        private float DeltaDistance = -0.01f;
        private void CheckTracked()
        {
            var v2 = t.position - lastPoint;
            v2 = v2 + v2.normalized * DeltaDistance;

            var bounds1 = MoveArea(lastPoint, v2, myCollider);

            for (int i=0; i < toTrack.Count; ++i)
            {

                //myCollider.bounds.Intersects

                //Vector3 intersection;
                var pNew = toTrack[i].cldr.transform.position;
                var v1 = pNew - toTrack[i].lastPos;
                v1 = v1 + v1.normalized * DeltaDistance;

              
                var contains = collided.Contains(toTrack[i].cldr);
                if (contains == false)//Check when should collide
                {
                    var bounds2 = MoveArea(toTrack[i].lastPos, v1, toTrack[i].cldr);
                    var intersect = bounds1.Intersects(bounds2);
                    //var intersect = Math3d.LineLineIntersection(out intersection,
                    //    toTrack[i].lastPos, v1,
                    //    lastPoint, v2, true);
                    if (intersect)
                    {
                       // Debug.LogWarning("contains == false Checking " + toTrack[i].cldr);

                        ColliderMissed(toTrack[i].cldr, Vector3.zero);

                        //Debug.DrawLine(toTrack[i].lastPos, pNew, Color.yellow, 10);
                        //Debug.DrawLine(lastPoint, t.position, Color.magenta, 10);
                        //Debug.Break();
                    }
                }else
                {
                   // Debug.Log("contains == false Checking " + toTrack[i].cldr);

                }

                toTrack[i].lastPos = pNew;
            }
        }

        public float forceFactor = 1;
        private void ColliderMissed(Collider cldr, Vector3 intersectPoint)
        {
            
            var otherRb = cldr.GetComponentInParent<Rigidbody>();
            if (otherRb == null) return;

            var relativeV = (rb.velocity - otherRb.velocity) * forceFactor;
            Debug.LogWarning("Missed " + cldr + " with rb " + otherRb + "force " + relativeV.ToMiliString() + " OtherV " + otherRb.velocity.ToMiliString());
            //if(Vector3.Dot(relativeV, rb.velocity) < 0)
            //{
            //    relativeV = -relativeV;
            //}
            //DebugExt.DrawArrowRay(otherRb.position, relativeV, Color.blue, 10);
            //DebugExt.DrawArrowRay(otherRb.position, relativeV.normalized, Color.red, 10);
            otherRb.AddForce(relativeV);//, raycastHit.point);
            //TODO add force to this?
            //rb.AddForce(-relativeV);
        }

        private void OnCollisionEnter(Collision collision)
        {
            collided.Add(collision.collider);
        }

        private void OnCollisionExit(Collision collision)
        {
            collided.Remove(collision.collider);
        }
    }
}