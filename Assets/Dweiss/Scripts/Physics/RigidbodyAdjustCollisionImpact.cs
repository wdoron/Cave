using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyAdjustCollisionImpact : MonoBehaviour
{

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private float impactFactor = 1, 
        maxImpactForce = 10, minImpactForce = 0, startVelocityFactor = .9f;

    public float ImpactFactor { get { return impactFactor; } set { impactFactor = value; } }
    public float StartVelocityFactor { get { return startVelocityFactor; } set { startVelocityFactor = value; } }

    //private int collideCount;
    //private Vector3 startV, firstImpulse;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()//show enable in scene
    {
        
    }
    public void ResetSettings()
    {
       // collideCount = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enabled == false) return;

        if (layerMask.HasLayer(collision.collider.gameObject))
        {
            //collideCount++;
            //if (collideCount == 1)
            //{
            //    startV = rb.velocity;
            //    firstImpulse = collision.impulse;
            //}

            OnCollision(collision);


            //Debug.Log("OnCollisionEnter with " + collision.collider.name + " >> " + collision.impulse.ToMiliStr());
        }
        //OnCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (enabled == false) return;

        if (layerMask.HasLayer(collision.collider.gameObject))
        {
            OnCollision(collision);
            //Debug.Log("OnCollisionStay with " + collision.collider.name + " >> " + collision.impulse.ToMiliStr());
        }
        //OnCollision(collision);
    }
    //private void OnCollisionExit(Collision collision)
    //{
    //    if (layerMask.HasLayer(collision.collider.gameObject))
    //    {
    //        //collideCount--;
    //        //if (collideCount == 0)
    //        //{
    //        //    // this.WaitForSecondsFixedTime(0, () =>
    //        //    OnCollision(startV - firstImpulse, startV);
    //        //    //);
    //        //}
    //        //Debug.Log("OnCollisionExit with " + collision.collider.name + " >> " + collision.impulse.ToMiliStr());
    //    }
    //}

    private void OnCollision(Collision collision)
    {

        //var forceImpulse = collision.relativeVelocity / Time.fixedDeltaTime;
        //var velocityImpulse = forceImpulse / rb.mass;

        Vector3 totalVelocity  = rb.velocity ;
        Vector3 startV = rb.velocity - collision.impulse;

        var impulse = collision.impulse;
        //startV = startV.normalized * startV.magnitude * reduceForce;

        var endCalculated = startV.normalized * startV.magnitude * startVelocityFactor +
            impulse.normalized * impulse.magnitude * impactFactor;

        endCalculated = endCalculated.normalized * Mathf.Max(
                 Mathf.Min(
                     endCalculated.magnitude
                     , maxImpactForce)
                 , minImpactForce);

        //var endV = 
        //     totalVelocity.normalized *
        //     Mathf.Max(
        //         Mathf.Min(
        //             startV.magnitude * reduceForce + 
        //             impulse.magnitude * impactFactor

        //             , maxImpactForce)
        //         , minImpactForce);

        var str = string.Format("totalV {3} startV {0} relativeV {1} impactF {4} => EndV {2}", 
            startV.ToMiliStr(), 
            collision.relativeVelocity.ToMiliStr(),
           endCalculated.ToMiliStr(), 
           totalVelocity.ToMiliStr()
           , collision.impulse.ToMiliStr());

        rb.velocity = endCalculated;
        //this.WaitForSecondsFixedTime(0, () => 
        //Debug.Log("Current V " + rb.velocity.ToMiliStr() + " >> " + str);

            //);
    }
    //public bool setV = false;
    //private void FixedUpdate()
    //{
    //    if(rb.velocity.sqrMagnitude > 0)
    //        Debug.Log("Current V " + rb.velocity.ToMiliStr());
    //}
}
