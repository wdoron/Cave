using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class SavePoint : MonoBehaviour
    {
        [SerializeField] private Transform reference;
        [SerializeField] private bool saveOnAwake = true;


        private Vector3 pos, rot, scale;
        private Transform parent;

        private Vector3 savedVelocity;
        public Dweiss.SimpleEvent onSave, onLoad;
        private Rigidbody rb;


        public void SetTransform(Transform newTransform)
        {
            reference = newTransform;
        }
        public void Unsave()
        {
            _wasSaved = false;
        }
        private bool _wasSaved;
        public void Save()
        {
            SaveWithReference(true);
        }
        private void SaveWithReference(bool considerReference) { 
            parent = transform.parent;
           
            scale = transform.localScale;

            if(considerReference && reference != null)
            {
                //Debug.Log("Saving by reference");

                pos = reference.InverseTransformPoint(transform.position);
                rot = (reference.rotation.Inverse() * transform.rotation).eulerAngles;
                //scale =  transform.localScale;
            } else
            {
                pos = transform.position;
                rot = transform.rotation.eulerAngles;
            }

            if (rb) savedVelocity = rb.velocity;

            _wasSaved = true;

            onSave.Invoke();
        }

        public void Load()
        {
            if(_wasSaved == false)
            {
                Debug.LogWarning(name + " Load rejected. No saved data");
                return;
            }
            transform.parent = parent;
            
            transform.localScale = scale;

            if (reference != null)
            {
                //Debug.Log("Loading by reference");
                transform.position = reference.TransformPoint(pos);
                transform.rotation = (reference.rotation * Quaternion.Euler(rot));
                //scale =  transform.localScale;
            } else
            {
                transform.position = pos;
                transform.rotation = Quaternion.Euler(rot);
            }

            if (rb) rb.velocity = savedVelocity;

            onLoad.Invoke();

        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if(saveOnAwake) Save();
        }

       
    }
}