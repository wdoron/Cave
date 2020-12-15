using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss {
    public class MoveByCurve : MonoBehaviour {

        public AnimationCurve shiftCurve;

        public Vector3 axisShiftAndDistance;
        public float fixedGravity = 1f;
        public Vector3 target;
        public Rigidbody rb;
        public ConstantForce cf;
        public bool moveRigidBody = true;

        public float meterPerSec;

        private Transform t;
        private Vector3 startPos;
        private double heightMoved;

        public Dweiss.SimpleEvent onFinish;
        
        private Queue<Vector3> _lastPos = new Queue<Vector3>();

        private void Reset()
        {
            if (cf == null) cf = GetComponentInParent<ConstantForce>();
            if (rb == null) rb = GetComponentInParent<Rigidbody>();
        }

        private void Awake()
        {
            t = GetComponent<Transform>();
            
        }

        private void Start()
        {
            if (target.IsInfinity() )
            {
                target = Camera.main.transform.position;
            }
            MoveTowards(target);
        }
        private bool coroutineActive;
        public void MoveTowards(Vector3 newTarget)
        {
            target = newTarget;

            coroutineActive = true;
            _lastPos.Enqueue(transform.position);

            StartCoroutine(MoveTowards());
        }

        private void OnDisable()
        {
            coroutineActive = false;
        }
        public void StartPhysics()
        {
            StartPhysicsAfterCollision(null);
            //Debug.Log("Start physics after trigger");
        }

        public float collisionSlowFactor = 0.8f;

        public void StartPhysicsAfterCollision(Collision clsn)
        {

            if (coroutineActive)
            {
                coroutineActive = false;

                StopAllCoroutines();

               // var startTime = Time.time;
                var distance = Vector3.Distance(target, startPos);
                var timeToMove = distance / meterPerSec;

                if (rb)
                {
                    //float distance = Mathf.Infinity;
                    rb.isKinematic = false;
                    //var newV = (target - startPos).normalized * meterPerSec;
                    var first = _lastPos.Dequeue();
                    while (_lastPos.Count > 1) _lastPos.Dequeue();
                    var last = _lastPos.Dequeue();
                    var newV = (last - first).normalized * meterPerSec;

                    //newV.y = 0;
                    rb.velocity = newV;
                    if(clsn != null)
                    {
                        var point = clsn.contacts[0];
                        var afterHit = Vector3.Reflect(rb.velocity, point.normal);
                        rb.velocity = new Vector3(afterHit.x * collisionSlowFactor, afterHit.y *.3f + rb.velocity.y * .7f, afterHit.z * collisionSlowFactor);
                    }
                    //var cf = rb.GetComponent<ConstantForce>();
                }
                if (cf) cf.force = new Vector3(0, fixedGravity * -(float)(heightMoved / timeToMove), 0);//heightMoved  = approximation of gravity integral

                onFinish.Invoke();
            }
        }



        IEnumerator MoveTowards()
        {
            yield return new WaitForFixedUpdate();
            _lastPos.Enqueue(transform.position);

            //float distance = Mathf.Infinity;
            var startTime = Time.time;
            var percent = 0f;
            startPos = t.position;
            var distance = Vector3.Distance(target, startPos);
            var timeToMove = distance / meterPerSec;

            //double heightMoved = 0;
            
            while (startTime + timeToMove > Time.time)
            {
                percent = (Time.time - startTime) / timeToMove;
                var shift = axisShiftAndDistance * shiftCurve.Evaluate(percent);
                var newPos = Vector3.Lerp(startPos, target, percent) + shift;
                //var newPos = Vector3.MoveTowards(t.position, target.position, meterPerSec * Time.fixedDeltaTime);
                heightMoved += Mathf.Abs(newPos.y - t.position.y);
                if (rb && moveRigidBody) rb.MovePosition(newPos);
                else t.position = newPos;
                //distance = Vector3.SqrMagnitude(t.position - target.position);
                
                yield return new WaitForFixedUpdate();
                _lastPos.Enqueue(transform.position);
                while (_lastPos.Count > 5) _lastPos.Dequeue();
            }
            
            //if (rb)
            //{
            //    rb.isKinematic = false;
            //    //lastV = (target - t.position);
            //    var newV = (target - startPos).normalized * meterPerSec;
            //    newV.y = 0;
            //    rb.velocity = newV;
            //    GetComponent<ConstantForce>().force = new Vector3(0, fixedGravity * -(float)(heightMoved/ timeToMove), 0);
            //}
            if (rb && moveRigidBody) rb.MovePosition(target);
            else t.position = target;
            
            StartPhysics();


        }
        
    }
}