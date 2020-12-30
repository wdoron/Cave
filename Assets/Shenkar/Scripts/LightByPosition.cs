using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightByPosition : MonoBehaviour
{
    public ChangeByPosition cbp;
    public Light[] lightsIntensity;
    public Vector2 lightRange;

    private void OnEnable() {
        cbp.onFactorChanged.AddAction(OnValueChange);
    }
    private void OnDisable() {
        cbp.onFactorChanged.AddAction(OnValueChange);
    }


    protected void OnValueChange(float v) {
        var lightIntensity = Mathf.Lerp(lightRange.x, lightRange.y, v);
        for (int i = 0; i < lightsIntensity.Length; i++) {
            lightsIntensity[i].intensity = lightIntensity;
        }
        //var scale = Vector3.Lerp(scaleOne, scaleTwo, v);
        //for (int i = 0; i < trans.Length; i++) {
        //    trans[i].localScale = scale;
        //}
    }

   
}
