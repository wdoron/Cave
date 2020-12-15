using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public static class RigidbodyExtension
    {

        public static bool RaycastAllFirst(Ray ray, float dist, LayerMask layer, System.Func<RaycastHit, bool> filter, out RaycastHit hit)
        {
            var allHits = Physics.RaycastAll(ray, dist, layer);
            //if(allHits.Length != 0) Debug.Log(allHits.ToCommaString(h => h.collider.name));
            for (int i = 0; i < allHits.Length; ++i)
            {
                if (filter(allHits[i]))
                {
                    hit = allHits[i];
                    return true;
                }
            }
            hit = new RaycastHit();
            return false;
        }

        public static Vector3[] CalculateMovement(this Rigidbody that,
               int stepCount, float timeBeteenStep)
        {
            return that.CalculateMovement(stepCount, timeBeteenStep, Vector3.zero, Vector3.zero);
        }
        public static Vector3[] CalculateMovement(this Rigidbody that,
               int stepCount, float timeBeteenStep, Vector3 addedSpeed)
        {
            return that.CalculateMovement(stepCount, timeBeteenStep, addedSpeed, Vector3.zero);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="that"></param>
        /// <param name="stepCount">Number of steps</param>
        /// <param name="timeBeteenStep">number of frames to skip</param>
        /// <param name="addedSpeed"></param>
        /// <param name="addedForce"></param>
        /// <returns></returns>
        public static Vector3[] CalculateMovement(this Rigidbody that,
               int stepCount, float timeBeteenStep, Vector3 addedSpeed, Vector3 addedForce)
        {
            var v = (that.isKinematic == false ? that.velocity : Vector3.zero);// + addedSpeed + addedV;
            var a = (that.useGravity && that.isKinematic == false ? Physics.gravity : Vector3.zero);
            return CalculateMovement(that.transform.position, v, a, stepCount, timeBeteenStep, addedSpeed, addedForce, that.mass, that.drag);
        }

        public static Vector3[] CalculateMovement(Vector3 position, Vector3 velocity, Vector3 acc,
               int stepCount, float timeBeteenStep, Vector3 addedSpeed, Vector3 addedForce, float mass, float drag)
        {
            var ret = new Vector3[stepCount];

            var addedV = (addedForce / mass) * Time.fixedDeltaTime;
            var v = velocity + addedSpeed + addedV;
            var a = acc;

            var x = position;
            var calc = new Vector3[] { x, v };
            for (var i = 0; i < stepCount; ++i)
            {
                calc = CalculateNewPos(calc[0], calc[1], a, drag, timeBeteenStep);
                ret[i] = calc[0];
            }
            return ret;
        }


        public static Vector3 CalcBallisticVelocityVectorProximation(this Rigidbody rb, Vector3 target, float angle)
        {
            var source = rb.position;
            Vector3 direction = target - source;
            float h = direction.y;
            direction.y = 0;
            float distance = direction.magnitude;
            float a = angle * Mathf.Deg2Rad;
            direction.y = distance * Mathf.Tan(a);
            distance += h / Mathf.Tan(a);

            // calculate velocity
            float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * a));
            return velocity * direction.normalized;
        }

        public static Vector3 CalcVelocityAfter(Vector3 v, Vector3 a, float drag, int deltaTimeCount)
        {
            var dt = Time.fixedDeltaTime;
            var aDt = a * dt;
            var dragDt = 1 - drag * dt;
            dragDt = dragDt < 0 ? 0 : dragDt;
            return (v + deltaTimeCount*aDt) * Mathf.Pow(dragDt, deltaTimeCount);
        }

        public static Vector3[] CalculateNewPos(Vector3 x, Vector3 v, Vector3 a, float drag, float deltaTimeCount)
        {
            var dt = Time.fixedDeltaTime;
            var aDt = a * dt;
            var dragDt = 1 - drag * dt;
            dragDt = dragDt < 0 ? 0 : dragDt;
            var acc = .5f * a * dt * dt;
            for (int i = 0; i < deltaTimeCount; ++i)
            {
                v = (v + aDt) * dragDt;
                x = x + v * dt + acc;
            }
            return new Vector3[] { x, v };
        }


        private static Vector3 CalculateVDrag(Vector3 v, Vector3 a, float drag, int deltaTimeCount)
        {
            var dt = Time.fixedDeltaTime;
            for (int i = 0; i < deltaTimeCount; ++i)
                v = (v + a * dt) * (1 - drag * dt);
            return v;
        }

        ////////Doesn't supports 
        public static Vector3[] CalculateTime(this Rigidbody that, Vector3 targetPos)
        {
            return CalculateTime(that, targetPos, Vector3.zero, Vector3.zero);
        }
        public static Vector3[] CalculateTime(this Rigidbody that, Vector3 targetPos,
            Vector3 addedSpeed)
        {
            return CalculateTime(that, targetPos, addedSpeed, Vector3.zero);
        }
        public static Vector3[] CalculateTime(this Rigidbody that, Vector3 targetPos,
            Vector3 addedSpeed, Vector3 addedForce)
        {

            var addedV = (addedForce / that.mass) * Time.fixedDeltaTime;

            var v = that.velocity + addedSpeed + addedV;

            var a = (that.useGravity && that.isKinematic == false ? Physics.gravity : Vector3.zero);

            var x0 = that.transform.position;
            //x = x0 +vt + .5*a*t^2
            //-b +- SQR(b*b - 4*a*c)/2*a // a= .5*a//b=v//a=x0-x
            //t12 = -v +- SQR(v*v -4 * .5 * a * (x0-x))/ 2 * .5 * a
            var x = x0 - targetPos;
            var sqr = (v.PointMul(v) - 4 * .5f * a.PointMul(x)).Sqrt();
            var t1 = (-v + sqr).PointDiv(2 * .5f * a);
            var t2 = (-v - sqr).PointDiv(2 * .5f * a);

            // a=0: (x0-x) + vt -> t = (x-x0)/v
            var tWhenA0 = x.PointDiv(v);
            //a = 0
            if (float.IsNaN(t1.x)) { t2.x = t1.x = tWhenA0.x; }
            if (float.IsNaN(t1.y)) { t2.y = t1.y = tWhenA0.y; }
            if (float.IsNaN(t1.z)) { t2.z = t1.z = tWhenA0.z; }
            // a = 0 && v = 0
            if (float.IsNaN(t1.x) && x0.x == targetPos.x) { t2.x = t1.x = 0; }
            if (float.IsNaN(t1.y) && x0.y == targetPos.y) { t2.y = t1.y = 0; }
            if (float.IsNaN(t1.z) && x0.z == targetPos.z) { t2.z = t1.z = 0; }
            return new Vector3[] { t1, t2 };
        }

        public static Vector3 Sqrt(this Vector3 v)
        {
            return new Vector3(Mathf.Sqrt(v.x), Mathf.Sqrt(v.y), Mathf.Sqrt(v.z));
        }
        public static Vector3 PointMul(this Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        public static Vector3 PointDiv(this Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }

    }
}