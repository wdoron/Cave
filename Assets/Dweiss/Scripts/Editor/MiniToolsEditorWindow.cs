using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Dweiss
{
    public class MiniToolsEditorWindow : EditorWindow
    {

        // Add menu named "My Window" to the Window menu
        [MenuItem("Dweiss/Mini Tools")]
        static void Init()
        {

            // Get existing open window or if none, make a new one:
            var window = (MiniToolsEditorWindow)EditorWindow.GetWindow(typeof(MiniToolsEditorWindow));
            window.Show();


        }

        private ITool[] _tools;
        private ITool[] Tools
        {
            get
            {
                if (_tools == null) _tools = new ITool[] {
                    new PivotTool(), new CopyScaleTool(), new OnSelect()
                , new SelectByTag(), new OrderInHierarchy(), new FastSelectFromAssets() };
                return _tools;
            }
        }

        private void Update()
        {
            Repaint();
        }

        public void OnGUI()
        {
            for (int i = 0; i < Tools.Length; ++i)
            {
                try
                {
                    Tools[i].OnGUI();
                    EditorGUILayout.Space();
                }catch(System.Exception e)
                {
                    Debug.LogError("Error " + i + ": " + e);
                }
            }
        }

        interface ITool
        {
            void OnGUI();
        }


        class PivotTool : ITool
        {
            Vector3 axis = Vector3.one;
            Vector3 axisShift = Vector3.zero;

            // Use this for initialization
            public void OnGUI()
            {
                GUILayout.Label("Create Pivot Parent", EditorStyles.boldLabel);

                GUILayout.Label("Axis to consider");
                GUILayout.BeginHorizontal();

                GUILayout.Label("(X,Y,Z)", GUILayout.Width(50));
                axis.x = EditorGUILayout.Toggle(axis.x == 1, GUILayout.Width(10)) ? 1 : 0;
                axis.y = EditorGUILayout.Toggle(axis.y == 1, GUILayout.Width(10)) ? 1 : 0;
                axis.z = EditorGUILayout.Toggle(axis.z == 1, GUILayout.Width(10)) ? 1 : 0;
                GUILayout.EndHorizontal();

                axisShift = EditorGUILayout.Vector3Field("Add shift by extends", axisShift);

                if (GUILayout.Button("Add Parent Pivot") && Selection.gameObjects.Length > 0)
                {
                    for (int i = 0; i < Selection.gameObjects.Length; ++i)
                    {
                        var go = Selection.gameObjects[i];
                        var newParentGo = new GameObject(go.name + "_pivot");
                        var meshBounds = go.TotalMeshBounds();
                        var center = meshBounds.center.PointMul(axis) +
                            go.transform.position.PointMul(Vector3.one - axis) +
                            axisShift.PointMul(meshBounds.extents);
                        newParentGo.transform.SetPositionAndRotation(center, go.transform.rotation);
                        var oldParent = go.transform.parent;
                        go.transform.parent = newParentGo.transform;
                        newParentGo.transform.parent = oldParent;
                    }
                }

            }
        }


        class CopyScaleTool : ITool
        {
            bool to = false;

            Bounds GetRealBounds(GameObject go)
            {
                var bounds = go.TotalColliderBounds();
                if (bounds.size.sqrMagnitude == 0)
                    bounds = go.TotalMeshBounds();
                return bounds;
            }

            // Use this for initialization
            public void OnGUI()
            {
                GUILayout.Label("Copy scale", EditorStyles.boldLabel);

                // if(Selection.gameObjects.Length == 2)
                {
                    if (GUILayout.Button("Switch Targets"))
                    {
                        to = !to;
                    }
                    //to = EditorGUILayout.Toggle("Switch Targets", to);
                    EditorGUI.BeginDisabledGroup(true);

                    var first = Selection.gameObjects.Length > 0 ? Selection.gameObjects[Selection.gameObjects.Length - 1] : null;
                    var sec = Selection.gameObjects.Length > 1 ? Selection.gameObjects[Selection.gameObjects.Length - 2] : null;

                    var fromGo = to ? first : sec;
                    var toGo = to ? sec : first;

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("From ");
                    EditorGUILayout.ObjectField(fromGo, typeof(GameObject));
                    GUILayout.Label(" To ");
                    EditorGUILayout.ObjectField(toGo, typeof(GameObject));
                    EditorGUILayout.EndHorizontal();

                    EditorGUI.EndDisabledGroup();

                    if (GUILayout.Button("Scale") && Selection.gameObjects.Length == 2)
                    {
                        var totalScaleFrom = GetRealBounds(fromGo);
                        var totalScaleTo = GetRealBounds(toGo);

                        var factor = (totalScaleTo.size).PointDiv(totalScaleFrom.size);

                        fromGo.transform.localScale = fromGo.transform.localScale.PointMul(factor);

                        Undo.RegisterCompleteObjectUndo(fromGo.transform, "Scaling");
                    }
                }

            }
        }

        class OnSelect : ITool
        {
            string prefabLocation = "";
            string suffix = ".prefab";
            public void OnGUI()
            {

                GUILayout.Label("Select " + suffix + " from Assets Folder");
                prefabLocation = EditorGUILayout.TextField("Location ", prefabLocation);
                suffix = EditorGUILayout.TextField("Suffix ", suffix);


                var assetIndex = prefabLocation.ToLower().IndexOf("assets");
                if (assetIndex < 0)
                {
                    prefabLocation = Path.Combine("Assets/", prefabLocation);
                }
                else
                {
                    prefabLocation = prefabLocation.Substring(assetIndex);
                }

                if (GUILayout.Button("Select"))
                {

                    var allPrefabs = new List<Object>();
                    List<string> files = new List<string>();
                    GetAllFilesUnderTree(prefabLocation, files);
                    foreach (var f in files)
                    {
                        if (f.EndsWith(suffix))
                        {
                            var prefab = AssetDatabase.LoadMainAssetAtPath(f);
                            allPrefabs.Add(prefab);
                        }

                    }

                    Debug.Log("Set " + allPrefabs.ToCommaString());
                    Selection.objects = allPrefabs.ToArray();
                }

            }
            private void GetAllFilesUnderTree(string folder, List<string> files)
            {
                files.AddRange(Directory.GetFiles(folder, "*", SearchOption.TopDirectoryOnly));//This is recursive
                var allDirs = System.IO.Directory.GetDirectories(folder);
                for (int i = 0; i < allDirs.Length; ++i)
                {
                    GetAllFilesUnderTree(allDirs[i], files);
                }
            }
        }

        class SelectByTag : ITool
        {
            private string tagToSearch = "";
            public void OnGUI()
            {
                tagToSearch = EditorGUILayout.TextField("Tag ", tagToSearch);

                if (GUILayout.Button("Select from tag"))
                {
                    Activate();
                }
            }
            private void Activate()
            {
                Selection.activeGameObject = GameObject.FindGameObjectWithTag(tagToSearch);
            }
        }

        class OrderInHierarchy : ITool
        {
            public void OnGUI()
            {

                GUILayout.Label("Sort childrens");

                if (GUILayout.Button("Sort childrens in selected"))
                {
                    var go = Selection.activeGameObject;
                    var childrens = new List<Transform>();
                    for (int i = 0; i < go.transform.childCount; ++i)
                        childrens.Add(go.transform.GetChild(i));

                    childrens.Sort((a, b) => string.Compare(a.name, b.name));
                    go.transform.DetachChildren();
                    for (int i = 0; i < childrens.Count; ++i)
                    {
                        childrens[i].SetParent(go.transform);
                    }
                }
            }
        }

        class FastSelectFromAssets : ITool
        {
            string prefabLocation;
            string suffix = ".prefab";
            int maxSelectCount = 100;

            int fileCount = 0;
            int selectedCount = 0;
            public void OnGUI()
            {

                GUILayout.Label("Select " + suffix + " from Assets Folder");
                prefabLocation = EditorGUILayout.TextField("Location ", prefabLocation);
                suffix = EditorGUILayout.TextField("Suffix ", suffix).ToLower();
                maxSelectCount = EditorGUILayout.IntField("Max select count ", maxSelectCount);
                EditorGUILayout.LabelField("File count " + fileCount + " selected " + selectedCount);
                if (GUILayout.Button("Select"))
                {
                    var assetIndex = prefabLocation.ToLower().IndexOf("assets");
                    if (assetIndex < 0)
                    {
                        prefabLocation = Path.Combine("Assets/", prefabLocation);
                    }
                    else
                    {
                        prefabLocation = prefabLocation.Substring(assetIndex);
                    }
                    var allPrefabs = new List<Object>();
                    List<string> files = new List<string>();
                    GetAllFilesUnderTree(prefabLocation, files);

                    //fileCount = files.Count;
                    selectedCount = 0;
                    foreach (var f in files)
                    {

                        var fileNameToCompare = f.ToLower();
                        if (fileNameToCompare.EndsWith(suffix) && fileNameToCompare.EndsWith(".meta") == false)
                        {
                            fileCount++;
                            if (selectedCount < maxSelectCount)
                            {
                                selectedCount++;
                                var asset = AssetDatabase.LoadMainAssetAtPath(f);
                                if (asset == null) Debug.LogError("Select <" + asset + "> at " + f);
                                else allPrefabs.Add(asset);
                            }
                        }



                    }

                    Debug.Log("Set " + allPrefabs.Count + " " + (allPrefabs.Count > 0 ? allPrefabs[0].ToString() + "..." : ""));
                    Selection.objects = allPrefabs.ToArray();
                }

            }
            private void GetAllFilesUnderTree(string folder, List<string> files)
            {
                files.AddRange(Directory.GetFiles(folder, "*", SearchOption.TopDirectoryOnly));
                var allDirs = System.IO.Directory.GetDirectories(folder);
                for (int i = 0; i < allDirs.Length; ++i)
                {
                    GetAllFilesUnderTree(allDirs[i], files);
                }
            }
        }


    }
}