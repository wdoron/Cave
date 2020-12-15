using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotTimeAnimation : ATimeAnimation
{
    public Transform target;
    public Vector3 axis = Vector3.up;
    private float _lastPerc;
    private const float FullDegrees = 360;
    protected new void Awake()
    {
        base.Awake();
        if (target == null) target = transform;
    }
    public new void OnEnable()
    {
        base.OnEnable();
        _lastPerc = reverse ? FullDegrees: 0;
    }
    public new void OnDisable()
    {
        base.OnDisable();
        _lastPerc = reverse ? FullDegrees : 0;
    }
    protected override void Interpolate(float perc)
    {
        var delta = perc - _lastPerc;
        target.Rotate(axis, FullDegrees * delta);
        _lastPerc = perc;
    }
}
