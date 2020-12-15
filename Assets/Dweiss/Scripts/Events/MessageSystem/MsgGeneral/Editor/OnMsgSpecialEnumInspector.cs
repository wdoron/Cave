using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dweiss.Msg
{
    [CustomEditor(typeof(OnMsgEnum))]
    public class OnMsgSpecialEnumInspector : Editor
    {
        private static MsgEnum GetMsgEnum()
        {
            var obj = GameObject.FindObjectOfType<MsgEnum>();
            if (obj == null)
            {
                var go = new GameObject("MessgeEnumOptions");
                obj = go.AddComponent<MsgEnum>();
            }
            return obj;
        }
        public static List<string> GetOptionList()
        {
            return GetMsgEnum().MsgOptions;
        }

        public static void SelectOptionList()
        {
            GetOptionList();
            var obj = GameObject.FindObjectOfType<MsgEnum>();
            UnityEditor.Selection.activeGameObject = obj.gameObject;
        }

        
        private bool showPosition = false;
        public override void OnInspectorGUI()
        {
            var script = ((OnMsgEnum)target);

            var serializedObject = new SerializedObject(target);

            if (script.msgId == null) script.msgId = new List<int>();
            EditorGUILayout.BeginHorizontal();
           // var obj = GameObject.FindObjectOfType<MsgEnum>();
            showPosition = EditorGUILayout.Foldout(showPosition, 
                "Msg Id (#" + script.msgId.Count  +  " - " +
                (script.msgId.Count>0 ? MsgEnum.GetNameById(script.msgId[0]) : "") + " ...) ");
            
            EditorGUILayout.EndHorizontal();

            if (showPosition)
            {
                EditorGUI.indentLevel++;
                //DrawField(serializedObject, script, "msgId");
                var allOptions = GetOptionList();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Size");
                var size = EditorGUILayout.IntField(script.msgId.Count);
                EditorGUILayout.EndHorizontal();

                while (size > script.msgId.Count)
                {
                    script.msgId.Add(script.msgId.Count>0? script.msgId[script.msgId.Count-1] : 0);
                }
                while (size < script.msgId.Count)
                {
                    script.msgId.RemoveAt(script.msgId.Count-1);
                }
                for (int i = 0; i < script.msgId.Count; ++i)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("#" + i);
                    script.msgId[i] = EditorGUILayout.Popup(script.msgId[i], allOptions.ToArray());

                    if (GUILayout.Button("+", GUILayout.MaxWidth(24)))
                    {
                        SelectOptionList();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }


            DrawField(serializedObject, script, "delay");
            DrawField(serializedObject, script, "runOnDisable");
            DrawField(serializedObject, script, "type");

            switch (script.type)
            {
                case OnMsgGeneric<int>.EventType.Bool:
                    DrawField(serializedObject, script, "onEventBool"); break;
                case OnMsgGeneric<int>.EventType.Int:
                    DrawField(serializedObject, script, "onEventInt"); break;
                case OnMsgGeneric<int>.EventType.Float:
                    DrawField(serializedObject, script, "onEventFloat"); break;
                case OnMsgGeneric<int>.EventType.String:
                    DrawField(serializedObject, script, "onEventString"); break;
                case OnMsgGeneric<int>.EventType.None:
                    DrawField(serializedObject, script, "onEventVoid"); break;
                case OnMsgGeneric<int>.EventType.Object:
                    DrawField(serializedObject, script, "onEventObject"); break;
                case OnMsgGeneric<int>.EventType.UnityObject:
                    DrawField(serializedObject, script, "onEventUnityObject"); break;
                case OnMsgGeneric<int>.EventType.Component:
                    DrawField(serializedObject, script, "onEventComponent"); break;
                case OnMsgGeneric<int>.EventType.GameObject:
                    DrawField(serializedObject, script, "onEventGameObject"); break;

                default: Debug.LogError(name + " Invoke Not supported " + script.type); break;
            }

            //DrawDefaultInspector();
            //EditorGUI.TextField(script.msgid);
            //UnityEngine.UI.


            //    [SerializeField] private T[] msgId;
            //[SerializeField] private float delay = -1;
            //[SerializeField] private bool runOnDisable = false;
        }

        private void DrawField<T>(SerializedObject serializedObject, OnMsgGeneric<T> script, string name)
        {
            var property = serializedObject.FindProperty(name);
            serializedObject.Update();
            EditorGUILayout.PropertyField(property, true);
            serializedObject.ApplyModifiedProperties();
        }

    }
    
}