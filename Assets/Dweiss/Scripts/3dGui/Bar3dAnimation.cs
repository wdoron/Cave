using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar3dAnimation : MonoBehaviour {


    private float yMax;
    private Transform t;
    private Renderer r;
    public bool startEmpty = true;
    private void Awake()
    {
        r = GetComponentInChildren<Renderer>();
        t = transform;
        yMax = t.localScale.y;
        if(startEmpty) SetValue(0);
    }

    public void SetValue(float v)
    {
        r.enabled = (v != 0);
        t.localScale = new Vector3(t.localScale.x, v * yMax, t.localScale.z);
    }
}
