using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class PrimitiveEventsToStringEvent : MonoBehaviour
    {
        public string textFormat = "{0}";
        public string toStringFormat = "";
        public Dweiss.EventString onString;

        public void OnValue(float v) { onString.Invoke(string.Format(textFormat,v.ToString(toStringFormat))); }
        public void OnValue(bool v) { onString.Invoke(string.Format(textFormat, v.ToString())); }
        public void OnValue(int v) { onString.Invoke(string.Format(textFormat, v.ToString(toStringFormat))); }
        public void OnValue(double v) { onString.Invoke(string.Format(textFormat, v.ToString(toStringFormat))); }
        public void OnValue(byte v) { onString.Invoke(string.Format(textFormat, v.ToString(toStringFormat))); }
    }
}