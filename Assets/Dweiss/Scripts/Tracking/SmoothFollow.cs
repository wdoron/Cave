using UnityEngine;
using System.Collections;


namespace Common
{
    public class SmoothFollow : MonoBehaviour
    {
        public float firstWeight = .5f;
        public Transform[] target;
        public Transform rotTarget;

        public float maxDistancePerSec = 0f;
        public float lerpTimeInSec = 0;

        public bool fixedUpdate = false;

        private Vector3 GetPosition()
        {
            var totalPos = Vector3.zero;

            for (var i = 1; i < target.Length; ++i)
            {
                totalPos += target[i].position;
            }
            var otherWeight = target.Length <=1 ? 0 : (1f - firstWeight);
            totalPos = firstWeight * target[0].position + otherWeight * totalPos;
            return totalPos;
        }

        //private void OnEnable()
        //{
        //    if(target.Length > 0) transform.position = GetPosition();
        //}

        private void FixedUpdate()
        {
            if (fixedUpdate == false) return;
            Follow();
        }
        private void Update()
        {
            if (fixedUpdate) return;
            Follow();
        }

        public bool DistanceFollow
        {
            get { return maxDistancePerSec != 0; }
        }

        private void Follow() { 
            if(DistanceFollow)
            {
                transform.position = Vector3.MoveTowards(transform.position, GetPosition(), maxDistancePerSec * Time.deltaTime);
            } else
            {
                transform.position = Vector3.Lerp(transform.position, GetPosition(), lerpTimeInSec * Time.deltaTime);
            }
            if(rotTarget != null) transform.rotation = Quaternion.Lerp(transform.rotation, rotTarget.rotation, lerpTimeInSec * Time.deltaTime);
        }
    }
}