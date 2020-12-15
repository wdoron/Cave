using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class StretchBetweenPoints : MonoExtension
    {
        public Transform p1;

        //[Header("Used for forward as well")]
        public Transform p2;

        void Update()
        {
            var streachV = p2.position - p1.position;
            var size = streachV.magnitude / 2;
            t.position = (p2.position + p1.position) / 2;
            t.localScale = new Vector3(t.localScale.x, size, t.localScale.z);

            var legForward = Vector3.ProjectOnPlane(p2.forward, -streachV);

            t.rotation = Quaternion.LookRotation(legForward, -streachV);
        }
    }
}