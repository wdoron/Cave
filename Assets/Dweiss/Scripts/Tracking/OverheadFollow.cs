using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverheadFollow : MonoBehaviour
{
    public Transform target;

    public float faceSlerp;
    public float posLerp;

    public bool setupShiftOnStart;
    public Vector3 shiftPos, shiftLook;

    private Transform t;
    private void Awake()
    {
        t = transform;
    }

    void Start () {
        if (target == null) target = Camera.main.transform;

		if(setupShiftOnStart)
        {
            shiftPos = target.position - t.position;
        }

    }
	
    void FaceTarget()
    {
        var direction = ((target.position + shiftLook) - t.position).normalized;
        var lookRotation = Quaternion.LookRotation(direction);
        t.rotation = Quaternion.Slerp(t.rotation, lookRotation, faceSlerp*Time.deltaTime);
    }

    void FollowTarget()
    {
        t.position = Vector3.Lerp(t.position, (target.position - shiftPos), posLerp * Time.deltaTime);
    }

    void LateUpdate () {

        if (faceSlerp > 0) FaceTarget();
        if (posLerp > 0) FollowTarget();
    }
}
