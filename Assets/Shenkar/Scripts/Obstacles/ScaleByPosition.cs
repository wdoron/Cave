using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleByPosition : ChangeByPosition
{
    public Vector3 firstScale;
    public Vector3 secondScale;

    public Transform[] toAffect;
    protected new void Reset()
    {
        base.Reset();
        firstScale = transform.localScale;
    }
    private void Awake() {
        if (toAffect == null || toAffect.Length == 0) toAffect = new Transform[] { transform };

        onFactorChanged.AddAction(OnFactorChanged);
    }

    private void OnFactorChanged(float v) {
        var localScale = Vector3.Lerp(secondScale, firstScale, v);
        for (int i = 0; i < toAffect.Length; i++) {
            toAffect[i].localScale = localScale;
        }
    }

}
