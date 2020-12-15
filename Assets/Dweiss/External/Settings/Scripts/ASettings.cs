/*******************************************************
 * Copyright (C) 2017 Doron Weiss  - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of unity license.
 * 
 * See https://abnormalcreativity.wixsite.com/home for more info
 *******************************************************/
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace Dweiss
{
    [System.Serializable]
    public abstract class ASettings : MonoBehaviour
    {
        public bool printDebug;

        public const string FileName = "Settings.txt";
        public const string FolderPath = ".FilesToCopy/";
        public const string SettingFolderPath = "Assets/" + FolderPath;

        public const string SettingPath = SettingFolderPath + FileName;//FilesToCopy/Settings.txt";

        //[HideInInspector]
        public bool _loadSettingInEditorPlay = true;
        public bool LoadSettingInEditorPlay { get { return _loadSettingInEditorPlay; } set { _loadSettingInEditorPlay = value; } }
        
        //[HideInInspector]
        private bool _autoSave = false;
        public bool AutoSave { get { return _autoSave; } set { _autoSave = value; } }
        public bool loadFromAssetsInEditor;


        public System.Action onValidateFunc;

        protected void Reset()
        {
            if (GameObject.FindObjectsOfType<ASettings>().Length != 1)
            {
                Debug.LogError("Too many settings singelton in scene. Destroy one");
            }
            else
            {
                name = "SettingsSingleton";
                Debug.Log("Create settings GameObject");
            }

            LoadEditorSetting();
#if UNITY_EDITOR
            SetScriptAwakeOrder<ASettings>(short.MinValue);
#endif
        }

        protected void OnValidate()
        {
#if UNITY_EDITOR
            if (AutoSave && Application.isPlaying == false)
            {
                //Debug.Log("Auto save " + Application.isPlaying + " " );
                SaveToFile(false);
            }
#endif
        }

        [ContextMenu("Log pathes")]
        private void LogWriteInfo()
        {
            Debug.LogFormat("{0}\n{1}", WindowsUserPath, LocalPath);
        }
        private string LocalPath { get
            {
                var localFile = Path.Combine(Application.dataPath, "../");
                return Path.GetFullPath(Path.Combine(localFile, FileName));
            } }
        private string WindowsUserPath
        {
            get
            {
                return Path.Combine(Application.persistentDataPath + "/", FileName);
            }
        }
        protected void Awake()
        {
            try
            {
                if (loadFromAssetsInEditor)
                {
                    if(LoadSetting(LocalPath) == false)
                        LoadSetting(WindowsUserPath);
                }
                else
                {
                    LoadEditorSetting();
                }
                var t = JsonUtility.ToJson(this);
                Debug.Log("Setting is: " + t);
            }
            catch (System.Exception e)
            {
                Debug.Log(name + " Error with loading scripts " + e);
            }
        }


        #region Inside Unity

        private void LoadEditorSetting()
        {
            if (LoadSettingInEditorPlay)
            {
                var filePath = System.IO.Path.Combine(Application.dataPath, FolderPath + FileName);

                LoadSetting(filePath);

            }
        }
        private string GetJsonFromFile(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return null;
        }

        private bool LoadSetting(string path)
        {
            var json = GetJsonFromFile(path);

            if (json != null)
            {
                Debug.Log("Sucess load settings file " + json);
                try
                {
                    var load = _loadSettingInEditorPlay;
                    var save = _autoSave;
                    JsonUtility.FromJsonOverwrite(json, this);
                    _autoSave = save;
                    _loadSettingInEditorPlay = load;
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(this);
#endif
                    return true;
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Error with overwriting settings file " + e);
                }
            }
            else
            {
                Debug.LogError("Failed loading settings of " + name + " from " + path);
            }
            return false;
        }
        #endregion
        #region Unity editor

#if UNITY_EDITOR
        public static void SetScriptAwakeOrder<T>(short num)
        {
            string scriptName = typeof(T).Name;
            SetScriptAwakeOrder(scriptName, num);
        }
        public static void SetScriptAwakeOrder(string scriptName, short num)
        {
            foreach (var monoScript in UnityEditor.MonoImporter.GetAllRuntimeMonoScripts())
            {
                if (monoScript.name == scriptName)
                {
                    var exeOrder = UnityEditor.MonoImporter.GetExecutionOrder(monoScript);
                    if (exeOrder != num)
                    {
                        //Debug.Log("Change script " + monoScript.name + " old " + exeOrder + " new " + num);
                        UnityEditor.MonoImporter.SetExecutionOrder(monoScript, num);
                    }
                    break;
                }
            }
        }
#endif
        public void LoadToScript()
        {
            var filePath = Path.Combine(Application.dataPath, FolderPath + FileName);
            LoadSetting(filePath);
        }

        [ContextMenu("Test Serialize")]
        private void TrySerialize()
        {
            var json = JsonUtility.ToJson(this);
            var values = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            //values.Remove("_autoSave");
            values.Remove("_loadSettingInEditorPlay");
            values.Remove("loadFromAssetsInEditor");
            values.Remove("printDebug");

            var json2 = Newtonsoft.Json.JsonConvert.SerializeObject(values);
            JsonUtility.FromJsonOverwrite(json2, this);
            Debug.Log("Test Json3 overwrite " + json2);
        }

        public void SaveToFile(bool forceWrite = true)
        {
            var filePath = Path.Combine(Application.dataPath, FolderPath + FileName);
            var loaded = GetJsonFromFile(filePath);
            string json = null;
            try
            {
                json = JsonUtility.ToJson(this);

            }
            catch (System.Exception e)
            {
                Debug.LogError("Error with loading settings file: " + e);
            }
            if (forceWrite || loaded != json || json == null)
            {
                var values = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                values.Remove("_loadSettingInEditorPlay");
                values.Remove("loadFromAssetsInEditor");
                values.Remove("printDebug");

                var json2 = Newtonsoft.Json.JsonConvert.SerializeObject(values);
                WriteToFile(json2);
            }
        }

        private static void WriteToFile(string json)
        {

            Debug.Log("Save settings file at " + ASettings.SettingPath + " with data: " + json);

            if (Directory.Exists(SettingFolderPath) == false)
            {
                Directory.CreateDirectory(SettingFolderPath);
            }
            using (var fs = File.CreateText(SettingPath))
            {
                fs.Write(' ');
            }
            if (json == null) json = "";
            File.WriteAllText(SettingPath, json);
        }
        #endregion
    }
}

