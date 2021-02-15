using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyzeDistanceToColliders : MonoBehaviour
{
    public bool debug;
    public LayerMask toColliderWith;
    //public Dweiss.EventFloat distance;
    public Dweiss.EventFloat normalizedDist;

    public float radius;
    RaycastHit[] results = new RaycastHit[100];

    private void OnDrawGizmos() {
        
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    // Update is called once per frame
    void Update()
    {
        
        var count = Physics.SphereCastNonAlloc(
            transform.position, radius, Vector3.forward, results, 0, toColliderWith);
        var minSqrMagnitude = float.MaxValue;
        Collider closest = null;
        for (int i = 0; i < count; i++) {
            var p = results[i].collider.ClosestPoint(transform.position);
            Debug.DrawLine(transform.position, p, Color.white, 5);
            var sqrP = (p - transform.position).sqrMagnitude;
            if (sqrP < minSqrMagnitude) {
                minSqrMagnitude = sqrP;
                closest = results[i].collider;
            }
        }
        if(minSqrMagnitude < float.MaxValue) {
            var norm = minSqrMagnitude / (radius * radius);
            if (debug) {
                Debug.LogFormat("SphereCastNonAlloc #{0} >> {3} >> {1} ({2}) ", 
                    count, Mathf.Sqrt(minSqrMagnitude), norm, closest == null?"?": closest.name);
            }
            //distance?.Invoke(minSqrMagnitude);
            normalizedDist?.Invoke(norm);
        }
    }
}//0.2 -0.05 ==> r0.3 => 0.7-0.02
