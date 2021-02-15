using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeOnView : MonoBehaviour
{
    [Header("References")]
    public Camera cam;
    public Renderer rndr;
    public Transform target;

    [Header("algo")]
    public Vector3 firstPos;
    public Vector3 lastPos;
    public float lerpToPos = .3f;
    public Algo algo;
    private bool wasVisible;
    private System.Random rnd;

    public enum Algo
    {
        GetCloseToCameraHeight,
        RandomPos
    }
    public Dweiss.EventBool onNotVisible;

    private void Reset() {
        cam = Camera.main;
        rndr = GetComponent<Renderer>();
        target = transform;
    }
    private void Awake() {
        rnd = new System.Random();
    }
    private void Start() {
        wasVisible = rndr.IsVisibleFrom(cam);
    }
    void OnVisibleChanged(bool isNowVisible) {
        if (!isNowVisible) {
            if (algo == Algo.RandomPos) {
                var newP = firstPos + rnd.RandomInVector(lastPos - firstPos);
                target.localPosition = Vector3.Lerp(target.localPosition, newP, lerpToPos);
            } else if (algo == Algo.GetCloseToCameraHeight) {
                var height = rnd.Next(1, 6) * 0.1f;
                var newY = cam.transform.position.y + height;
                target.position = new Vector3(target.position.x, newY, target.position.z);
            } else {
                throw new System.NotSupportedException("algo " + algo);
            }
            //Debug.Log(isNowVisible ? "Visible" : "NotVisible");
        }
        onNotVisible.Invoke(!isNowVisible);
    }
    //private bool Hit

    // Update is called once per frame
    void Update()
    {
        if (rndr.IsVisibleFrom(cam)) {
            if (wasVisible == false) OnVisibleChanged(true);
            wasVisible = true;
        } else {
            if (wasVisible == true) OnVisibleChanged(false);
            wasVisible = false;
        }
        //cam.ViewportPointToRay(Vector2.zero)
    }
}
