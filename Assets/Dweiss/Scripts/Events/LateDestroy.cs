using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateDestroy : MonoBehaviour {

    public bool disableInsteadOfDestroy = false;

    public float destoryAfter;
    public float minimizeTime = -1;
    public Vector3 targetSize = Vector3.zero;
	// Use this for initialization
	void Start () {
        if (disableInsteadOfDestroy == false) Destroy(gameObject, destoryAfter);
        else Invoke("DisableGameObject", destoryAfter);

        if(minimizeTime >= 0) StartCoroutine("Minimize");
	}
	
    private void DisableGameObject()
    {
        gameObject.SetActive(false);
    }
    IEnumerator Minimize()
    {
        yield return new WaitForSeconds(destoryAfter - minimizeTime);
        var endTime = Time.time + minimizeTime;
        var t = transform;
        var startScale = t.localScale;
        while (true)
        {
            t.localScale = Vector3.Lerp(startScale, targetSize, 1f - (endTime - Time.time) / minimizeTime);
            yield return 0;
        }
    }
}
