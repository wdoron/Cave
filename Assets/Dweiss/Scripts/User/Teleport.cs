using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Dweiss.Fade fader;
    [SerializeField] private Transform rotateHelper;
    private bool teleporting = false;
    public bool IsTeleporting => teleporting;


    public bool Teleporting { get { return teleporting; } }
    public void Fader(System.Action onBlack)
    {
        Fader(onBlack, () => { });
    }
    public void Fader(System.Action onBlack, System.Action onFinish)
    {
        teleporting = true;

        //onStart();

        fader.StartFade(() =>
        {
            onBlack();
        }, () => { teleporting = false; onFinish(); });
    }
    


    private static Vector3 GetTargetPosition(Transform tPos, Vector3 target)
    {
        var calcTarget = target - (/*tPos.position -*/ Camera.main.transform.position);
        var res = tPos.position + calcTarget;
        res.y = target.y;
        return res;
    }

    private Quaternion GetTargetRotation(Quaternion rot)
    {
        return Quaternion.Inverse(rotateHelper.rotation) * rot;
    }


    private void RotatePlayer(Transform rotateRig, Quaternion newRot)
    {
        var camT = Camera.main.transform;
        rotateHelper.rotation = newRot;

        camT.parent.SetParent(rotateRig.parent, true);
        rotateRig.position = camT.position;
        rotateRig.rotation = Quaternion.Euler(0, 0, 0);
        camT.parent.SetParent(rotateRig, true);

        rotateRig.rotation = Quaternion.Inverse(camT.rotation) * newRot;
        rotateRig.localEulerAngles = new Vector3(0, rotateRig.localEulerAngles.y, 0);
    }

    private void RotatePlayer(Transform rotateRig, Vector3 up, float rotateAngle)
    {
        var camT = Camera.main.transform;
        rotateRig.RotateAround(camT.position, up, rotateAngle);
    }
    public bool TryTeleportTo(Transform tPos, Transform tRot, Vector3 target, Quaternion? rot,
       System.Action onFinish)
    {
        if (IsTeleporting) return false;
        teleporting = true;
        Fader(
            () =>
            {
                transform.position = GetTargetPosition(transform, target);
                if (rot.HasValue) RotatePlayer(tRot, rot.Value);
            }
        , () => { teleporting = false; onFinish?.Invoke(); }
        );
        return true;
    }
    public bool TryTeleportNoFade(Transform tPos, Transform tRot, Vector3 target, Quaternion? rot)
    {
        if (IsTeleporting) return false;
        transform.position = GetTargetPosition(transform, target);
        if (rot.HasValue) RotatePlayer(tRot, rot.Value);
        return true;
    }

}
