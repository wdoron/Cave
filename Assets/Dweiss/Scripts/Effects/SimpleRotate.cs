using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour {
    public Vector3 axe;
    public float degreePerSec;

    private Transform t;


	void Start () {
        t = transform;
	}
	
	void Update () {
        t.Rotate(axe, degreePerSec * Time.deltaTime);
	}
}
