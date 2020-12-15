using UnityEngine;

namespace Dweiss.Common
{
    [System.Serializable]
    public struct FloatAttribute
    {
        [SerializeField]
        private float value;

        public EventFloat onValueChanged;

        public void Invoke()
        {
            onValueChanged.Invoke(this.value);

        }
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