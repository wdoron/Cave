using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    [RequireComponent(typeof(MoveToFace))]
    public class SmoothKeepInView : MonoBehaviour {
        private MoveToFace mtf;
        private Renderer rnd;
        private Camera cam;
        void Start() {
            cam = Camera.main;
            mtf = GetComponent<MoveToFace>();
            rnd = GetComponentInChildren<Renderer>();
        }




        void Update() {
            if (rnd.IsVisibleFrom(cam))
            {
               
            }else
            {
               if(mtf.IsMoving == false) mtf.Move();
            }
        }
    }
}