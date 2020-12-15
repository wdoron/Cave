using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    //[ExecuteInEditMode]
    public class HideShowAll : MonoBehaviour
    {
        public Renderer[] allRndrs;
        public Behaviour[] allMono;
        public GameObject[] allgo;

        public bool setActive;


        private void Start()
        {
            SetHideShow(setActive);
        }
        public void SetHideShow(bool active)
        {
            for(int i=0; i < allgo.Length; ++i)
            {
                allgo[i].SetActive(active);
            }
            for (int i = 0; i < allMono.Length; ++i)
            {
                allMono[i].enabled = (active);
            }
            for (int i = 0; i < allRndrs.Length; ++i)
            {
                allRndrs[i].enabled = (active);
            }
        }
    }
}