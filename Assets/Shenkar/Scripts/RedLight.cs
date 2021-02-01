using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLight : MonoBehaviour
{
    float lightIntensity = 0;
    Light redLight;
    // Start is called before the first frame update
    void Start()
    {
        redLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPercent(float per)
    {

        if(per < .3f)
        {
            redLight.intensity = (1 - per) * 3.2f;

            Debug.Log(per + " A lot INTENSITY: " + redLight.intensity);
        }
        else
        {
            redLight.intensity = (1 - per) * 2.4f;

            Debug.Log(per + " some INTENSITY: " + redLight.intensity);
        }




        // curWaitFactor = per;
        // if (per.InRange(pitchRange.x, pitchRange.y))
        // {
        //     var pitchPerc = 1 - ((per - pitchRange.x) / (pitchRange.y - pitchRange.x));
        //     audioS.pitch = pitchValues.x + (pitchValues.y - pitchValues.x) * pitchPerc;
        // }
    }
}
