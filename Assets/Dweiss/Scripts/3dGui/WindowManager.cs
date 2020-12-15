using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    [RequireComponent(typeof(MoveToFace))]
    public class WindowManager : MonoBehaviour
    {
        public Transform resetPos;
        [SerializeField]private Transform _cam;
        public Transform Cam
        {
            get
            {
                if (_cam == null)
                    _cam = Camera.main.transform;
                return _cam;
            }
        }
        [SerializeField] private MoveToFace mover;
        public bool hideOnStart;


        [SerializeField]private bool isShowing;
        void Awake()
        {
           
            if(mover != null)
                mover = GetComponentInChildren<MoveToFace>();

            mover.onFinish.AddListener(OnWindowFinishedMoving);
            if (hideOnStart)
            {
                Hide();
                isShowing = false;
            }

        }

        private void OnWindowFinishedMoving()
        {
            mover.transform.SetParent(isShowing ? null : resetPos);
        }

        public void Show()
        {
            isShowing = true;
            mover.MoveTowards(Cam);
        }
        public void Hide()
        {
            mover.transform.SetParent(resetPos);
            isShowing = false;
            mover.MoveTowards(resetPos);

        }
    }
}