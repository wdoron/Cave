using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightHall : ATimeAnimation
{
    public GameObject[] slotsWalls;
    public Light[] lights;
    public float minIntensity;
    public float maxIntensity = float.NaN;

    public float maxScale;
    public float minScale = float.NaN;

    protected void OnValidate() {
        if (float.IsNaN(maxIntensity) && lights.Length > 0 && lights[0] != null)
            maxIntensity = lights[0].intensity;
        if (slotsWalls.Length > 0 && slotsWalls[0] != null && float.IsNaN(minScale)) {
            minScale = slotsWalls[0].transform.localScale.z;
        }
    }

    protected override void Interpolate(float perc) {
        var intensityValue = Mathf.Lerp(maxIntensity, minIntensity, perc);
        for (int i = 0; i < lights.Length; i++) {
            lights[i].intensity = intensityValue;
        }
        var scaleZ = Mathf.Lerp(minScale, maxScale, perc);
        for (int i = 0; i < slotsWalls.Length; i++) {
            slotsWalls[i].transform.localScale = 
                new Vector3(slotsWalls[i].transform.localScale.x, slotsWalls[i].transform.localScale.y, scaleZ);
        }



    }


}
