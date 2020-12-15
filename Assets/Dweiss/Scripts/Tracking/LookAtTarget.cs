using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common {
	//[ExecuteInEditMode]
	public class LookAtTarget : MonoBehaviour {
		public Transform target;

		//Try to use the different lock and see what happens
		public bool xLock, yLock = true, zLock;

        public bool inUpdate = true;
        private void LateUpdate()
        {
            if(inUpdate == false) SetLook();
        }
        private void Update() {
            if (inUpdate) SetLook();
        }
        private void SetLook() { 
            if (target == null) target = Camera.main.transform;
            var p = target.position;
			if(xLock) p.x = transform.position.x;
			if (yLock) p.y = transform.position.y;
			if (zLock) p.z = transform.position.z;
			transform.LookAt(p);
		}
	}
}

