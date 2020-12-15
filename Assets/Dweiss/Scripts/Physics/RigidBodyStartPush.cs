using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class RigidBodyStartPush : MonoBehaviour
    {
        public Vector3 force;


        private void Start()
        {
            var rb = GetComponent<Rigidbody>();
            rb.AddForce(force);
        }
    }
}
