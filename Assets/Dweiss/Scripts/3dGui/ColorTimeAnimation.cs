using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTimeAnimation : ATimeAnimation
{
    private Renderer[] myRndrs;
    public Color startColor = new Color(0, 0, 0, 0), endColor = Color.white;
    public string colorName = "";

    protected new void Awake()
    {
        base.Awake();
        myRndrs = GetComponentsInChildren<Renderer>();
    }

    private void SetColor(Color newColor)
    {
        if (string.IsNullOrEmpty(colorName))
        {
            for (int i = 0; i < myRndrs.Length; i++)
            {
                myRndrs[i].material.color = newColor;
            }
        } else {
            for (int i = 0; i < myRndrs.Length; i++)
            {
                myRndrs[i].material.SetColor(colorName, newColor);
            }
        }
    }

    public new void OnEnable()
    {
        base.OnEnable();
        SetColor(startColor);
    }
    public new void OnDisable()
    {
        base.OnDisable();
        SetColor(startColor);
    }
    protected override void Interpolate(float perc)
    {
        var newColor = Color.Lerp(startColor, endColor, perc);
        SetColor(newColor);
    }
}
