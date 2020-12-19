﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleByPosition : MonoBehaviour
{
    public Vector3 startArea, endArea;
    public ScaleBy scaleBy;
    public Vector3 secondScale;
    public Transform origin;

    public enum ScaleBy
    {
        SecondScaleAtCenter, SecondScaleAtStart

    }

    public Vector3 firstScale;
    // Start is called before the first frame update
    void Reset()
    {
        firstScale = transform.localScale;
    }
    public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value) {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }

    private float GetFactor() {
        var projectedPoint = Vector3.Project(origin.position, endArea - startArea);
        var v = InverseLerp(startArea, endArea, projectedPoint);
        
        return v;
    }

    void Update()
    {
        var v = GetFactor();
        if (v < 0) v = 0;
        if (v > 1) v = 1;
        if (scaleBy == ScaleBy.SecondScaleAtCenter) v = Mathf.Abs(v * 2 - 1);
        else if (scaleBy == ScaleBy.SecondScaleAtStart) v = 1 - v;
        transform.localScale = Vector3.Lerp(secondScale, firstScale, v);
        //Debug.Log("Value is " + GetFactor());
    }
}
