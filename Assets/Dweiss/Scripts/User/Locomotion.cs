using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class Locomotion : MonoBehaviour
    {
        public bool useGlobalCoordinateMoveFactor = true;
        [SerializeField] private Vector3 moveFactor = Vector3.zero;



        public LayerMask wallLayer;
        public float kickback = .01f;
        private float Delta = 0.001f;

      //  public Dweiss.EventFloat onHeightChanged;

        private Vector3 lastCamPos, startingPos;
        private Transform cam, locomotionRig;

        public float GetZMoveFactor()
        {
            if (enabled && UseFactor) return moveFactor.x;
            return 0;
        }

        public void AdjustHeight(float val)
        {
            startingPos.y = val;
            transform.position = new Vector3 (transform.position.x, val, transform.position.z);
           // onHeightChanged.Invoke(val);
        }

        private void Awake()
        {
            startingPos = transform.position;
            cam = Camera.main.transform;
            lastCamPos = cam.position;
            locomotionRig = transform;
        }


        public void SetXZFactor(float factor)
        {
            moveFactor.x = factor;
            moveFactor.z = factor;
        }

        public void ResetLocomotionPosToZero()
        {
            locomotionRig.localPosition = Vector3.zero;
        }


        void DontMoveThroughWalls(Vector3 currentDiff)
        {
            RaycastHit hit;
            if (Physics.Raycast(new Ray(lastCamPos, currentDiff), out hit, currentDiff.magnitude + Delta, wallLayer))
            {
                Debug.DrawRay(lastCamPos, -currentDiff.normalized * 10, Color.green, 10);
                //Debug.Log("Hit raycast wall " + hit.collider + "," + hit.point);

                locomotionRig.position -= currentDiff + kickback * currentDiff.normalized;// 100 * ( cam.position - hit.point);//TODO fix it 
            }
        }

        void AddFactor(Vector3 currentDiff)
        {
            if (useGlobalCoordinateMoveFactor)
            {
                locomotionRig.position += currentDiff.PointMul(moveFactor - new Vector3(1, 0, 1));
            }
            else
            {
                locomotionRig.position += cam.rotation * currentDiff.PointMul(moveFactor - new Vector3(1, 0, 1));
            }
        }


        private void Update()
        {
            if (UseFactor == false) return;

            var diff = cam.position - lastCamPos;
            if (diff.sqrMagnitude > 0)
            {
                AddFactor(diff);
            }
        }

        

        void LateUpdate()
        {
            var diff = cam.position - lastCamPos;
            if (diff.sqrMagnitude > 0)
            {
                DontMoveThroughWalls(diff);
            }

            lastCamPos = cam.position;

        }


        //public void LocomotionYOnly(System.Func<float, float> funcFactor, Vector3 referenceStart)
        //{
        //    var diff = cam.localPosition.y;
        //    //Debug.Log("Diff " + diff);
        //    if (diff >= 0)
        //    {
        //        locomotionRig.position = new Vector3(locomotionRig.position.x, referenceStart.y + funcFactor(diff), locomotionRig.position.z);
        //    }
        //    else
        //    {
        //        locomotionRig.position = new Vector3(locomotionRig.position.x, referenceStart.y, locomotionRig.position.z);
        //    }
        //}

        bool _useFactor = true;
        public bool UseFactor
        {
            get { return _useFactor; }
            set
            {
                if (value && _useFactor == false)
                {
                    lastCamPos = cam.position;//Reset calculation
                }
                _useFactor = value;
            }
        }

        public void ResetPositionToCalibratedPos ()
        {
            transform.position = startingPos;
        }
        public void DontTrackThisFrame()
        {
            lastCamPos = cam.position;
            //transform.position = startingPos;
        }
        private void OnEnable()
        {
            lastCamPos = cam.position;
        }
    }
}