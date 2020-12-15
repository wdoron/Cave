

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

 [InitializeOnLoad]
class RunInEditorHelpSetup
{
    //static System.DateTime executeTime;
    static RunInEditorHelpSetup()
    {
        UnityEditor.Undo.willFlushUndoRecord += OnWillFlushUndoRecord;
        //EditorApplication.hierarchyChanged += OnHierarchyChanged;
       // EditorApplication.update += Update;

        //EditorApplication.projectChanged += ProjectChanged;
        // Application.change.scene
    }

    public RunInEditorHelpSetup()
    { // regular constructor where we add this to static list
    }

    //static void OnHierarchyChanged()
    //{
    //    Debug.Log("OnHierarchyChanged ");
    //    ExecuteEditorUpdate();
    //}
    static void OnWillFlushUndoRecord()
    {
        //Debug.Log("OnWillFlushUndoRecord ");
        ExecuteEditorUpdate();
    }
    static void ExecuteEditorUpdate()
    {
        //if (EditorSceneManager.GetActiveScene().isDirty == false) return;
       // EditorSceneManager.
        
        if (Selection.activeGameObject)
        {
            
            for(int i=0; i < Selection.gameObjects.Length; ++i)
            {
                var go = Selection.gameObjects[i];
                //Debug.Log("Raise event UpdateOnlyInEditor " + go);
                if (go.activeSelf)
                {
                    try
                    {
                        go.SendMessage("UpdateOnlyInEditor", SendMessageOptions.DontRequireReceiver);
                    }catch(System.Exception e)
                    {
                        Debug.LogWarning("Failed running in editor " + e);
                    }
                    EditorUtility.SetDirty(go);
                }
            }
        }
    }

    

}
