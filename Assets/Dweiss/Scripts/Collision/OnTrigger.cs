using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss.Common
{
    public class OnTrigger : MonoBehaviour
    {
        public bool debug;

        public OnTriggerEvent onTriggerEnter, onTriggerExit;
        public Dweiss.EventString onTriggerEnterTag, onTriggerExitTag;

        public LayerMask layer = new LayerMask() {value = -1 /*Everything*/ };
        public string tagToCompare = "";


        private void Reset()
        {
            layer.value = -1;
        }

        [System.Serializable]
        public class OnTriggerEvent : UnityEngine.Events.UnityEvent<Collider> { }

        public void OnTriggerEnter(Collider cldr)
        {
            if (debug) Debug.Log(name + " OnTriggerEnter " + cldr + " tag " + cldr.gameObject.tag);
            if ((string.IsNullOrEmpty(tagToCompare) || cldr.gameObject.CompareTag(tagToCompare)) &&
                  layer.HasLayer(cldr.gameObject.layer))
            {
                //Debug.Log(name + " OnTriggerEnter valid on " + cldr );
                onTriggerEnter.Invoke(cldr);
                onTriggerEnterTag.Invoke(cldr.tag);
            }
        }

        public void OnTriggerExit(Collider cldr)
        {
            if (debug) Debug.Log(transform.FullName() + " OnTriggerExit " + cldr + " tag " + cldr.gameObject.tag);
            if ((string.IsNullOrEmpty(tagToCompare) || cldr.gameObject.CompareTag(tagToCompare)) &&
                   layer.HasLayer(cldr.gameObject.layer))
            {
                //Debug.Log(name + " OnTriggerExit valid on " + cldr);
                onTriggerExit.Invoke(cldr);
                onTriggerExitTag.Invoke(cldr.tag);
            }
        }
    }
}