/*****************************************************************************
 * Copyright (C) 2017 Doron Weiss  - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of unity3d license https://unity3d.com/legal/as_terms.
 * 
 * See https://abnormalcreativity.wixsite.com/home for more info
 ******************************************************************************/
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Dweiss
{
    [CustomEditor(typeof(RunOnKey))]
    public class RunOnKeyInspector : Editor
    {
        private string CanvasName = "EditorAutoCreate_dontRename";

        private string GetCanvasFullName(RunOnKey script)
        {
            return CanvasName;// script.name + "_" + CanvasName;
        }

        private GameObject canvas;
        private void TrySetupCanvas(RunOnKey script)
        {
            if (canvas == null)
            {
                var CanvasScript = GameObject.FindObjectsOfType<Canvas>().FirstOrDefault(a => a.name == GetCanvasFullName(script));
                canvas = CanvasScript == null ? null : CanvasScript.gameObject;
            }
        }
        public override void OnInspectorGUI()
        {
            var script = ((RunOnKey)target);
            //if (GUILayout.Button("Refresh"))
            //{
            //    script.RefreshItems();
            //}
            try
            {
              
                DrawDefaultInspector();
            }
            catch (System.Exception e) { Debug.LogError( e +" Changeing reference script or class/enum dependencies after setting it is not allowed. Please recreate references"); }
            if (GUILayout.Button("Update Auto Canvas"))
            {
                BindToCanvas(script);
            }
        }

        private void BindToCanvas(RunOnKey script)
        {
            var toDestroy = new List<GameObject>();
            //GameObject Canvas;
            TrySetupCanvas(script);
            if (canvas == null)
            {
                var CanvasPrefab = Resources.Load<GameObject>("Canvas");
                if(GameObject.FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
                {
                    var eventSys = new GameObject("EventSystem");
                    eventSys.AddComponent<UnityEngine.EventSystems.EventSystem>();
                    eventSys.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
                }

                canvas = Instantiate(CanvasPrefab);
            }

            else
            {
                foreach (Transform t in canvas.transform)
                {
                   // Debug.Log(t + " enable " + t.gameObject.activeSelf);
                    if (t.GetComponent<CanvasHelper>() == null)
                    {
                        toDestroy.Add(t.gameObject);
                    }
                }
            }

            canvas.name = GetCanvasFullName(script);

            int i = 0;
            int buttonsCount = 0;

            foreach (var s in GameObject.FindObjectsOfType<RunOnKey>())
            {
                foreach (var item in s.itemsToChangeOnUpdate)
                {
                    if (item.IsValid() == false)
                    {
                        Debug.LogError("Item " + item + " is not valid. Skip");
                        continue;
                    }
                    item.Setup();
                    Debug.Log(i + " Item " + item + "\n" + item.MemeberInfo);

                    GameObject bGo = null;
                    switch (item.MemeberInfo.MemberT)
                    {
                        case System.Reflection.MemberTypes.Method:
                            var mType = item.GetMethodParamTypes();
                            if (mType.Length == 1 && (mType[0] == typeof(int) || mType[0] == typeof(float)))
                            {
                                var bPrefab = Resources.Load<GameObject>("ButtonNumber");
                                bGo = Instantiate(bPrefab);

                                bGo.name = item.ToString();
                                bGo.transform.SetParent(canvas.transform, true);
                                var rt = bGo.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector2(rt.sizeDelta.x / 2 * 1.1f, -rt.sizeDelta.y * (.5f * 1.1f + 1) - rt.sizeDelta.y * (buttonsCount++));

                                item.Button = (new ItemsToChange.UiButton()
                                {
                                    go = bGo,

                                    bType =
                                    mType[0] == typeof(int) ? ItemsToChange.UiButtonType.NumberFuncInt : ItemsToChange.UiButtonType.NumberFuncFloat
                                });

                                bGo.name = item.MemeberInfo.Name;

                            }
                            else if (mType.Length == 1 && mType[0] == typeof(bool))
                            {
                                var bPrefab = Resources.Load<GameObject>("ButtonBool");
                                bGo = Instantiate(bPrefab);

                                bGo.name = item.ToString();
                                bGo.transform.SetParent(canvas.transform, true);
                                var rt = bGo.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector2(rt.sizeDelta.x / 2 * 1.1f, -rt.sizeDelta.y * (.5f * 1.1f + 1) - rt.sizeDelta.y * (buttonsCount++));
                                item.Button = (new ItemsToChange.UiButton() { go = bGo, bType = ItemsToChange.UiButtonType.BoolFunc });

                                bGo.name = item.MemeberInfo.Name;

                            }
                            else if (mType.Length == 1 && mType[0] == typeof(string))
                            {
                                var bPrefab = Resources.Load<GameObject>("ButtonString");
                                bGo = Instantiate(bPrefab);

                                bGo.name = item.ToString();
                                bGo.transform.SetParent(canvas.transform, true);
                                var rt = bGo.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector2(rt.sizeDelta.x / 2 * 1.1f, -rt.sizeDelta.y * (.5f * 1.1f + 1) - rt.sizeDelta.y * (buttonsCount++));
                                item.Button = (new ItemsToChange.UiButton() { go = bGo, bType = ItemsToChange.UiButtonType.StringFunc });

                                bGo.name = item.MemeberInfo.Name;

                            }
                            else if (mType.Length == 1 && mType[0].IsEnum)
                            {
                                var bPrefab = Resources.Load<GameObject>("EnumButton");
                                bGo = Instantiate(bPrefab);

                                bGo.name = item.ToString();
                                bGo.transform.SetParent(canvas.transform, true);
                                var rt = bGo.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector2(rt.sizeDelta.x / 2 * 1.1f, -rt.sizeDelta.y * (.5f * 1.1f + 1) - rt.sizeDelta.y * (buttonsCount++));
                                item.Button = (new ItemsToChange.UiButton() { go = bGo, bType = ItemsToChange.UiButtonType.EnumFunc });

                                bGo.name = item.MemeberInfo.Name;

                                var eb = bGo.GetComponent<EnumButton>();
                                eb.enumName = mType[0].FullName;
                            }
                            else if (mType.Length == 0)
                            {
                                var bPrefab = Resources.Load<GameObject>("Button");
                                bGo = Instantiate(bPrefab);

                                bGo.name = item.ToString();
                                bGo.transform.SetParent(canvas.transform, true);
                                var rt = bGo.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector2(rt.sizeDelta.x / 2 * 1.1f, -rt.sizeDelta.y * (.5f * 1.1f + 1) - rt.sizeDelta.y * (buttonsCount++));

                                item.Button = (new ItemsToChange.UiButton() { go = bGo, bType = ItemsToChange.UiButtonType.Button });

                                var txt = bGo.GetComponentInChildren<Text>();
                                txt.text = item.MemeberInfo.Name;

                                bGo.name = item.MemeberInfo.Name;
                            }
                            else
                            {
                                Debug.LogWarning("Item " + item + " not supported " + item.MemeberInfo.MemberT);
                                continue;
                            }
                            break;

                        case System.Reflection.MemberTypes.Field:
                            var type = item.GetValueType();
                            if (type == typeof(int) || type == typeof(float))
                            {
                                var bPrefab = Resources.Load<GameObject>("ButtonNumber");
                                bGo = Instantiate(bPrefab);

                                bGo.name = item.ToString();
                                bGo.transform.SetParent(canvas.transform, true);
                                var rt = bGo.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector2(rt.sizeDelta.x / 2 * 1.1f, -rt.sizeDelta.y * (.5f * 1.1f + 1) - rt.sizeDelta.y * (buttonsCount++));
                                item.Button = (new ItemsToChange.UiButton()
                                {
                                    go = bGo,

                                    bType =
                                     type == typeof(int) ? ItemsToChange.UiButtonType.NumberFuncInt : ItemsToChange.UiButtonType.NumberFuncFloat
                                });

                                bGo.name = item.MemeberInfo.Name;

                            }
                            else if (type == typeof(bool))
                            {
                                var bPrefab = Resources.Load<GameObject>("ButtonBool");
                                bGo = Instantiate(bPrefab);

                                bGo.name = item.ToString();
                                bGo.transform.SetParent(canvas.transform, true);
                                var rt = bGo.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector2(rt.sizeDelta.x / 2 * 1.1f, -rt.sizeDelta.y * (.5f * 1.1f + 1) - rt.sizeDelta.y * (buttonsCount++));
                                item.Button = (new ItemsToChange.UiButton() { go = bGo, bType = ItemsToChange.UiButtonType.BoolButton });

                                bGo.name = item.MemeberInfo.Name;
                            }
                            else if (type == typeof(string))
                            {
                                var bPrefab = Resources.Load<GameObject>("ButtonString");
                                bGo = Instantiate(bPrefab);

                                bGo.name = item.ToString();
                                bGo.transform.SetParent(canvas.transform, true);
                                var rt = bGo.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector2(rt.sizeDelta.x / 2 * 1.1f, -rt.sizeDelta.y * (.5f * 1.1f + 1) - rt.sizeDelta.y * (buttonsCount++));
                                item.Button = (new ItemsToChange.UiButton() { go = bGo, bType = ItemsToChange.UiButtonType.StringButton });

                                bGo.name = item.MemeberInfo.Name;
                            }
                            else if (type.IsEnum)
                            {
                                var bPrefab = Resources.Load<GameObject>("EnumButton");
                                bGo = Instantiate(bPrefab);

                                bGo.name = item.ToString();
                                bGo.transform.SetParent(canvas.transform, true);
                                var rt = bGo.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector2(rt.sizeDelta.x / 2 * 1.1f, -rt.sizeDelta.y * (.5f * 1.1f + 1) - rt.sizeDelta.y * (buttonsCount++));
                                item.Button = (new ItemsToChange.UiButton() { go = bGo, bType = ItemsToChange.UiButtonType.EnumButton });

                                bGo.name = item.MemeberInfo.Name;

                                var eb = bGo.GetComponent<EnumButton>();
                                eb.enumName = type.FullName;
                            }
                            else
                            {
                                Debug.LogError("Item " + item + " not supported " + item.MemeberInfo.MemberT);
                                continue;
                            }
                            break;
                        case System.Reflection.MemberTypes.Property:
                            var propType = item.GetValueType();
                            if (propType == typeof(int) || propType == typeof(float))
                            {
                                var bPrefab = Resources.Load<GameObject>("ButtonNumber");
                                bGo = Instantiate(bPrefab);

                                bGo.name = item.ToString();
                                bGo.transform.SetParent(canvas.transform, true);
                                var rt = bGo.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector2(rt.sizeDelta.x / 2 * 1.1f, -rt.sizeDelta.y * (.5f * 1.1f + 1) - rt.sizeDelta.y * (buttonsCount++));
                                item.Button = (new ItemsToChange.UiButton()
                                {
                                    go = bGo,

                                    bType =
                                     propType == typeof(int) ? ItemsToChange.UiButtonType.NumberButtonInt : ItemsToChange.UiButtonType.NumberButtonFloat
                                });

                                bGo.name = item.MemeberInfo.Name;

                            }
                            else if (propType.IsEnum)
                            {
                                var bPrefab = Resources.Load<GameObject>("EnumButton");
                                bGo = Instantiate(bPrefab);

                                bGo.name = item.ToString();
                                bGo.transform.SetParent(canvas.transform, true);
                                var rt = bGo.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector2(rt.sizeDelta.x / 2 * 1.1f, -rt.sizeDelta.y * (.5f * 1.1f + 1) - rt.sizeDelta.y * (buttonsCount++));
                                item.Button = (new ItemsToChange.UiButton() { go = bGo, bType = ItemsToChange.UiButtonType.EnumButton });

                                bGo.name = item.MemeberInfo.Name;

                                var eb = bGo.GetComponent<EnumButton>();
                                eb.enumName = propType.FullName;
                            }
                            else if (propType == typeof(bool))
                            {
                                var bPrefab = Resources.Load<GameObject>("ButtonBool");
                                bGo = Instantiate(bPrefab);

                                bGo.name = item.ToString();
                                bGo.transform.SetParent(canvas.transform, true);
                                var rt = bGo.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector2(rt.sizeDelta.x / 2 * 1.1f, -rt.sizeDelta.y * (.5f * 1.1f + 1) - rt.sizeDelta.y * (buttonsCount++));
                                item.Button = (new ItemsToChange.UiButton() { go = bGo, bType = ItemsToChange.UiButtonType.BoolButton });

                                bGo.name = item.MemeberInfo.Name;
                            }
                            else if (propType == typeof(string))
                            {
                                var bPrefab = Resources.Load<GameObject>("ButtonString");
                                bGo = Instantiate(bPrefab);

                                bGo.name = item.ToString();
                                bGo.transform.SetParent(canvas.transform, true);
                                var rt = bGo.GetComponent<RectTransform>();
                                rt.anchoredPosition = new Vector2(rt.sizeDelta.x / 2 * 1.1f, -rt.sizeDelta.y * (.5f * 1.1f + 1) - rt.sizeDelta.y * (buttonsCount++));
                                item.Button = (new ItemsToChange.UiButton() { go = bGo, bType = ItemsToChange.UiButtonType.StringButton });

                                bGo.name = item.MemeberInfo.Name;
                            }
                            else
                            {
                                Debug.LogError("Item " + item + " not supported " + item.MemeberInfo.MemberT);
                                continue;
                            }
                            break;
                        default:
                            Debug.LogError("Item " + item + " not supported " + item.MemeberInfo.MemberT);
                            continue;
                            //case System.Reflection.MemberTypes.Field:
                            //    break;
                    }
                    if (bGo != null)
                    {
                        bGo.SetActive(false); bGo.GetComponent<RectTransform>().localScale = Vector3.one;
                    }
                    i++;
                }
            }
            foreach (var go in toDestroy)
            {
                Debug.Log(go + " enable " + go.activeSelf);
                DestroyImmediate(go);
            }

           // var prefab = (GameObject)PrefabUtility.GetPrefabParent(canvas);
           // if (prefab != null) PrefabUtility.CreatePrefab(canvas, prefab);
        }

 

    }
}