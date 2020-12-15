using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayOnCollider : MonoBehaviour
{
    public LayerMask layerToWalkOn;
    public LayerMask layerToNotWalkOn;
    public MeshCollider cldrOnMiss;
    public float shiftRayYPos;
    public Transform tranOrigin;
    public UserManager userMngr;



    // Start is called before the first frame update
    private void OnEnable()
    {
        var mov = GetComponentInChildren<Move>();
        mov.getNextP += GetNextDeltaPosition;
        userMngr.filterPosition += GetNextPosition;
    }
    private void OnDisable() {
        var mov = GetComponentInChildren<Move>();
        mov.getNextP -= GetNextDeltaPosition;
        userMngr.filterPosition -= GetNextPosition;
    }

    private Vector3 GetNextDeltaPosition(Vector3 p) {
        if (cldrOnMiss == null) return p;

        var t = GetPointToWalkOn(tranOrigin.position + p + new Vector3(0, shiftRayYPos, 0));
            
        var ret = t - tranOrigin.position;
        ret.y += tranOrigin.localPosition.y;
        return ret;
    }

    private Vector3 GetNextPosition(Vector3 p) {
        var delta = GetNextDeltaPosition(p - transform.position);
        //Debug.LogFormat("p {0} tp {1} delta {2}", p, transform.position, delta);
        //delta.y -= tranOrigin.localPosition.y;
        return transform.position + delta;
    }


    private Vector3 Origin => tranOrigin.position + new Vector3(0, shiftRayYPos, 0);

    private Vector3? GetRaycastHitPoint(Vector3? origin = null, Vector3? dir = null) {
        var ray = new Ray(origin.HasValue? origin.Value : Origin, dir.HasValue? dir.Value :Vector3.down);
        RaycastHit hit;



        if (Physics.Raycast(ray, float.MaxValue, layerToNotWalkOn))
            return null;

        if (Physics.Raycast(ray, out hit, float.PositiveInfinity, layerToWalkOn)) {
            Debug.DrawLine(ray.origin, hit.point, Color.red, 5);
            return new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }
        return null;
    }

    private Vector3 GetPointToWalkOn(Vector3 source) {
        if (cldrOnMiss == null) return source;

        var p = GetRaycastHitPoint(source);
        
        if (p.HasValue) {
            return p.Value;
        } else {
            //var pOnBounds = cldrOnMiss.ClosestPoint(source);

            var pOnBounds = GetCloestPointInMesh(Origin);


            Debug.DrawLine(source, pOnBounds, Color.blue, 5);
            //GetRaycastHitPoint(pOnBounds + shiftRayYPos);
            return pOnBounds;// new Vector3(pOnBounds.x, tranOrigin.position.y, pOnBounds.z);
        }
    }

    private Vector3 GetCloestPointInMesh(Vector3 pOut) {
        var p = cldrOnMiss.transform.InverseTransformPoint(pOut);
        var vrtz = cldrOnMiss.sharedMesh.vertices;
        var minDist = float.MaxValue;
        var index = 0;

        for (int i = 0; i < vrtz.Length; i++) {
            var sqrDist = (vrtz[i] - p).sqrMagnitude;
            if ((vrtz[i] - p).sqrMagnitude < minDist) {
                index = i;
                minDist = (vrtz[i] - p).sqrMagnitude;
            }
        }
        return cldrOnMiss.transform.TransformPoint(vrtz[index]);
    }

    // Update is called once per frame
    //void LateUpdate()
    //{
    //    var transTargetPos = GetPointToWalkOn();
    //    //Get current y shift from target
    //    //var curShiftY = transform.position.y - tranOrigin.localPosition.y;
    //    var newShiftY = transTargetPos.y;// + tranOrigin.localPosition.y;

    //    //Deduce new shift
    //    transform.position = new Vector3(transTargetPos.x, newShiftY, transTargetPos.z);
    //}
}
