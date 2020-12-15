using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontGoThroughThings : MonoBehaviour {

    // Careful when setting this to true - it might cause double
    // events to be fired - but it won't pass through the trigger
   // public bool sendTriggerMessage = false;

    public LayerMask layerMask = -1; //make sure we aren't in this layer 
    public float skinWidth = 0.1f; //probably doesn't need to be changed 

    private float minimumExtent;
    private float partialExtent;
    private float sqrMinimumExtent;
    private Vector3 previousPosition;
    private Rigidbody myRigidbody;
    private Collider myCollider;

    //initialize values 
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponentInChildren<Collider>();
        previousPosition = myRigidbody.position;
        minimumExtent = Mathf.Min(Mathf.Min(myCollider.bounds.extents.x, myCollider.bounds.extents.y), myCollider.bounds.extents.z);
        partialExtent = minimumExtent * (1.0f - skinWidth);
        sqrMinimumExtent = minimumExtent * minimumExtent;
    }


    private List<Collider> addedColliders = new List<Collider>();
    private List<Collider> removedColliders = new List<Collider>();

    void OnTriggerEnter(Collider cldr)
    {
        addedColliders.Add(cldr);
    }
    void OnTriggerExit(Collider cldr)
    {
        removedColliders.Add(cldr);
    }


    public bool considerCollisionAsWell = false;

    void OnCollisionEnter(Collision clsn)
    {
        if(considerCollisionAsWell) addedColliders.Add(clsn.collider);
    }
    void OnCollisionExit(Collision clsn)
    {
        if (considerCollisionAsWell) removedColliders.Add(clsn.collider);
    }

    public bool runAnyway;


    IEnumerator CoroutineCheckCollision(Ray ray, float movementMagnitude, string eventRoRaise, List<Collider> collidersToCheck)
    {
        yield return 0;
        CheckCollision(ray, movementMagnitude, eventRoRaise, collidersToCheck);
    }
    void CheckCollision(Ray ray, float movementMagnitude, string eventRoRaise, List<Collider> collidersToCheck) { 
        var hits = Physics.RaycastAll(ray, movementMagnitude, layerMask.value);

        for (int i = 0; i < hits.Length; ++i)
        {
            //if (hits[i].collider == null) continue;
            if (myCollider.isTrigger || hits[i].collider.isTrigger)
            {
                if (collidersToCheck.Contains(hits[i].collider) == false)
                {
                    //hits[i].collider.SendMessage("OnTriggerEnter", myCollider);
                    gameObject.SendMessage(eventRoRaise, hits[i].collider);
                    Debug.Log("Raised " + eventRoRaise + " manually");
                }
               
            }
        }
    }

    void FixedUpdate()
    {
        //have we moved more than our minimum extent? 
        Vector3 movementThisStep = myRigidbody.position - previousPosition;
        float movementSqrMagnitude = movementThisStep.sqrMagnitude;

        if (runAnyway || movementSqrMagnitude > sqrMinimumExtent)
        {
            float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
            var ray = new Ray(previousPosition, movementThisStep);
            //var backRay = new Ray(myRigidbody.position, -movementThisStep);

            //StartCoroutine(
            CheckCollision(ray, movementMagnitude, "OnTriggerEnter", addedColliders);
           // );
            //CheckCollision(backRay, movementMagnitude, "OnTriggerExit", removedColliders);

            //var hits = Physics.RaycastAll(ray, movementMagnitude, layerMask.value);

            //for(int i=0; i < hits.Length; ++i)
            //{
            //    //if (hits[i].collider == null) continue;
            //    if (myCollider.isTrigger ||  hits[i].collider.isTrigger)
            //    {
            //        if (addedColliders.Contains(hits[i].collider) == false)
            //        {
            //            //hits[i].collider.SendMessage("OnTriggerEnter", myCollider);
            //            gameObject.SendMessage("OnTriggerEnter", hits[i].collider);
            //            Debug.Log("Raised OnTriggerEnter manually");
            //        }
            //        if (removedColliders.Contains(hits[i].collider) == false)
            //        {
            //            //hits[i].collider.SendMessage("OnTriggerExit", myCollider);
            //            gameObject.SendMessage("OnTriggerExit", hits[i].collider);

            //            Debug.Log("Raised OnTriggerExit manually");
            //        }
            //        //removedColliders
            //    }
            //}
            removedColliders.Clear();
            addedColliders.Clear();

        }

        previousPosition = myRigidbody.position;
    }
}