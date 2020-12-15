using UnityEngine;
using System.Collections;

namespace Dweiss
{
    [RequireComponent(typeof(TextMesh))]
    public class Fps : MonoBehaviour
    {
        public bool hideInBuild;

        public bool onGuiDraw;
        public bool debugLog;

        public TextMesh tMesh;
       

        float deltaTime = 0.0f;
        int deltaTimeCount = 0;


        private void Start()
        {
#if UNITY_EDITOR == false
            if (hideInBuild)
            {
                Destroy(gameObject);
            }
#endif
        }

        void Update()
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            deltaTimeCount++;

            if (tMesh != null)
                tMesh.text = "" + FpsCount + "FPS";

            if (debugLog) Debug.Log("FPS: " + FpsCount + " dlta" + Time.deltaTime + " t " + Time.time);
        }


        private void Reset()
        {
            tMesh = GetComponent<TextMesh>();
        }

        public int FpsCount
        {
            get { return (int)(deltaTimeCount / Time.time); }
        }



        void OnGUI()
        {
            if (onGuiDraw == false) return;

            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();

            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 50;
            style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
            //float fps = Time.time / deltaTimeCount;
            float msec = deltaTime * 1000.0f;

            Rect rect = new Rect(w * guiOnScreenPosFactor.x, h * guiOnScreenPosFactor.y, w, h);
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, FpsCount);
            GUI.Label(rect, text, style);
        }

        public Vector2 guiOnScreenPosFactor = new Vector2(.5f, .03f);
    }
}