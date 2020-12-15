using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTimeAnimation : ATimeAnimation
{
    public Dweiss.EventEmpty onStart, onEnd;
    public Dweiss.EventFloat onChange;

    public new void OnEnable()
    {
        base.OnEnable();
        onStart.Invoke();
    }
    public new void OnDisable()
    {
        base.OnDisable();
        onEnd.Invoke();
    }
    protected override void Interpolate(float perc)
    {
        if (perc == 0) onStart.Invoke();
        if (perc == 1) onEnd.Invoke();
        onChange.Invoke(perc);
    }
}
