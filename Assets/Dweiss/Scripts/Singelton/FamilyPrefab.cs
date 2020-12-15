using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class FamilyPrefab : MonoBehaviour
    {

        [System.Serializable]
        public class ReferenceGo
        {
            public GameObject go;
            public Vector3 shift;
            public Vector3 rotate;
            public Vector3 scaleFactor;
            public Quaternion Rotation { get { return Quaternion.Euler(rotate); } }
            public ReferenceGo(GameObject pref, GameObject go)
            {
                this.go = pref;
                shift = go.transform.localPosition;
                rotate = go.transform.localRotation.eulerAngles;
                scaleFactor = go.transform.localScale;
            }
        }

        public List<ReferenceGo> referencedPrefabs;
        //public GameObject[] currentChildren;


        private void Reset()
        {
            CreateReferenceFromScene();
        }


        private void Awake()
        {
            ResetFromReference(false);
        }

        public void ResetFromReference(bool createNew)
        {
            if (createNew)
            {
                //for (int i = 0; i < currentChildren.Length; ++i)
                //{
                //    Destroy(currentChildren[i]);
                //}
                //currentChildren = new GameObject();
                for (int i = transform.childCount-1; i >= 0; --i)
                {
                    DestroyImmediate(transform.GetChild(i).gameObject);
                }
            } else
            {
                if(transform.childCount > 0) { return; }
            }
            //currentChildren = new GameObject[referencedPrefabs.Count];
            for (int i = 0; i < referencedPrefabs.Count; ++i)
            {
                GameObject go;
#if UNITY_EDITOR
                go = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(referencedPrefabs[i].go);
                go.transform.parent = transform;
#else
                go = Instantiate(referencedPrefabs[i].go, transform);
#endif
                go.transform.localPosition = referencedPrefabs[i].shift;
                go.transform.localRotation = referencedPrefabs[i].Rotation;
                go.transform.localScale = referencedPrefabs[i].scaleFactor;
               // currentChildren[i] = go;
            }
        }

        public void CreateReferenceFromScene()
        {
            referencedPrefabs = new List<ReferenceGo>(transform.childCount);
            for (int i = 0; i < transform.childCount; ++i)
            {
                var child = transform.GetChild(i);
#if UNITY_EDITOR
                var prefabP = (GameObject)UnityEditor.PrefabUtility.GetPrefabParent(child.gameObject);
                if (prefabP)
                {
                    referencedPrefabs.Add(new ReferenceGo(prefabP, child.gameObject));
                }
#endif
            }


        }

    }
}