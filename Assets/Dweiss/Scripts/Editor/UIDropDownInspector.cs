using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System.IO;
using System;
using System.Linq;
//using System.Collections.Generic;
namespace Dweiss.UI
{
    //[CustomEditor(typeof(UIDropDown))]
    //public class UIDropDownInspector : Editor
    //{

        
    //    int selected = 0;


    //    private bool IsNameSpaceValid(System.Type type)
    //    {
    //        var nameSpace = type.Namespace.StartsWith()
    //    }

    //    public override void OnInspectorGUI()
    //    {
    //        var script = ((UIDropDown)target);
    //        var allEnums = AppDomain.CurrentDomain.GetAssemblies()
    //                   .SelectMany(t => t.GetTypes())
    //                   .Where(t => t.IsEnum && IsNameSpaceValid(t));

    //        var serializedObject = new SerializedObject(target);
    //        DrawField(serializedObject, "decription");

    //        string[] options = allEnums.Select(a => a.ToString()).ToArray();
    //        selected = EditorGUILayout.Popup("Enum", selected, options);
    //        script.enumToUse = options[selected];
    //        //EditorGUILayout.DropdownButton()



    //    }

    //    private void DrawField(SerializedObject serializedObject, string name)
    //    {
    //        var property = serializedObject.FindProperty(name);
    //        serializedObject.Update();
    //        EditorGUILayout.PropertyField(property, true);
    //        serializedObject.ApplyModifiedProperties();
    //    }

    //}
}