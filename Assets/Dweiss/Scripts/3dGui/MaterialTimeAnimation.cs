using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialTimeAnimation : ATimeAnimation
{
    private Renderer[] myRndrs;
    public string parameter;

    public float factor = 1;
    public float added = 0;
    public float min = 0, max = float.MaxValue;
    protected new void Awake()
    {
        base.Awake();
        myRndrs = GetComponentsInChildren<Renderer>();
    }

    private void SetMaterialParameter(float v)
    {
        var newV = factor * v + added;
        newV = Mathf.Max(newV, min);
        newV = Mathf.Min(newV, max);

        for (int i = 0; i < myRndrs.Length; i++)
        {
            myRndrs[i].material.SetFloat(parameter, newV);
        }
    }

    public new void OnEnable()
    {
        base.OnEnable();
        SetMaterialParameter(0);
    }
   
    protected override void Interpolate(float perc)
    {
        SetMaterialParameter(perc);
    }
}
