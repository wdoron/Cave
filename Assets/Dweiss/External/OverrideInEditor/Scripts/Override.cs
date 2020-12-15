/*****************************************************************************
 * Copyright (C) 2017 Doron Weiss  - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of unity3d license https://unity3d.com/legal/as_terms.
 * 
 * See https://abnormalcreativity.wixsite.com/home for more info
 ******************************************************************************/
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Dweiss
{
   
    public class Override : MonoBehaviour
    {

        public List<RuntimePlatform> runtimePlatforms = new List<RuntimePlatform>(new[] {
            RuntimePlatform.OSXEditor, RuntimePlatform.LinuxEditor, RuntimePlatform.WindowsEditor });


        [Tooltip("Write to log on action successful")]
        public bool showInfo = false;

        [EditorItem(false)]
        [Tooltip("Choose scripts you want to change during editor or call different functions during different builds. " +
            "Especially usable inside unity editor for testing. " +
            "Allow configuration of prefabs that are being instantiated (it will be affected on play and reverted during stop)")]
        public ItemsToChange[] itemsToChange = new ItemsToChange[0];


        [Tooltip("Usable for prefabs manipulation")]
        private static bool revertOnDestroy = true;

        private void Reset()
        {
            this.SetScriptAwakeOrder(short.MinValue);
            runtimePlatforms = new List<RuntimePlatform>(new[] {
                RuntimePlatform.OSXEditor, RuntimePlatform.LinuxEditor, RuntimePlatform.WindowsEditor });
        }


        //Show enable button
        private void Start() { }

        private void Awake()
        {
            var kes = ItemsToChange.Keys;
            if (enabled == false) return;
            
            if (runtimePlatforms.Contains(Application.platform))
            {
                if (itemsToChange.Length > 0)
                {
                    Debug.LogWarningFormat(this + " Override {0} items:\n{1}", itemsToChange.Length, 
                        string.Join("\n", itemsToChange.Select(a => a.ToString()).ToArray()));
                    ExeOverride();
                }
            }
        }

        private void ExeOverride()
        {
            for (int i = 0; i < itemsToChange.Length; ++i)
            {
                try
                {
                    itemsToChange[i].Override(showInfo);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Error in override " + itemsToChange[i] + ": " + e);
                    if(Application.isEditor == false)
                    {
                        Debug.LogError("Prefabs might have problems in override during play");
                    }

                }
            }
        }

        private void OnDestroy()
        {
            if (itemsToChange.Length == 0 || enabled == false)
            {
                return;
            }

            if (revertOnDestroy)
            {
                if (showInfo) Debug.Log("Revert on destroy");
                bool succeded = true;
                for (int i = 0; i < itemsToChange.Length; ++i)
                {
                    try
                    {
                        if (itemsToChange[i].go != null)
                        {
                            if(itemsToChange[i].UndoOverride(showInfo) == false)
                            {
                                succeded = false;
                                Debug.LogError("Error in UndoOverride: " + itemsToChange[i].Info() + "  should be " + itemsToChange[i].oldValue);
                            }
                        }
                    } catch(System.Exception e)
                    {
                        Debug.LogError("Error in UndoOverride " + itemsToChange[i] + ": " + e);
                    }
                }
                if (succeded)
                {
                    if (showInfo) Debug.LogWarning("Override items reverted successfuly");
                }
                else
                {
                    Debug.LogError("Override items Failed!! see previuos comments for errors and fixes");
                }
            }
        }

    }
}