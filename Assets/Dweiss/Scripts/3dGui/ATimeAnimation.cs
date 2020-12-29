using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ATimeAnimation : MonoBehaviour {

    protected Transform t;


    [Header("Time options")]
    public float moveTime = 1;
    public float interpolationPower = 1;

    [Header("Repeat options")]
    public bool reverse;
    public bool repeat;
    public bool pingPong = false;

    [Header("Delay options")]
    public float startDelayTime = 0;
    public float maxWaitTime = 0;

    [Header("Misc")]
    public bool disableGoOnFinish = false;
    protected void Awake()
    {
        t = transform;
    }

    public void SetReverse(bool newReverse)
    {
        reverse = newReverse;
    }
    public void ResetAnimation() {
        StopAllCoroutines();
        StartCoroutine(Animate());
    }
    public void OnEnable()
    {
        StartCoroutine(Animate());
    }
    public void OnDisable()
    {
        StopAllCoroutines();
    }
    protected abstract void Interpolate(float perc);

    private IEnumerator Animate()
    {
        if (startDelayTime > 0) yield return new WaitForSeconds(startDelayTime);
        if (pingPong)
        {
            reverse = false;
        }
        do {
            var start = Time.time;
            //var lastValue = 0;
            while (Time.time - start < moveTime) {
                var v = (Time.time - start) / moveTime;
                if (reverse) { v = 1 - v; }
                if (interpolationPower != 1) v = Mathf.Pow(v, interpolationPower);
                Interpolate(v);
                // Interpolate(lastValue == 0?  v : (lastValue - v));
                yield return 0;
            }
            if (maxWaitTime > 0) yield return new WaitForSeconds(maxWaitTime);

            if (pingPong) {
                reverse = !reverse;
            }
        } while (repeat);
        if (disableGoOnFinish)
        {
            gameObject.SetActive(false);
        }
        else
        {
            enabled = false;
        }
    }

}
