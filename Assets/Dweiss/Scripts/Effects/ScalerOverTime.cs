using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalerOverTime : MonoBehaviour {
    public float lerp;
    public float delta;
    public Vector3 targetSize;

    public UnityEngine.Events.UnityEvent onAnimationFinished;
    private Transform t;
	void Start () {
        t = transform;
        StartCoroutine(StartScalerOverTime());
	}
	
	// Update is called once per frame
	IEnumerator StartScalerOverTime() {
        while (Vector3.SqrMagnitude(t.localScale - targetSize) >= delta)
        {
            t.localScale = Vector3.Lerp(t.localScale, targetSize, lerp * Time.deltaTime);
            yield return 0;
        }
        t.localScale = targetSize;

        onAnimationFinished.Invoke();
    }
}
