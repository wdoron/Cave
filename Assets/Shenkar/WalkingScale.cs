using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingScale : ATimeAnimationWalk
{
    public Vector3 targetScale;

    private Vector3 startScale;

    protected new void Awake()
    {
        base.Awake();
        startScale = t.localScale;
    }

    protected override void Interpolate(float perc)
    {
        //if(perc == 0) _startPos = t.localPosition;
        t.localScale = Vector3.Lerp(startScale, targetScale, perc);
    }
}

public abstract class ATimeAnimationWalk : MonoBehaviour
{

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
    public float scaleV = 0.2f;


    [Header("Misc")]
    public bool disableGoOnFinish = true;
    protected void Awake()
    {
        t = transform;
    }

    public void SetReverse(bool newReverse)
    {
        reverse = newReverse;
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

    public void scaleDown()
    {

        scaleV += 0.0013f;
        Interpolate(scaleV);
    }

    public void scaleUp()
    {

        scaleV -= 0.003f;
        Interpolate(scaleV);

    }
    private IEnumerator Animate()
    {
        if (startDelayTime > 0) yield return new WaitForSeconds(startDelayTime);
        if (pingPong)
        {
            reverse = false;
        }
        while (repeat)
        {
            var start = Time.time;
            //var lastValue = 0;
            while (Time.time - start < moveTime)
            {
                var v = (Time.time - start) / moveTime;
                if (reverse) { v = 1 - v; }
                if (interpolationPower != 1) v = Mathf.Pow(v, interpolationPower);
                Interpolate(v);
                // Interpolate(lastValue == 0?  v : (lastValue - v));
                yield return 0;
            }
            if (maxWaitTime > 0) yield return new WaitForSeconds(maxWaitTime);

            if (pingPong)
            {
                reverse = !reverse;
            }
        }
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