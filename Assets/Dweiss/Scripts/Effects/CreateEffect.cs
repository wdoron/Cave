using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class CreateEffect : MonoBehaviour
    {
        public GameObject prefab;
        public float ttl;

        private GameObject current;

        public void StartEffect()
        {
            StopEffect();
            current = Instantiate(prefab, transform);
            current.transform.localPosition = Vector3.zero;
            current.transform.localRotation = Quaternion.identity;
            if (ttl > 0) Destroy(current, ttl);
        }

        public void StopEffect()
        {
            if (current)
            {
                Destroy(current);
                current = null;
            }
        }

        private void OnDisable()
        {
            StopEffect();
        }
    }
}