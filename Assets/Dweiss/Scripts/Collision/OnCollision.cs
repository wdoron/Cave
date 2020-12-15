using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dweiss.Common
{
    public class OnCollision : MonoBehaviour
    {
        public bool debug;

        public OnCollisionEvent onCollisionEnter, onCollisionExit;
        public Dweiss.EventCollider onColliderEnter, onColliderExit;

        public LayerMask layer = new LayerMask() { value = -1 /*Everything*/ };
        public string tagToCompare = "";

        private void Reset()
        {
            layer.value = -1;
        }

        [System.Serializable]
        public class OnCollisionEvent : UnityEngine.Events.UnityEvent<Collision> { }

        public void OnCollisionEnter(Collision cldr)
        {
            if ((string.IsNullOrEmpty(tagToCompare) || cldr.gameObject.CompareTag(tagToCompare)) &&
                layer.HasLayer(cldr.gameObject.layer))
            {
                if (debug) Debug.Log(name + " OnCollisionEnter success with " + cldr.collider);
                onCollisionEnter.Invoke(cldr);
                onColliderEnter.Invoke(cldr.collider);
            }
        }
        public void OnCollisionExit(Collision cldr)
        {
            if ((string.IsNullOrEmpty(tagToCompare) || cldr.gameObject.CompareTag(tagToCompare)) &&
                layer.HasLayer(cldr.gameObject.layer))
            {
                if (debug) Debug.Log(name + " OnCollisionExit success with " + cldr.collider);
                onCollisionExit.Invoke(cldr);
                onColliderExit.Invoke(cldr.collider);
            }
        }
    }
}