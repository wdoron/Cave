using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class TransformCopy : MonoBehaviour
    {
        public Transform target;
        public Vector3 posShift = Vector3.zero;
        public bool pos = true, rot = true;

        public bool myLocal = false, hisLocal = false;
        public float lerpMoveFactor = float.PositiveInfinity, lerpRotFactor = float.PositiveInfinity;

        private Transform t;
        private void Awake()
        {
            t = transform;
        }

        [ContextMenu("Set Shift")]
        private void SetPosShift()
        {
            posShift = transform.position - target.position;
        }

        private void Update()
        {
            if (myLocal == false)
            {
                if (pos) t.position = Vector3.Lerp(t.position, TargetPos + posShift, Time.deltaTime * lerpMoveFactor);
                if (rot) t.rotation = Quaternion.Lerp(t.rotation, TargetRot, Time.deltaTime * lerpMoveFactor);
            } else
            {
                if (pos) t.localPosition = Vector3.Lerp(t.localPosition, TargetPos + posShift, Time.deltaTime * lerpRotFactor);
                if (rot) t.localRotation = Quaternion.Lerp(t.localRotation, TargetRot, Time.deltaTime * lerpRotFactor);
            }
        }

        private Vector3 TargetPos { get { return (hisLocal ? target.localPosition :target.position) + posShift; } }
        private Quaternion TargetRot { get { return (hisLocal ? target.localRotation : target.rotation); } }
    }
}