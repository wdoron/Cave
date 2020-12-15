using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{

    [CreateAssetMenu(fileName = "SIntAttribute", menuName = "Data/SIntAttribute")]
    public class SIntAttribute : ScriptableObject
    {
        [SerializeField]
        private int value;

        public Common.EventInt onValueChanged;

        public int Value
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