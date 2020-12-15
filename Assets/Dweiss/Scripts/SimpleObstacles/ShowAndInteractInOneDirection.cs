using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAndInteractInOneDirection : MonoBehaviour {


    public Vector3 activeDir = Vector3.forward;

    [SerializeField]private Collider[] clidrs;

    private Renderer[] rndrs;
    private Rigidbody rb;

    private void Reset()
    {
        clidrs = GetComponentsInChildren<Collider>();
    }

    void Awake () {
        rb = GetComponent<Rigidbody>();
        rndrs = GetComponentsInChildren<Renderer>();

    }

    public void ReverseDir()
    {
        activeDir = -activeDir;
    }

    private IEnumerator Start()
    {
        // allow physics to take effect
        DirectionChanged();
        yield return new WaitForFixedUpdate();
        DirectionChanged();
        yield return new WaitForSeconds(0.05f);
        DirectionChanged();
        yield return new WaitForFixedUpdate();
        DirectionChanged();
    }

    public void DirectionChanged()
    {
        var enabled = rb.velocity.sqrMagnitude == 0 || Vector3.Dot(rb.velocity, activeDir) > 0;
        clidrs.Enable(enabled);
        rndrs.Enable(enabled);

    }



}
