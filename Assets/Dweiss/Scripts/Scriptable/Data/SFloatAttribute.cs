using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{

    [CreateAssetMenu(fileName = "SFloatAttribute", menuName = "Data/SFloatAttribute")]
    public class SFloatAttribute : ScriptableObject
    {
        [SerializeField]
        private float value;

        public Common.EventFloat onValueChanged;

        public float Value
        {
            get { return value; }
            set
            {
                if (value != this.value)
                {
                    this.value = value;
                    onValueChanged.Invoke(this.value);
                }
            }
        }
    }
}