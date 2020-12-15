using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepSlowPhysicsHeightConstant : MonoBehaviour {


    public float tooMuchDiffAddForce = 0.1f;
    public float addHeightFactor = 1;
    public float timeBetweenAdd = .2f;
    public bool autoSetHeight = true;

    private SlowPhysics sPhysics;
    private Vector3 maxHeight;
    private Vector3 preV;
    private float lastAdded;

    private int reachTop = 0, addedHeightCount;


    void Awake () {
        sPhysics = GetComponent<SlowPhysics>();
	}

    private void Start()
    {
        maxHeight = sPhysics.Gravity.normalized.PointMul(sPhysics.transform.position);
    }

    private void OnEnable()
    {
        preV = Vector3.zero;
        addedHeightCount = reachTop = 0;

        if (autoSetHeight)
        {
            maxHeight = sPhysics.Gravity.normalized.PointMul(sPhysics.transform.position);
        }
    }
    void FixedUpdate () {
        var currentDir = Vector3.Dot(sPhysics.Gravity, sPhysics.Velocity);
        var futureDir = Vector3.Dot(sPhysics.Gravity, sPhysics.NextVelocity) ;
        if(currentDir < 0 && futureDir > 0)
        {
            var curHeight = sPhysics.Gravity.normalized.PointMul(sPhysics.transform.position);
            if (autoSetHeight && maxHeight.sqrMagnitude < curHeight.sqrMagnitude)//TODO psoition depended (missing sign)
            {
                autoSetHeight = false;
                maxHeight = curHeight;
            }
            reachTop++;

            if (Time.time - lastAdded > timeBetweenAdd &&
                maxHeight.sqrMagnitude > curHeight.sqrMagnitude)
            {
                lastAdded = Time.time;
                //var curHeight = sPhysics.gravity.normalized.PointMul(sPhysics.transform.position);
                var diff = (maxHeight - curHeight);
                if (
                    diff.sqrMagnitude > tooMuchDiffAddForce)
                {
                    //  Time.fixedDeltaTime * sPhysics.TimeScale
                    sPhysics.AddVelocity(diff * addHeightFactor);
                    //transform.position -= diff * addHeight;
                    addedHeightCount++;
                }
            }
        }

        

        preV = sPhysics.Velocity;

    }
}
