using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEditor;
using System.IO;

namespace Dweiss.Msg
{
    [CustomEditor(typeof(Dweiss.Msg.OnMsgString))]
    public class OnMsgStringInspector : OnMsgGenericInspector<string> { }

    //[CustomEditor(typeof(Dweiss.Msg.OnMsgInt))]
    //public class OnMsgIntInspector : OnMsgGenericInspector<int> { }

    //[CustomEditor(typeof(Dweiss.Msg.Test.OnMsgEnum))]
    //public class OnMsgEnumInspector : OnMsgGenericInspector<Dweiss.Msg.Test.MsgEnum> { }

    //[CustomEditor(typeof(OnMsgString))]
    public class OnMsgGenericInspector<T> : Editor
    {
        public override void OnInspectorGUI()
        {
            var script = ((OnMsgGeneric<T>)target);

            var serializedObject = new SerializedObject(target);
            DrawField(serializedObject, script, "msgId");
            DrawField(serializedObject, script, "delay");
            DrawField(serializedObject, script, "runOnDisable");
            DrawField(serializedObject, script, "type");

            switch (script.type)
            {
                case OnMsgGeneric<T>.EventType.Bool: DrawField(serializedObject, script, "onEventBool"); break;
                case OnMsgGeneric<T>.EventType.Int:
                    DrawField(serializedObject, script, "onEventInt"); break;
                case OnMsgGeneric<T>.EventType.Float:
                    DrawField(serializedObject, script, "onEventFloat"); break;
                case OnMsgGeneric<T>.EventType.String:
                    DrawField(serializedObject, script, "onEventString"); break;
                case OnMsgGeneric<T>.EventType.None:
                    DrawField(serializedObject, script, "onEventVoid"); break;
                case OnMsgGeneric<T>.EventType.Object:
                    DrawField(serializedObject, script, "onEventObject"); break;
                case OnMsgGeneric<T>.EventType.UnityObject:
                    DrawField(serializedObject, script, "onEventUnityObject"); break;
                case OnMsgGeneric<T>.EventType.Component:
                    DrawField(serializedObject, script, "onEventComponent"); break;
                case OnMsgGeneric<T>.EventType.GameObject:
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

        private void DrawField(SerializedObject serializedObject, OnMsgGeneric<T> script, string name)
        {
            var property = serializedObject.FindProperty(name);
            serializedObject.Update();
            EditorGUILayout.PropertyField(property, true);
            serializedObject.ApplyModifiedProperties();
        }

    }
}