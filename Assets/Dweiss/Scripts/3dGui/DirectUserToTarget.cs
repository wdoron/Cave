using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class DirectUserToTarget : MonoBehaviour
    {
        public Transform showTarget;
        public Collider target;
        public Camera cam;

        public float maxTimeForInvisible;

        private float _lostTime;
        private bool _isVisible;
        private bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible && value == false)
                {
                    _lostTime = Time.realtimeSinceStartup;
                }
                _isVisible = value;
            }
        }
        private bool _animationActive;
        private bool AnimationActive()
        {
            return IsVisible == false && Time.realtimeSinceStartup > _lostTime + maxTimeForInvisible;
        }

        private void Start()
        {
            if(cam == null)
                cam = Camera.main;
        }


        private float GetSign()
        {
            var dir = target.transform.position - cam.transform.position;
            return Vector3.Dot(dir, cam.transform.right) > 0 ? 1 : -1;

        }

        void Update()
        {

            IsVisible = ComponentExtension.IsVisibleFrom(target.bounds,cam);
            if (_animationActive && IsVisible)
            {
                SetAnimation(false);
            }else  if (_animationActive == false && AnimationActive())
            {
                SetAnimation(true);
            }

            if (_animationActive)
            {
                showTarget.LookAt(target.transform);
            }
        }

        private void SetAnimation(bool isAnimActive)
        {
            _animationActive = isAnimActive;
            showTarget.gameObject.SetActive(isAnimActive);
            
        }
    }
}