using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeTimeAnimation : ATimeAnimation
{
    public float sizeFactor;
    private Vector3 _startSize;
    public bool setOnAwake = true;
    protected new void  Awake()
    {
        base.Awake();
       if (setOnAwake)
        {
            wasSet = true;
            _startSize = t.localScale;
        }
    }
    
    private bool wasSet = false;
    protected override void Interpolate(float perc)
    {
        if(wasSet == false)
        {
            wasSet = true;
            _startSize = t.localScale;
        }
        //if(perc == 0) _startPos = t.localPosition;
        float distance = (_startSize * sizeFactor - _startSize).magnitude;
        t.localScale = Vector3.MoveTowards(_startSize, _startSize * sizeFactor, distance * perc);
    }

}
