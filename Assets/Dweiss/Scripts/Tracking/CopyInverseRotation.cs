using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class CopyInverseRotation : MonoBehaviour
    {
        public Transform target;

        private Transform t;
        private void Awake()
        {
            t = transform;
        }
        // Use this for initialization
        void Start()
        {
            if (target == null) target = Camera.main.transform;
        }

        void LateUpdate()
        {
            t.rotation = Quaternion.LookRotation(-target.forward, target.up);
        }
    }
}