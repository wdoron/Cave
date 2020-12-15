using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dweiss.Msg
{
    [CustomEditor(typeof(RaiseMsgEnum))]
    public class RaiseMsgEnumSpecialEnumInspector : Editor
    {

        public static List<string> GetOptionList()
        {
            var obj = GameObject.FindObjectOfType<MsgEnum>();
            if (obj == null)
            {
                var go = new GameObject("MessgeEnumOptions");
                obj = go.AddComponent<MsgEnum>();
            }
            return obj.MsgOptions;
        }

        public static void SelectOptionList()
        {
            GetOptionList();
            var obj = GameObject.FindObjectOfType<MsgEnum>();
            UnityEditor.Selection.activeGameObject = obj.gameObject;
        }

        public override void OnInspectorGUI()
        {
            var script = ((RaiseMsgEnum)target);

            var serializedObject = new SerializedObject(target);

            var allOptions = GetOptionList();
            if (allOptions == null) allOptions = new List<string>();
            EditorGUILayout.BeginHorizontal();
            //showPosition = EditorGUILayout.Foldout(showPosition, "Msg Id (" + script.msgId.Count + ")");
            script.defaultId = EditorGUILayout.Popup(script.defaultId, allOptions.ToArray());
            if (GUILayout.Button("+", GUILayout.MaxWidth(24)))
            {
                SelectOptionList();
            }
            EditorGUILayout.EndHorizontal();

            DrawField(serializedObject, "delay");
        }

        private void DrawField(SerializedObject serializedObject, string name)
        {
            var property = serializedObject.FindProperty(name);
            serializedObject.Update();
            EditorGUILayout.PropertyField(property, true);
            serializedObject.ApplyModifiedProperties();
        }

    }
}