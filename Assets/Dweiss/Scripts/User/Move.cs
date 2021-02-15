using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dweiss;

public class Move : MonoBehaviour {


    public bool considerLocomotion = true;

    public float movePerSec;
    //  public float rotPerFrame;
    private Vector3 lastMousePos;

    public Transform rotateRig, rotateRig2, rotateHelper;
    private Camera mainCamera;
    private Transform camT;

  
    public bool useCamera = false;
    private Transform t
    {
        get
        {
            return useCamera ? Camera.main.transform : transform;
        }
    }

    private Transform rotT
    {
        get
        {
            return useCamera ? Camera.main.transform : rotateRig;
        }
    }

    public System.Action<Vector3> onRotate;
    public System.Action<Vector3> onUserMove;
    public System.Action onJump;
    public bool moveByAbsoluteAxis;

    [SerializeField] private float jumpHeight = .5f, jumpTime = 1f, jumpPower = .33f;
    private bool jumping;
    public bool IsJumping => jumping;


    private Dweiss.Locomotion locomotion;

    public float keyboardRotateFactor = 10;

    public System.Func<Vector3, Vector3> getNextP;

    private void Start()
    {
        locomotion = GetComponent<Dweiss.Locomotion>();
        mainCamera = Camera.main;
        camT = mainCamera.transform;
    }


    
    IEnumerator Jump()
    {
        jumping = true;
        var yPos = t.position.y;
        yield return HeightMove(jumpPower, yPos, yPos + jumpHeight);
        yield return HeightMove(1 / jumpPower, yPos + jumpHeight, yPos);

       
        jumping = false;
    }

    IEnumerator HeightMove(float factor, float startY, float endY)
    {
        var start = Time.time;
        //var yPos = t.position.y;
        while (Time.time < start + jumpTime)
        {
            var v = (Time.time - start) / jumpTime;
            v = Mathf.Pow(v, factor);
            t.position = new Vector3(t.position.x, Mathf.Lerp(startY, endY, v), t.position.z);
            yield return 0;
        }

        t.position = new Vector3(t.position.x, endY, t.position.z);
    }


   


    public void SimpleRotate(float rotAmount) { 
        
        if (rotAmount != 0) RotatePlayer(Vector3.up, Time.deltaTime * keyboardRotateFactor * rotAmount);
    }


    public void SimpleMove(float v, float h, float factor, float y = 0) {
        if (v == 0 && h == 0 && y == 0) return;

        var deltaPos = new Vector3(h, y, v) * movePerSec * Time.deltaTime * factor;

        if (moveByAbsoluteAxis == false) deltaPos = camT.rotation * deltaPos;

        Vector3 simpleDeltaPos = deltaPos;
        DeltaMove(simpleDeltaPos, y);
    }

    public void DeltaMove(Vector3 simpleDeltaPos) {
        DeltaMove(simpleDeltaPos, 0);
    }
    public void DeltaMove(Vector3 simpleDeltaPos, float y) { 

        if (useCamera == false)
            simpleDeltaPos = Vector3.ProjectOnPlane(simpleDeltaPos, Vector3.up);
        //Debug.Log("simpleDeltaPos " + simpleDeltaPos.ToMiliStr() + "h/v" + h + "/" + v);
        if (getNextP != null)
            simpleDeltaPos = getNextP(simpleDeltaPos);
        //Debug.Log("getNextP " + simpleDeltaPos.ToMiliStr());
        if (considerLocomotion && locomotion.GetZMoveFactor() > 1) {
            var newP = simpleDeltaPos / locomotion.GetZMoveFactor(); ;
            simpleDeltaPos = new Vector3(newP.x, simpleDeltaPos.y, newP.z);
            //Debug.Log("simpleDeltaPos3 " + simpleDeltaPos.ToMiliStr());
        }
        simpleDeltaPos = simpleDeltaPos + new Vector3(0, y, 0);
        //Debug.Log("simpleDeltaPos4 " + simpleDeltaPos.ToMiliStr() + " t.position " + t.position.ToMiliStr());
        t.position += simpleDeltaPos;
        //Debug.Log("t.position " + t.position.ToMiliStr());

        if (simpleDeltaPos.sqrMagnitude != 0) if (onUserMove != null) onUserMove(simpleDeltaPos);
    }

    public bool TryJump() {
        if (jumping == false) {
            StartCoroutine(Jump());
            if (onJump != null) onJump();
            return true;
        }
        return false;
    }

   




    public void CursorRotate(bool pressing, bool startPress, Vector3 pos)
    {
        if (startPress)
        {
            lastMousePos = pos;
        }
        else if (pressing)
        {
            //var oldPoint = Camera.main.ScreenToWorldPoint(lastMousePos);
            //var newPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //var point = 
            //Vector3.Angle(Input.mousePosition, startMouse);

            if (lastMousePos != pos)
            {
                var delta = new Vector3(Screen.width / 2, Screen.height / 2, 0) + pos - lastMousePos;
                var deltaPoint = mainCamera.ScreenPointToRay(delta);

                // Debug.DrawRay(deltaPoint.origin, (deltaPoint.direction) * 100);
                //var delta = - startMouse;

                rotateHelper.LookAt(deltaPoint.origin + deltaPoint.direction * 100);
                //Debug.Log("Point " + deltaPoint);



                var angle = camT.forward.SignAngle(rotateHelper.forward, camT.right);
                //if (angle != 0) RotatePlayer(Vector3.Cross(camT.forward, rotateHelper.forward) * Mathf.Sign(angle), angle);
                if (angle != 0) RotatePlayer(Vector3.Cross(camT.forward, rotateHelper.forward) * Mathf.Sign(angle), angle);

                lastMousePos = pos;
            }

        }
    }


    private void RotatePlayer(Vector3 up, float rotateAngle)
    {
        if (rotT != camT)
        {
            var tempParent = camT.parent;
            rotateRig2.SetParent(null, true);
            rotT.position = camT.position;//Fix position for simple rotate
            rotateRig2.SetParent(rotateRig, true);
        }

        //rotT.Rotate(up, rotateAngle);
        rotT.RotateAround(camT.position, up, rotateAngle);

        if (onRotate != null) onRotate(up.normalized * rotateAngle);
    }

    public void ResetTransform()
    {
        rotateRig.localRotation = Quaternion.Euler(Vector3.zero);
        rotateRig.localPosition = (Vector3.zero);

        rotateRig2.localRotation = Quaternion.Euler(Vector3.zero);
        rotateRig2.localPosition = (Vector3.zero);

        rotateHelper.localRotation = Quaternion.Euler(Vector3.zero);
        rotateHelper.localPosition = (Vector3.zero);
        //Camera.main.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    
}
