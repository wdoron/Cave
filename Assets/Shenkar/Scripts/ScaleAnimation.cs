using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAnimation : ATimeAnimation
{
    public Vector3 targetScale;
    
    private Vector3 startScale;

    protected new void Awake() {
        base.Awake();
        startScale = t.localScale;
    }

    protected override void Interpolate(float perc) {
        //if(perc == 0) _startPos = t.localPosition;
        t.localScale = Vector3.Lerp(startScale, targetScale, perc);
    }
}
