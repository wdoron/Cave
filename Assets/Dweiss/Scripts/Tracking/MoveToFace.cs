using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class MoveToFace : MonoBehaviour
    {
        public Transform target;
        public Vector3 shift;
        public float distance = 2.4f;
        public float lerp = 1, moveLegnth = 2, maxRadians = .4f, maxMagnitudeDelta = 0;
        public float deltaSqr = 0.001f, deltaAngle = .1f;



        private Vector3 startPos;
        public bool autoStart;
        public bool autoStartOnEnable;
        public bool endless;

        public UnityEngine.Events.UnityEvent onFinish;

        private Transform t;
        private void Awake()
        {
            t = transform;

            if (target == null) target = Camera.main.transform;
        }
        private void Start()
        {
           

            if(autoStartOnEnable == false && autoStart) MoveTowards(target);
        }

        private void OnEnable()
        {
            if (autoStartOnEnable) MoveTowards(target);
        }

        public void MoveTowards(Transform target)
        {
            startPos = transform.position;
            this.target = target;
            StopAllCoroutines();
            StartCoroutine(CoroutineMove());
        }

        public void Move()
        {
            MoveTowards(target);
        }

        private void OnDisable()
        {
            IsMoving = false;
        }
        public bool IsMoving
        {
            get;
            private set;
        }

        IEnumerator CoroutineMove()
        {
            IsMoving = true;
            var targetFace = target.position + target.forward * distance + target.rotation * shift;
            var angleDist = Vector3.Angle(t.forward, -target.forward);
            var startTime = Time.realtimeSinceStartup;
            while (endless || ((t.position - targetFace).sqrMagnitude > deltaSqr || angleDist > deltaAngle))
            {
                var moveTime = moveLegnth > 0;
                var lerpV = moveTime  ? (Time.realtimeSinceStartup - startTime) / moveLegnth : (lerp * Time.fixedUnscaledDeltaTime);
                t.position = Vector3.Lerp(moveTime? startPos : t.position, targetFace, lerpV); //lerp * Time.fixedUnscaledDeltaTime);

                t.rotation = Quaternion.LookRotation(
                    Vector3.RotateTowards(t.forward, -target.forward, maxRadians * Time.fixedUnscaledDeltaTime, maxMagnitudeDelta * Time.fixedUnscaledDeltaTime)
                    , target.up);

                targetFace = target.position + target.forward * distance + target.rotation * shift;
                angleDist = Vector3.Angle(t.forward, -target.forward);
                yield return new WaitForSecondsRealtime(0);
            }
            IsMoving = false;
            onFinish.Invoke();

        }
    }
}