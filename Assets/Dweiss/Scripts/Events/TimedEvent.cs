using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dweiss;

public class TimedEvent : MonoBehaviour {

    public UnityEngine.Events.UnityEvent onTimeEnded;

    public float timeOnEnable = 1;
    private void OnEnable()
    {
        StartCoroutine(WaitFor(timeOnEnable));
        //this.WaitForSeconds(timeOnEnable, () => onTimeEnded.Invoke());
    }


    IEnumerator WaitFor(float wait)
    {
        yield return new WaitForSecondsRealtime(wait);
        onTimeEnded.Invoke();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
