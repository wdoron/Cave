using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dweiss;

namespace Dweiss
{
    [ExecuteInEditMode]
    public class VersionNumber : MonoBehaviour
    {
        public UnityEngine.UI.Text uiText;
        public TextMesh textMesh;
        private void Reset()
        {
            uiText = this.GetComponentInHierarchy<UnityEngine.UI.Text>();
            textMesh = this.GetComponentInHierarchy<TextMesh>();

            SetVersion();
        }
        private void OnValidate()
        {
            if (uiText == null) uiText = this.GetComponentInHierarchy<UnityEngine.UI.Text>();
            if (textMesh == null) textMesh = this.GetComponentInHierarchy<TextMesh>();

            //if (Application.isEditor && Application.isPlaying == false)
            //    SetVersion();
        }

#if UNITY_EDITOR
        void Start()
        {
            if (Application.isEditor && Application.isPlaying == false)
                SetVersion();
        }

        //private void Update()
        //{
        //    if (Application.isEditor && Application.isPlaying == false)
        //        SetVersion();
        //}
#endif

        void SetVersion()
        {
            var version = Application.version;
            if ((textMesh != null && textMesh.text != version) ||
                (uiText != null && uiText.text != version))
            {
                Debug.Log("set version " + version);
                if (textMesh) textMesh.text = version;
                if (uiText) uiText.text = version;
#if UNITY_EDITOR
                if (textMesh) UnityEditor.EditorUtility.SetDirty(textMesh);
                if (uiText) UnityEditor.EditorUtility.SetDirty(uiText);
#endif
            }



        }
    }
}