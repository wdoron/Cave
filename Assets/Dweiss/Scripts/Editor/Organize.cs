using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Dweiss;


namespace Common
{
    public class Organize : EditorWindow
    {

        //private float woodWidth = .05f;

        [MenuItem("Dweiss/Organize")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(Organize));

        }

        List<GameObject> childrens = new List<GameObject>();
        void Create()
        {
            for(int i=0; i < childrens.Count; ++i) {
                DestroyImmediate(childrens[i]);
            }
            childrens.Clear();

            Bounds bounds = new Bounds();
            var rndrs = parent.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < rndrs.Length; ++i)
            {
                if (i == 0) bounds = new Bounds(rndrs[i].bounds.center, rndrs[i].bounds.size) ;
                else bounds.Encapsulate(rndrs[i].bounds);
            }
            var distBetweenBlocks = bounds.size.PointDiv(numOfBlocks);
            var beginShift = distBetweenBlocks;
            if (beginShift.x == 1) beginShift.x = 0;
            if (beginShift.y == 1) beginShift.y = 0;
            if (beginShift.z == 1) beginShift.z = 0;
            for (int x=0; x < numOfBlocks.x; ++x)
            {
                for (int y = 0; y < numOfBlocks.y; ++y)
                {
                    for (int z = 0; z < numOfBlocks.z; ++z)
                    {
                        GameObject child = Instantiate(childBlock, parent.transform, true);
                        child.transform.position =  
                            bounds.center + bounds.extents - distBetweenBlocks.PointMul(x, y, z) 
                            - .5f * beginShift
                            + absoluteShift;
                        childrens.Add(child);
                    }
                }
            }
        }

        GameObject parent;
        GameObject childBlock;
        Vector3 numOfBlocks = new Vector3(1,3,1);
        Vector3 absoluteShift = new Vector3(0, 0, 0);

        void OnGUI()
        {
            GUILayout.Label("Configuration", EditorStyles.boldLabel);

            numOfBlocks = EditorGUILayout.Vector3Field("Number of blocks", numOfBlocks);
            absoluteShift = EditorGUILayout.Vector3Field("Absolute shift", absoluteShift);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Parent");
            parent = (GameObject) EditorGUILayout.ObjectField(parent, typeof(GameObject), true);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("child");
            childBlock = (GameObject)EditorGUILayout.ObjectField(childBlock, typeof(GameObject), true);
            GUILayout.EndHorizontal();
            //woodWidth = EditorGUILayout.Slider("Slider", woodWidth, 0f, .3f);

            if (GUILayout.Button("Create"))
            {
                Create();
            }

        }

    }
}