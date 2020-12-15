using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class ResetLegTracker : MonoBehaviour
    {
        public Transform floorReference;
        public Transform virtualTracker;

        private Lerp tracker;
        private void Start()
        {
            tracker = GetComponentInParent<Lerp>();
        }

        private void OnEnable()
        {
            MsgSystemStr.S.Register("ViveResetEnd", ResetTracker);
        }
        private void OnDisable()
        {
            if(MsgSystemStr.S)
                MsgSystemStr.S.Unregister("ViveResetEnd", ResetTracker);
        }

        void ResetTracker()
        {
            var yShift = virtualTracker.position.y - (floorReference == null ? 0 : floorReference.position.y);
            tracker.shift.y = -yShift;

            virtualTracker.LookAt(virtualTracker.position + Vector3.forward*100, Vector3.up);
        }
    }
}