using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class ConstantRotate : MonoExtension
    {
        [SerializeField]private float anglePerSec = 10;
        public Dweiss.EventFloat onFloatEvent;

        public bool raiseAbs = false;
        public float AnglePerSec { get { return anglePerSec; }
            set
            {
                
                if(anglePerSec != value)
                {
                   // Debug.Log("Value changed " + value + " compare " + anglePerSec);
                    anglePerSec = value;
                    onFloatEvent.Invoke(raiseAbs? Mathf.Abs(value) : value);
                }
            }
        }

        public void SetAbsValue(float newVal)
        {
            AnglePerSec = Mathf.Sign(anglePerSec) * Mathf.Abs(newVal);
        }

        public Vector3 localAxis;

        private void Awake()
        {
            onFloatEvent.Invoke(anglePerSec);
        }

        void LateUpdate()
        {
            
            t.Rotate(t.rotation*localAxis, anglePerSec * Time.deltaTime);
        }
    }
}