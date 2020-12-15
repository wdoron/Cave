using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class Lerp : MonoBehaviour
    {
        public Transform target;
        public float lerp = 1;
        public Vector3 shift;
        private Transform t;


        [ContextMenu("SetShiftNow")]
        void SetShiftNow()
        {
            shift = target.position - transform.position;
        }

        void Start()
        {
            if (target == null) target = Camera.main.transform;
            t = transform;
        }

        // Update is called once per frame
        void Update()
        {
            t.position = Vector3.Lerp(t.position, target.position + target.rotation*shift, lerp * Time.deltaTime);
        }
    }
}