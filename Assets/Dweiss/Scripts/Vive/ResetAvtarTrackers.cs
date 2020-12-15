using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class ResetAvtarTrackers : MonoBehaviour
    {
        public Transform leftVirtualTracker, rightVirtualTracker;
        public Lerp leftLegTracker, rightLegTracker;
        public TransformCopy leftFullMove, rightFullMove;
        private Transform head;


        //public void ResetLegsOrientationTrackingToDefault()
        //{
        //    SetLegsOrientationTracking(Vector3.up);
        //}
        //public void ResetLegsOrientationTrackingToAllDirection()
        //{
        //    SetLegsOrientationTracking(Vector3.zero);
        //}
        //public void SetLegsOrientationTracking(Vector3 axis)
        //{
        //    var leftTrl = leftVirtualTracker.GetComponent<TransformRotationLerp>();
        //    var rightTrl = rightVirtualTracker.GetComponent<TransformRotationLerp>();

        //    leftTrl.rotAxis = axis;
        //    rightTrl.rotAxis = axis;
        //}

        private void Awake()
        {
            head = Camera.main.transform;
        }

        private void OnEnable()
        {
            MsgSystemStr.S.Register("ViveResetStart", ResetTrackers);
        }
        private void OnDisable()
        {
            if (MsgSystemStr.S)
                MsgSystemStr.S.Unregister("ViveResetStart", ResetTrackers);
        }

        void ScaleLeg(Transform legModel, float legWidth)
        {
            legModel.parent.localScale = Vector3.one * legWidth;
            //legModel.localScale = Vector3.one * legWidth;
        }

        public Vector2 legRangeSize = new Vector2(.2f,.4f);
        public bool resetLegSize = true, autoDetectLegSide = true;

        public float leftWidthFactorFromLowerLeg = 1.6f;
        public float zFactorFromLeg = -.3f;
        public float xFactorFromLeg = .5f;

        private void CheckLegDir()
        {
            var headDistToRight = rightVirtualTracker.transform.position - head.position;
            var headDistToLeft = leftVirtualTracker.transform.position - head.position;

            DebugExt.DrawArrowLine(head.position, head.position + head.right * 10,
                Color.cyan, 10);

            DebugExt.DrawArrowLine(head.position - headDistToRight.normalized * .1f, head.position + headDistToRight.normalized * 3,
                Color.red, 10);

            DebugExt.DrawArrowLine(head.position, head.position + headDistToLeft.normalized * 3,
                Color.yellow, 10);
            var distToRight = Vector3.Dot(head.right, headDistToRight);
            var distToLeft = Vector3.Dot(head.right, headDistToLeft);
            Debug.LogFormat("CheckLegDir R/L: {0} | {1} ", distToRight, distToLeft);
            if (distToRight < distToLeft)
            {
                Debug.LogFormat("Swap legs trackers R/L: {0} | {1} ", distToRight, distToLeft);
                SwapLegs();
            }
        }

        private void SwapLegs()
        {
            var leftFoot = leftLegTracker.gameObject;
            var rightFoot = rightLegTracker.gameObject;



            var temp = leftVirtualTracker;
            leftVirtualTracker = rightVirtualTracker;
            rightVirtualTracker = temp;

            leftFoot.GetComponent<TransformRotationLerp>().target = leftVirtualTracker;
            leftFoot.GetComponent<Lerp>().target = leftVirtualTracker;
            leftFoot.GetComponent<ResetLegTracker>().virtualTracker = leftVirtualTracker;


            rightFoot.GetComponent<TransformRotationLerp>().target = rightVirtualTracker;
            rightFoot.GetComponent<Lerp>().target = rightVirtualTracker;
            rightFoot.GetComponent<ResetLegTracker>().virtualTracker = rightVirtualTracker;


            if(leftFullMove)leftFullMove.target = leftVirtualTracker;
            if(rightFullMove)rightFullMove.target = rightVirtualTracker;


            var tempTracker = leftLegTracker;
            leftLegTracker = rightLegTracker;
            rightLegTracker = tempTracker;



            Dweiss.Msg.MsgSystem.MsgStr.Raise("SwapLegs", tempTracker.transform.FullName() + " is now right leg" );
        }


        void ResetTrackers()
        {
            if (UnityEngine.XR.XRDevice.isPresent == false) return;

            if(autoDetectLegSide)
                CheckLegDir();

            if (resetLegSize)
            {
                var width = leftVirtualTracker.position - rightVirtualTracker.position;
                var legWidth = Mathf.Abs(width.magnitude / 2);
                leftLegTracker.shift.x = xFactorFromLeg * legWidth;
                rightLegTracker.shift.x = -xFactorFromLeg * legWidth;
                leftLegTracker.shift.z = rightLegTracker.shift.z = zFactorFromLeg * legWidth;
                var factor = legWidth * leftWidthFactorFromLowerLeg;
                factor = Mathf.Max(legRangeSize.x, Mathf.Min(legRangeSize.y, factor));
                ScaleLeg(leftLegTracker.transform.GetChild(0), factor);
                ScaleLeg(rightLegTracker.transform.GetChild(0), factor);

                Debug.Log("Leg scale to " + factor);
                MsgSystemStrFloat.S.Raise("LegsScaleReset", factor);
            }
        }
    }
}