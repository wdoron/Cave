using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosTimeAnimation : ATimeAnimation
{
    public Vector3 movePos;
    private Vector3 _startPos;
    
    protected new void  Awake()
    {
        base.Awake();
        _startPos = t.localPosition;
    }

    protected override void Interpolate(float perc)
    {
        //if(perc == 0) _startPos = t.localPosition;
        t.localPosition = Vector3.MoveTowards(_startPos, (_startPos + movePos), movePos.magnitude* perc);
    }

}
