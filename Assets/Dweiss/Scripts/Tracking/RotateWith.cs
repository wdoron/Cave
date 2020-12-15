 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWith : MonoBehaviour {
    public Transform target;

    public Vector3 distance = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

    private Transform t;
    public float lerp = 1;

    private void Start()
    {
        //Debug.Log("Target " + target);
        if (target == null) target = Camera.main.transform;
        t = transform;
        if (distance.IsInfinity())
        {
            distance = Quaternion.Inverse(target.rotation) * (t.position - target.position);
        }
    }

    private void Update()
    {

        t.position = Vector3.Lerp(t.position, target.position + target.rotation * distance, lerp * Time.deltaTime);
    }

}
