using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnBuild : MonoBehaviour {

    void Awake()
    {
#if UNITY_EDITOR
#else
        Destroy(gameObject);
#endif
    }
}
