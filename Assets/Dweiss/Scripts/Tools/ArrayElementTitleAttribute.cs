using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class ArrayElementTitleAttribute : PropertyAttribute
    {
        public string Varname;
        public ArrayElementTitleAttribute(string ElementTitleVar)
        {
            Varname = ElementTitleVar;
        }
    }
}
