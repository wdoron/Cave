using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class Trackmovement : MonoBehaviour
    {
        public Dweiss.EventVector3 moveBackDelta;

        [SerializeField] private int maxHistoryFrameCount = 100;
        private Queue<Vector3> positions = new Queue<Vector3>();

        [SerializeField] private bool trackInFixedUpdate = false;
        public bool SkipThisFrame { get; set; }
        private bool WantToSkipThisFrame { get { if (SkipThisFrame) { SkipThisFrame = true; return true; } return false; } }


        public void GoToLastFrame() {
            if (positions.Count > 0) {
                var value = positions.Peek();
                moveBackDelta.Invoke(value - transform.position);
            }
        }
        // Update is called once per frame
        void Update() {
            if (trackInFixedUpdate || WantToSkipThisFrame) {
                SkipThisFrame = false;
                return;
            }

            positions.Enqueue(transform.position);
            while (positions.Count > maxHistoryFrameCount) {
                positions.Dequeue();
            }
        }

        void FixedUpdate() {
            if (trackInFixedUpdate == false || WantToSkipThisFrame) {
                SkipThisFrame = false;
                return;
            }

            positions.Enqueue(transform.position);
            while (positions.Count > maxHistoryFrameCount) {
                positions.Dequeue();
            }
        }
    }
}