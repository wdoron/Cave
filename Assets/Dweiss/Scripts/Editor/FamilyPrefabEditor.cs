using System.Collections;
using System.Collections.Generic;

/*******************************************************
 * Copyright (C) 2017 Doron Weiss  - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of unity license.
 * 
 * See https://abnormalcreativity.wixsite.com/home for more info
 *******************************************************/
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Dweiss
{
    [CustomEditor(typeof(FamilyPrefab))]
    public class FamilyPrefabEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var script = ((FamilyPrefab)target);

            //EditorGUILayout.Separator();
            //EditorGUILayout.Separator();
            //EditorGUILayout.Separator();

            //EditorGUILayout.LabelField("_______Editor_______", EditorStyles.boldLabel);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUILayout.Button("Update prebab from scene"))
            {
                script.CreateReferenceFromScene();
                var prefab = (GameObject)PrefabUtility.GetPrefabParent(script.gameObject);
                var prefabFamily = prefab.GetComponent<FamilyPrefab>();
                var references = new List<FamilyPrefab.ReferenceGo>(script.referencedPrefabs);
                prefabFamily.referencedPrefabs = references;
                Debug.LogFormat("Updated prefab with new list " + references.Count);
            }

            //if (GUILayout.Button("Create references from scene"))
            //{
            //    script.CreateReferenceFromScene();
            //}
            //EditorGUILayout.Space();
            //if (GUILayout.Button("Update prefab"))
            //{
            //    var prefab = (GameObject)PrefabUtility.GetPrefabParent(script.gameObject);
            //    var prefabFamily = prefab.GetComponent<FamilyPrefab>();
            //    var references = new List<FamilyPrefab.ReferenceGo>(script.referencedPrefabs);
            //    prefabFamily.referencedPrefabs = references;
            //    Debug.LogFormat("Updated prefab with new list " + references.Count);
            //}

            //EditorGUILayout.Space();
            EditorGUILayout.Space();
            if (GUILayout.Button("Reset children"))
            {
                script.ResetFromReference(true);
            }


        }

    }
}