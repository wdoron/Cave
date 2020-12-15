using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    [ExecuteInEditMode]
    public class RigidbodyMoveCycle : MonoBehaviour
    {
        Rigidbody rb;
        public Transform p1, p2;
        public float sqrDistanceToStop = .01f, accel = .5f, maxSpeedSqr = 0.3f, minSpeedSqr = 0.05f, deltaDistanceToSwitch = .001f;

        private Transform target;
        public Dweiss.SimpleEvent onDirChange;

        

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            target = p1;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(p1.position, .2f);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(p2.position, .2f);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            var targetV = target.position - transform.position;
            var v = target == p1 ? (p2.position - target.position) : (p1.position - target.position);
            
            var dot = Vector3.Dot(-targetV, v.normalized);
            if (targetV.sqrMagnitude < sqrDistanceToStop )
            {
                if(rb.velocity.sqrMagnitude > minSpeedSqr)
                    rb.AddForce(-targetV.normalized * accel);
                else
                {
                    rb.velocity = targetV.normalized * minSpeedSqr;
                }
            }
            else
            {
                rb.AddForce(targetV.normalized * accel);
            }

            if (rb.velocity.sqrMagnitude > maxSpeedSqr)
            {
                rb.velocity = rb.velocity.normalized * maxSpeedSqr;
            }
            
            //Debug.Log("Dot " + dot + " mg " + v.sqrMagnitude);
            if (targetV.sqrMagnitude < deltaDistanceToSwitch || dot < 0 )
            {
                target = target== p1? p2 : p1;
                onDirChange.Invoke();
            }
        }
    }
}