using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class ViveTrackPadToMovement : MonoBehaviour
    {
        public enum InputType : int
        {
            HTC_VIU_LeftTrackpadHorizontal,
            HTC_VIU_LeftTrackpadVertical,
            HTC_VIU_LeftTrigger,
            HTC_VIU_RightTrackpadHorizontal,
            HTC_VIU_RightTrackpadVertical,
            HTC_VIU_RightTrigger
        }

        public enum ObjectAxis
        {
            RotX, RotY, RotZ
                ,MovX, MovY, MovZ
        }

        [SerializeField]private InputType inputAxis;
        [SerializeField] private ObjectAxis objectAxis;


        private string[] inputAxisName =
        {
            "HTC_VIU_LeftTrackpadHorizontal",
            "HTC_VIU_LeftTrackpadVertical",
            "HTC_VIU_LeftTrigger",
            "HTC_VIU_RightTrackpadHorizontal",
            "HTC_VIU_RightTrackpadVertical",
            "HTC_VIU_RightTrigger"
        };

        private string InputAxis { get { return inputAxisName[(int)inputAxis]; } }

        private float AxisValue { get { return Input.GetAxis(InputAxis); } }

        public float rangeValue = 1, shiftInputValue =.5f;
        //private Quaternion lastRot = Quaternion.identity;
        private void SetToAxis(float val)
        {
            switch (objectAxis)
            {
                case ObjectAxis.RotX: SetLocalRotation(Vector3.right, val); break;
                case ObjectAxis.RotY: SetLocalRotation(Vector3.up, val); break;
                case ObjectAxis.RotZ: SetLocalRotation(Vector3.forward, val); break;

                case ObjectAxis.MovX: transform.localPosition = new Vector3((val - shiftInputValue) * rangeValue, transform.localPosition.y, transform.localPosition.z); break;
                case ObjectAxis.MovY: transform.localPosition = new Vector3(transform.localPosition.x, (val - shiftInputValue) * rangeValue, transform.localPosition.z); break;
                case ObjectAxis.MovZ: transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, (val - shiftInputValue) * rangeValue); break;
                default: throw new System.InvalidOperationException("Axis "  + objectAxis + " to object not supported from input joystick");
            }
        }
        
        private void SetLocalRotation(Vector3 axis, float val)
        {
            var newRot = Quaternion.AngleAxis((val - shiftInputValue) * rangeValue, axis);
            transform.localRotation = newRot * Quaternion.identity;
        }

        void Update()
        {
            SetToAxis(AxisValue);
        }
    }
}