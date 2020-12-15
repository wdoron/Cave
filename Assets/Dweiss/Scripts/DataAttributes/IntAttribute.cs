using UnityEngine;

namespace Dweiss.Common
{
    [System.Serializable]
    public struct IntAttribute
    {
        [SerializeField]
        private int value;

        public EventInt onValueChanged;

        public void Invoke()
        {
            onValueChanged.Invoke(this.value);
        }

        public void Increament()
        {
            Value = value + 1;
        }
        public void Decreament()
        {
            Value = value - 1;
        }
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