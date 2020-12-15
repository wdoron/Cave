using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantVelocity : MonoBehaviour {
    public float speed;
    public bool startForward;
    public float addedRandomMoveOnImpact = 0;

    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if(startForward) rb.velocity = transform.forward * speed;
    }

    private void FixedUpdate()
    {
        rb.velocity = rb.velocity.normalized * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var p = collision.contacts[0];
        rb.velocity = ((Vector3.Project(collision.relativeVelocity, p.normal) + addedRandomMoveOnImpact * Random.onUnitSphere).normalized) * speed;
    }

}
