/*******************************************************
 * Copyright (C) 2017 Doron Weiss  - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of unity license.
 * 
 * See https://abnormalcreativity.wixsite.com/home for more info
 *******************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Dweiss
{
    [System.Serializable]
    public class Settings : Dweiss.Singleton<ASettings>
    {
        public bool printDebug;
        
        [Header("Command parsing")]

        [ArrayElementTitle("shortDescription")]
        public GameObject[] arr;
        

      
        

        private new void Awake()
        {
            base.Awake();
            Setup();
        }

        private void Setup()
        {
            //GameObject.FindObjectOfType<MusicManager>().url = wavBackgroundMusic;


#if UNITY_EDITOR
          
#endif
            
        }





    }
}