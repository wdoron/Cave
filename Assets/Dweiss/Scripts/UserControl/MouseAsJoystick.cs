using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAsJoystick : MonoBehaviour
{
    public Transform target;

    public float transMoveSpeed, transWheelSpeed;
    public float rotMoveSpeed, rotWheelSpeed;


    private Vector3 currentAxisValue;//, currentDiff;
    private Vector3 totalRot;


    public KeyCode bindButton = KeyCode.LeftShift;

    public MouseButtons moveButton;
    public MouseButtons rotationButton;

    public KeyCode resetButton;

    public enum MouseButtons : int
    {
        Left = 0,
        Right = 1,
        Middle = 2,
        None
    }

    public enum MoveRelative
    {
        MoveRelativeToWorld,
        MoveRelativeToTarget,
        MoveForwardRelativeToFloor
    }
    public bool forwardOnFloorOnly = false;

    private void RotateUsingMouse()
    {
        totalRot.z += currentAxisValue.x * rotMoveSpeed ;
        totalRot.x += currentAxisValue.y * rotMoveSpeed;
        totalRot.y += currentAxisValue.z * rotWheelSpeed;

        target.localRotation = Quaternion.AngleAxis(totalRot.z, Vector3.forward)
         * Quaternion.AngleAxis(totalRot.x, Vector3.up)
         * Quaternion.AngleAxis(totalRot.y, Vector3.right)
         ;

    }

    private void MoveUsingMouse()
    {
        Vector3 diff;
        diff.x = currentAxisValue.x * transMoveSpeed;
        diff.y = currentAxisValue.y * transMoveSpeed;
        diff.z = currentAxisValue.z * transWheelSpeed;

        var forward = target.forward;
        var up = target.up;
        var right = target.right;
        if (forwardOnFloorOnly)
        {
            forward = Vector3.ProjectOnPlane(forward, Vector3.up);
            up = Vector3.up;
            right = Vector3.Cross(up, forward);
        }
        //if (moveRelativeToWorld)
        //{
        //    forward = Vector3.ProjectOnPlane(forward, Vector3.up);
        //    up = Vector3.up;
        //    right = Vector3.Cross(up, forward);
        //}

        target.localPosition += forward * diff.y + right * diff.x + up * diff.z;

    }

  
    private void Reset()
    {
        target = transform;
    }

    private float NormalizeAxis(float axis)
    {
        return axis > 0 ? 1 : (axis < 0 ? -1 : 0);
    }

    void Update()
    {
        //Vector3 currentAxisValue;
        currentAxisValue.x = NormalizeAxis(Input.GetAxis("Mouse X"));
        currentAxisValue.y = NormalizeAxis(Input.GetAxis("Mouse Y"));
        currentAxisValue.z = NormalizeAxis(Input.GetAxis("Mouse ScrollWheel"));

        //if (Input.GetAxis("Mouse ScrollWheel") != 0) Debug.Log("ScrollWheel " + Input.GetAxis("Mouse ScrollWheel"));

        currentAxisValue = currentAxisValue * Time.deltaTime;
        //currentDiff = currentAxisValue - lastAxisValue;

        if (resetButton != KeyCode.None && Input.GetKeyDown(resetButton))
        {
            totalRot = Vector3.zero;
            target.localPosition = Vector3.zero;
            target.localRotation = Quaternion.identity;
        }
        if (bindButton == KeyCode.None || Input.GetKey(bindButton))
        {

            if (moveButton == MouseButtons.None || Input.GetMouseButton((int)moveButton))
            {
                MoveUsingMouse();
            }
            if (rotationButton == MouseButtons.None || Input.GetMouseButton((int)rotationButton))
            {
                RotateUsingMouse();
            }
        }

        //lastAxisValue = currentAxisValue;
    }
}
