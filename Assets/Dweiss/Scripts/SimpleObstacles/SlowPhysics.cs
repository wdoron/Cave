using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dweiss;

public class SlowPhysics : MonoBehaviour
{

    [Tooltip("Freeze: use 0 or 1 only")]
    [SerializeField] private Vector3 freezeValue;

    [Range(0,1)]
    public float timeScale;
    public LayerMask layer;
    public float FreezeSpeedEpsilon = .4f;

    [SerializeField]private Vector3 gravity = new Vector3(0, -10, 0);

    //private Vector3 prevPos;

    private Rigidbody rb;
    private Collider myCldr;

    private Vector3 velocity;
    public Vector3 Velocity
    {
        get { return velocity; }
    }
    public Vector3 Gravity { get { return gravity; } }

    public Vector3 NextVelocity
    {
        get { return velocity + AddedVelocity; }
    }

    //public float addedImpactFactor = .1f;

    private Collider stickToFloor = null;


    private float DeltaTime
    {
        get
        {
            return Time.fixedDeltaTime * timeScale;
        }
    }

    private Vector3 GravityForce
    {
        get
        {
            return gravity / rb.mass;
        }
    }

    private Vector3 AddedVelocity
    {
        get
        {
            return GravityForce * DeltaTime;
        }
    }


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myCldr = GetComponent<Collider>();
    }

    private void OnValidate()
    {
        freezeValue = new Vector3(freezeValue.x != 0 ? 1 : 0,
            freezeValue.y != 0 ? 1 : 0,
            freezeValue.z != 0 ? 1 : 0);

        timeScale = Mathf.Round( timeScale*16)/ 16;
    }

    bool TryMove()
    {

        float deltaTime = DeltaTime;
        var force = GravityForce;
        var dragDt = 1 - rb.drag * deltaTime;
        velocity += AddedVelocity * dragDt;

        velocity = freezeValue.PointMul(velocity);//Velocity should consider x,y,z freeze;

        var shift = velocity * deltaTime + .5f * force * deltaTime * deltaTime;
        var ray = new Ray(transform.position - shift.normalized*.05f, shift);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * shift.magnitude * 1.1f, Color.blue, 5f);
        if (RigidbodyExtension.RaycastAllFirst
            (ray, 
            shift.magnitude * 1.1f, layer, (rh) => rh.collider.isTrigger == false, out hit))
        {
          //  Debug.Log("found " + hit.collider);

            //var diffToPoint = hit.point - transform.position;
            //var distToCldr = myCldr.bounds.extents.PointMul(diffToPoint.normalized);
            //shift = -(distToCldr - diffToPoint) * .99f;
            //shift = freezeValue.PointMul(shift);

            //rb.MovePosition(rb.position + shift);
            if (KeepDistance(hit.collider) )
            {
            }
            TryFindFloor(hit.collider);
            return true;
        }
        shift = freezeValue.PointMul(shift);
        rb.MovePosition(rb.position + shift);
        return false;
    }

    void FixedUpdate()
    {
        ContinuePush();

        if (stickToFloor == null)
        {
            TryMove();
            //prevPos = transform.position;
        }
    }

    private bool KeepDistance(Collider cldr)
    {
        var pOther = cldr.ClosestPoint(transform.position);

        RaycastHit hit;
        var ray = new Ray(pOther, (pOther - transform.position));
       

        if (ray.direction.sqrMagnitude == 0)
        {
            Debug.Log("Ray " + pOther + "  " + ray.direction + " pos " + transform.position +
           " cldr " + cldr + " my " + myCldr);
            return true;
        }

        ray = new Ray(ray.origin + ray.direction * myCldr.bounds.size.sqrMagnitude, -ray.direction);
        Debug.DrawRay(ray.origin, ray.direction * myCldr.bounds.size.sqrMagnitude * 1.1f, Color.red, 5);
        if (myCldr.Raycast(ray, out hit, myCldr.bounds.size.sqrMagnitude*1.1f))
        {
            var shiftFromFloor = hit.point - pOther;
            var old = rb.position;
            shiftFromFloor = freezeValue.PointMul(shiftFromFloor);
            rb.MovePosition(rb.position + 0.99f *shiftFromFloor);
            Debug.Log("Moved " + old.ToMiliString() + " -> " + rb.position.ToMiliString() + " shift" + shiftFromFloor.ToMiliString());
            return true;
        }
        return false;
    }

    private void ContinuePush()
    {
        if (activeCldr)
        {
            //var dir
            KeepDistance(activeCldr);

            TryFindFloor(activeCldr);
            //var pMy = myCldr.ClosestPoint(activeCldr.transform.position);
            //var shiftFromFloor = pOther - pMy;
            //rb.MovePosition(rb.position + shiftFromFloor);
            //Debug.Log("Moved " + old.ToMiliString() + " -> " + rb.position.ToMiliString() + " shift" + shiftFromFloor.ToMiliString() );
        }
    }
    private Collider activeCldr;
    public void PushFrom(Collider cldr)
    {
        this.activeCldr = cldr;

        if (Vector3.Dot(transform.position - activeCldr.transform.position, velocity) < 0)
            ReverseSpeedOnObstacle(cldr);

        ContinuePush();
        
    }

    public void StopPushFrom(Collider cldr)
    {
        this.activeCldr = null;
    }

    bool TryFindFloor(Collider cldr)
    {

        var v = cldr.transform.position - transform.position;
        var isOnFLoor = Vector3.Dot(v, gravity) > 0;


        var currentSpeedSqr = velocity.sqrMagnitude;
       // var isPosOnFloor = Vector3.Dot(cldr.transform.position - prevPos, gravity) > 0; 
        if (isOnFLoor && currentSpeedSqr < (FreezeSpeedEpsilon* FreezeSpeedEpsilon))// || Vector3.Dot(velocity, transform.position - cldr.transform.position) > 0)
        {
            velocity = Vector3.zero;
            stickToFloor = cldr;
            return true;
        }
        return false;
    }

    private void OnEnable()
    {
        velocity = Vector3.zero;
        stickToFloor = null;
    }
   
    public void AddVelocity(Vector3 addV)
    {
        velocity += addV;// / timeScale / timeScale;
    }
    public void AddSimpleForce(Vector3 addedForce)
    {
        velocity += addedForce * DeltaTime;
    }

    private void ReverseSpeedOnObstacle(Collider cldr)
    {

        var p = cldr.ClosestPoint(transform.position);
       // var pMy = myCldr.ClosestPoint(cldr.transform.position);
        var normal = (transform.position - p).normalized;
        var dot = Vector3.Dot(normal, velocity);
        //Debug.Log("velocity " + velocity +
        //    " p " + p.ToMiliString() + " trns " + transform.position +
        //    " normal " + normal + " dot " + dot);

        if (dot > 0) normal = -normal;
        else if (dot == 0) normal = velocity.normalized;

        //velocity += AddedVelocity;
        // Debug.DrawRay(transform.position, velocity, Color.red, 10);
        velocity = Vector3.Reflect(velocity, normal);

        velocity = freezeValue.PointMul(velocity);
    }

    private void OnTriggerEnter(Collider cldr)
    {
        if (cldr.isTrigger == false )
        {
            ReverseSpeedOnObstacle(cldr);


            //Debug.DrawRay(transform.position, velocity, Color.green, 10);


            //Debug.DrawLine(p, transform.position + normal * 10, Color.blue, 10);
            //Debug.DrawLine(p, transform.position, Color.cyan, 10);

            //Debug.Log("Enter " + cldr);
            if (TryFindFloor(cldr))
            {
                //if (TryMove(false) == false)
                //    velocity = Vector3.zero;
            }

            //var pOther = cldr.ClosestPoint(transform.position);
            //var pMy = myCldr.ClosestPoint(cldr.transform.position);
            //var shiftFromFloor = pOther - pMy;
            //rb.MovePosition(transform.position + shiftFromFloor * .9f);
        }

        //var newV = Vector3.Reflect(rb.velocity, Vector3.up);

        //var force = newV.normalized * GetVelocityForMax() * bumperForce;
        //Debug.DrawRay(transform.position, force*10, Color.blue, 1);
        //Debug.DrawRay(transform.position, force, Color.red, 1);
        //Debug.Log("spd v " + GetVelocityForMax() + " newv " + force);
        //rb.velocity = force;


    }

    private void OnTriggerExit(Collider other)
    {
       // Debug.Log("OnTriggerExit " + other);
        if (stickToFloor == other)
        {
            velocity = Vector3.zero;
            //Debug.Log("Exit floor " + other);
            stickToFloor = null;
        }
    }
}
