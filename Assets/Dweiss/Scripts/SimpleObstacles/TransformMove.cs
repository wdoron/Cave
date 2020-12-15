using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMove : MonoBehaviour {

    [SerializeField] private Vector3 target1;
    [SerializeField]private float meterPerSec;

    [SerializeField] private float waitTime;
    [SerializeField] private float startDelayTime;

    [SerializeField] private Vector3 target2;


    public float WaitTime
    {
        get
        {
            return waitTime;
        }
        set
        {
            waitTime = value;
        }
    }

    public float MeterPerSec
    {
        get
        {
            return meterPerSec;
        }
        set
        {
            meterPerSec = value;
        }
    }

    private float TimeToHalfCycle
    {
        get
        {
            return (target2 - target1).magnitude / meterPerSec;
        }
    }

    private void Start()
    {
        target1 = transform.rotation * target1 + transform.position;
        target2 = transform.position;
        StartCoroutine(MoveIt());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, transform.lossyScale.magnitude * .02f);
        Gizmos.color = Color.red;
        var realTarget = Application.isPlaying? target1 : transform.rotation * target1 + transform.position;
        Gizmos.DrawLine(transform.position, realTarget);
        Gizmos.DrawSphere(realTarget, transform.lossyScale.magnitude * .05f);
    }


    IEnumerator HalfCycle()
    {
        for (float endTime = Time.time + TimeToHalfCycle; endTime >= Time.time;)
        {
            transform.position = Vector3.Lerp(target1, target2, (endTime - Time.time) / TimeToHalfCycle);
            yield return 0;
        }
        var temp = target1;
        target1 = target2;
        target2 = temp;
    }


    IEnumerator MoveIt()
    {
        yield return new WaitForSeconds(startDelayTime);
        while (true)
        {

            yield return HalfCycle();
            yield return HalfCycle();
            yield return new WaitForSeconds(waitTime);
        }
    }
}
