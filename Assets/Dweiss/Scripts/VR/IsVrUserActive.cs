using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class IsVrUserActive : MonoBehaviour
    {
        public Transform trans;

        public float timeBetweenCheck = 1;
        public float minFreezeTimeToDisable = .1f;

        public Dweiss.EventBool onUserActiveChange;
        public Dweiss.SimpleEvent onUserActive, onUserNotActive;

        private float lastStandTime = 0;

        private Vector3 lastPos;
        private Quaternion lastRot;

        private bool wasMoving = false;

        private void Reset()
        {
            trans = transform;
        }

        private void OnEnable()
        {
            StopAllCoroutines();
            this.SimpleCoroutine(TestIsActive);
        }

        public bool IsUserActive { get; private set; }


        public void Invoke()
        {
            onUserActiveChange.Invoke(IsUserActive);
        }


        public bool testHmd = false;
        float TestIsActive()
        {
            return 0;
            //var isOn = testHmd == false || UnityEngine.XR.XRDevice.userPresence == UnityEngine.XR.UserPresenceState.Present;
            //var isMoving = (trans.localPosition != lastPos || lastRot != trans.localRotation) && isOn;

            //lastRot = trans.localRotation;
            //lastPos = trans.localPosition;

            //if(wasMoving != isMoving)
            //{
            //    wasMoving = isMoving;

            //    if (isMoving == false)
            //    {
            //        lastStandTime = Time.time;
            //    }
            //    else
            //    {
            //        IsUserActive = true;

            //        onUserActive.Invoke();
            //        onUserActiveChange.Invoke(true);
            //    }
            //}
            //if (isMoving == false && Time.time - lastStandTime >= minFreezeTimeToDisable)
            //{
            //    lastStandTime = Mathf.Infinity;
            //    IsUserActive = false;

            //    onUserNotActive.Invoke();
            //    onUserActiveChange.Invoke(false);
            //}
            //return timeBetweenCheck;
        }

    }
}