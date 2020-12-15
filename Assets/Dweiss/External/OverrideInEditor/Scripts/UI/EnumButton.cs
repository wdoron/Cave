using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EnumButton : MonoBehaviour
{
    [SerializeField] private Button callBackButton;
    public string enumName;
    private System.Type bindedEnum;


    private Text text;

    private string[] enumNames;
    private object[] enumValues;

    private int enumIndex;

    public System.Action<object> onValueChange;

    public void ChangeValue(object enumVal)
    {
        for(int i=0; i < enumValues.Length; ++i)
        {
            if(enumValues[i] == enumVal)
            {
                enumIndex = i;
                text.text = enumNames[enumIndex];
                return;
            }
        }
        for (int i = 0; i < enumNames.Length; ++i)
        {
            if (enumNames[i] == enumVal.ToString())
            {
                enumIndex = i;
                text.text = enumNames[enumIndex];
                return;
            }
        }
        Debug.LogError("Enum value not supported " + enumVal + " for " + bindedEnum);
    }

    // Use this for initialization
    void Awake () {
        bindedEnum = Dweiss.ReflectionWrapper.ReflectionUtils.GetType(enumName);
        text = callBackButton.GetComponentInChildren<Text>();

        var values = System.Enum.GetValues(bindedEnum);
        enumValues = new object[values.Length];
        for (int i = 0; i < values.Length; ++i) enumValues[i] = values.GetValue(i);
        enumNames = new string[enumValues.Length];
        for(int i=0; i < enumValues.Length; ++i) enumNames[i] = System.Enum.GetName(bindedEnum, enumValues[i]);

        text.text = enumNames[enumIndex];
        callBackButton.onClick.AddListener(UserClick);

        //if (t.IsEnum)
        //{
        //    System.Array enumValues = System.Enum.GetValues(t);
        //    if (enumValues.Length > 0)
        //    {
        //        return enumValues.GetValue(0);
        //    }
        //    return null;
        //}
    }

    private void UserClick()
    {
        enumIndex = (enumIndex + 1) % enumNames.Length;
        text.text = enumNames[enumIndex];
        Debug.Log("User click " + enumNames[enumIndex]);
        if (onValueChange != null) onValueChange(enumValues[enumIndex]);
    }

   
}
