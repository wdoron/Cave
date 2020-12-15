using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UserManager : MonoBehaviour {
    public Move mover;
    public Teleport teleporter;
    public Transform rotateRig;
    public Dweiss.Locomotion locomotion;
    //public CalcAccel userForce;
    public System.Func<Vector3, Vector3> filterPosition;

    public bool IsTeleporting =>teleporter.IsTeleporting;

    public bool controlsMoveCameraInDevice = false;

    public bool ControlsMoveCameraInDevice
    {
        get { return controlsMoveCameraInDevice; }
        set { controlsMoveCameraInDevice = value; }
    }

    public void SetControlsMoveCameraInDevice(bool active)
    {
        controlsMoveCameraInDevice = active;
    }

    //[SerializeField] private Transform rotateHelper;

    // public Dweiss.EventBool onEnterDeviceFalse;

    public void ChangeParent(Transform newParent)
    {
        transform.parent = newParent;
    }


    bool wasEnabled = false;
    private void EnableMovement(bool useLocomotion)
    {
        if(useLocomotion == false)
        {
            wasEnabled = locomotion.UseFactor && locomotion.enabled;
        }
       // mover.enabled = useLocomotion;

        locomotion.UseFactor = useLocomotion == false? false : useLocomotion;
        if (useLocomotion && wasEnabled)//weird bug???
        {
            locomotion.enabled = false;
            locomotion.enabled = true;
        }
#if UNITY_EDITOR
        if(controlsMoveCameraInDevice) mover.useCamera = !useLocomotion;
#endif
    }
    
    public void EnterSimpleArea()
    {
        EnableMovement(false);
        SaveStartingPoint();
    }
    public void EnterSimpleArea(Transform parent, Vector3 point, Quaternion? rotation)
    {
        EnterSimpleArea();
        TryTeleport(parent, point, rotation, () => { });
    }

    public void ExitArea()
    {
        if (transform.parent == null)
        {
            EnableMovement(true);
            return;
        }

        transform.parent = null;
        teleporter.Fader(
            () =>
            {

                transform.localPosition = locomotionRigPos;
                transform.localRotation = locomotionRigRot;

                mover.rotateRig.localPosition = rotationRigPos;
                mover.rotateRig.localRotation = rotationRigRot;


                mover.rotateRig2.localPosition = rotationRigPos2;
                mover.rotateRig2.localRotation = rotationRigRot2;

               

                EnableMovement(true);
            }
            
            );

    }

    private Vector3 locomotionRigPos, rotationRigPos, rotationRigPos2;
    private Quaternion locomotionRigRot, rotationRigRot, rotationRigRot2;
    public void SaveStartingPoint()
    {
        locomotionRigPos = transform.localPosition;
        locomotionRigRot = transform.localRotation;

        rotationRigPos = mover.rotateRig.localPosition;
        rotationRigRot = mover.rotateRig.localRotation;

        rotationRigPos2 = mover.rotateRig2.localPosition;
        rotationRigRot2 = mover.rotateRig2.localRotation;
    }

    //public void Teleport(Vector3 pos, Quaternion? rot)
    //{
    //    teleporter.TeleportTo(transform, rotateRig, pos, rot, () => {  });
    //}
    public bool TryTeleport(Transform parent, Vector3 pos, Quaternion? rot, System.Action onFinish)
    {
        if(teleporter.IsTeleporting == false)
            transform.parent = parent;
        else return false;
        teleporter.TryTeleportTo(transform, rotateRig, pos, rot, onFinish);
        return true;
    }

    public bool TryTeleport(Vector3 pos, Quaternion? rot, System.Action onFinish) {
        if (filterPosition != null) pos = filterPosition(pos);

        if (teleporter.IsTeleporting) return false;
            teleporter.TryTeleportTo(transform, rotateRig, pos, rot, onFinish);
        return true;
    }
    public bool TryTeleportNoFade(Transform parent, Vector3 pos, Quaternion? rot)
    {
        if (teleporter.IsTeleporting == false)
            transform.parent = parent;
        else return false;
        teleporter.TryTeleportNoFade(transform, rotateRig, pos, rot);
        return true;
    }

    //private void RegisterCoin(Common.GameEventListener ls)
    //{
    //    listeners.Add(ls);
    //    coinCount = 0;
    //}
    //private void Unregister(Common.GameEventListener ls)
    //{
    //    listeners.Remove(ls);
    //}
    //public void OnCollects()
    //{
    //    coinCount++;
    //    if(coinCount == listeners.Count)
    //    {
    //        Debug.Log("Collected all stars");
    //    }
    //}
    //private void EmptyListeners()
    //{
    //    listeners = new List<Common.GameEventListener>();
    //}

    //private int coinCount = 0;
    //private List<Common.GameEventListener> listeners = new List<Common.GameEventListener>();
    //public Common.GameEvent ge;

    //private void OnEnable()
    //{
    //    ge.onItemRegister.AddListener(RegisterCoin);
    //    ge.onItemUnregister.AddListener(RegisterCoin);
    //    ge.onListenersEmpty.AddListener(EmptyListeners);
    //}
    //private void OnDisable()
    //{
    //    ge.onItemRegister.RemoveListener(RegisterCoin);
    //    ge.onItemUnregister.RemoveListener(RegisterCoin);
    //    ge.onListenersEmpty.RemoveListener(EmptyListeners);
    //}



}
