using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class MouseControlThreeAxes : MonoBehaviour
    {
        public int mouseKeyToActivate = 2;
        public float xzMoveFactor = 0.001f;
        public float yScrollFactor = 0.1f;
        private Vector3 lastMousePos;

        private Transform t;
        private void Awake()
        {
            t = transform;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(mouseKeyToActivate))
            {
                lastMousePos = Input.mousePosition;
            }
            else if (Input.GetMouseButton(mouseKeyToActivate))
            {
                var delta = lastMousePos - Input.mousePosition;
                t.position += new Vector3(-delta.x, 0, -delta.y) * xzMoveFactor;
            }

            t.position += new Vector3(0, Input.mouseScrollDelta.y * yScrollFactor, 0);
        }
    }
}