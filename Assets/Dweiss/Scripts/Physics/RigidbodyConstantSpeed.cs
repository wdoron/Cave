using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyConstantSpeed : MonoBehaviour {


    [SerializeField] private float maxSpeed = 100;
    [SerializeField] private float slowRatePerSec = 1;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate () {
        if (maxSpeed < rb.velocity.magnitude)
        {
            rb.velocity = rb.velocity.normalized * Mathf.Max(maxSpeed, rb.velocity.magnitude * (1 - slowRatePerSec * Time.fixedDeltaTime));
        }
    }
}
