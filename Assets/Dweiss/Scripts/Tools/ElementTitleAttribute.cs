using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementTitleAttribute : PropertyAttribute
{

    public string Varname;
    public ElementTitleAttribute(string ElementTitleVar)
    {
        Varname = ElementTitleVar;
    }
}