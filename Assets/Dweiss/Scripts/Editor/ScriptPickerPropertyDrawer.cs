using UnityEngine;
using UnityEditor;
[CustomPropertyDrawer(typeof(ScriptPickerAttribute))]
public class ScriptPickerPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        Rect rect;

        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        int objectFieldWidth = 45;

        // Detect drag & drop
        CheckDragAndDrop(position, prop, controlID);

        // Allow manual text entry
        rect = new Rect(
            position.x,
            position.y,
            position.width - objectFieldWidth - 2,
            position.height
        );
        prop.stringValue = EditorGUI.TextField(
            rect, label, prop.stringValue
        );

        // Allow drag & drop or selection from dialog
        rect = new Rect(
            position.xMax - objectFieldWidth,
            position.y,
            objectFieldWidth,
            position.height
        );
        MonoScript script = (MonoScript)EditorGUI.ObjectField(
            rect, null, typeof(MonoScript), false
        );
        if (script != null)
        {
            prop.stringValue = script.name;
        }
    }

    private void CheckDragAndDrop(Rect position, SerializedProperty prop, int controlID)
    {
        Event e = Event.current;
        EventType eventType = e.GetTypeForControl(controlID);
        switch (eventType)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (position.Contains(e.mousePosition)
                    && DragAndDrop.objectReferences.Length == 1
                    && DragAndDrop.objectReferences[0] is MonoScript)
                {
                    if (eventType == EventType.DragUpdated)
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                        DragAndDrop.activeControlID = controlID;
                    }
                    else
                    {
                        prop.stringValue = DragAndDrop.objectReferences[0].name;
                        DragAndDrop.AcceptDrag();
                        DragAndDrop.visualMode = DragAndDropVisualMode.None;
                    }
                    e.Use();
                }
                break;
        }
    }
}