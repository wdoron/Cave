using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class ActiveFor : MonoBehaviour
    {
        public float activeTime;

        private void OnEnable()
        {
            this.Invoke("Disable", activeTime);
        }
        private void Disable()
        {
            gameObject.SetActive(false);
        }

        public void ReactivateGameObject()
        {
            gameObject.SetActive(true);
            StopAllCoroutines();
            this.Invoke("Disable", activeTime);
        }
    }
}