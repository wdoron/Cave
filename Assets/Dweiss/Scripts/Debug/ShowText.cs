using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowText : MonoBehaviour {

    public TextMesh txt;
    public bool showPosition;

    public Transform t;


    private void Reset()
    {
        t = transform;
    }

    private void Start()
    {
        if (t == null)
            t = Camera.main.transform;

    }

    private void Update()
    {
         txt.text = "" + (showPosition?  t.position.ToMiliString() : t.rotation.eulerAngles.ToMiliString());
    }
}
