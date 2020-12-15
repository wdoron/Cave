using UnityEngine;
using UnityEditor;
using System.Collections;
using Dweiss;

namespace Common
{
    public class ScaleCabinet : EditorWindow
    {

        private float woodWidth = .05f;

        [MenuItem("Dweiss/EditorWindow")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(ScaleCabinet));

        }


        private void FixSize()
        {
            var active = Selection.activeGameObject;
            //Debug.Log("Active " + active);
            foreach (var trns in active.GetComponentsInChildren<Transform>())
            {

                var mesh = trns.GetComponent<MeshRenderer>();
                if (mesh == null) continue;
                var mPreSize = mesh.bounds.size;
                var mSize = mesh.bounds.size.PointMul(trns.localScale);

                //var origRot = trns.rotation;
                //var origSize = trns.localScale;

                //trns.rotation = Quaternion.identity;
                //Repaint();


                //var min = mSize.x;
                var newScale = trns.localRotation * trns.localScale;

                //char scaleLetter;
                if (mSize.x < mSize.y && mSize.x < mSize.z)
                {
                    newScale.x *= (woodWidth / mSize.x);
                    //scaleLetter = 'x';
                    //Debug.Log(trns.name + ": " + scaleLetter + " : " + newScale.x);
                }
                else if (mSize.y < mSize.x && mSize.y < mSize.x)
                {
                    newScale.y *= (woodWidth / mSize.y);
                    //scaleLetter = 'y';
                    //Debug.Log(trns.name + ": " + scaleLetter + " : " + newScale.y);
                }
                else
                {
                    newScale.z *= (woodWidth / mSize.z);
                    //scaleLetter = 'z';
                    //Debug.Log(trns.name + ": " + scaleLetter + " : " + newScale.z);
                }
                trns.localScale = Quaternion.Inverse( trns.localRotation) * newScale;
                //var mSizeAfter = mesh.bounds.size.PointMul(trns.localScale);

                //trns.rotation = origRot;

                //Debug.Log(trns.name + ": " + scaleLetter + " : " +
                //    origSize.ToMiliString() + " > " + newScale.ToMiliString() + " --- " +
                //    mSize.ToMiliString() + " > " + mSizeAfter.ToMiliString() + " --- " +
                //    mPreSize.ToMiliString() + " > " + mesh.bounds.size.ToMiliString());

            }
        }
        void OnGUI()
        {
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);


            woodWidth = EditorGUILayout.Slider("Slider", woodWidth, 0f, .3f);

            if (GUILayout.Button("Set width"))
            {
                FixSize();
            }

            //var sldir = EditorGUILayout.Slider("Slider", _scale, -3, 3);

            //myString = EditorGUILayout.TextField("Text Field", myString);

            //groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
            //myBool = EditorGUILayout.Toggle("Toggle", myBool);
            //myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
            //EditorGUILayout.EndToggleGroup();
        }

    }
}