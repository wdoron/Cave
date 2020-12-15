using UnityEngine;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class PrefabSupport : MonoBehaviour {

    public string prefabGroupName;

    private void Reset()
    {
        prefabGroupName = "PrefabGroupID_" + Random.Range(0, int.MaxValue);
        UpdateName();
    }

    [System.Serializable]
    public class GameObjectInfo
    {
        public GameObject prefab;

        public string parentName = "";
        public Vector3 localPosition = Vector3.zero;
        public Quaternion localRotation = Quaternion.identity;
        public Vector3 localScale = Vector3.one;
    }

	public List<GameObjectInfo> prefabsChildrens;
	public bool validateFinished = false;
//	public bool updatePrefabsChildrens = false;


//	private void SetupChildrens() {
//		if (updatePrefabsChildrens) {
//			Debug.Log (name + " setup prefabsChildrens");
//			var list = transform.parent.Cast<Transform> ();
//			prefabsChildrens =  list.Select (a => a.gameObject).Where(a => a.name != name && a.name != transform.parent.name).ToList ();
//			updatePrefabsChildrens = false;
//		}
//	}

	private bool SetupPrefab() {
		
		
#if UNITY_EDITOR
		if (validateFinished)
			return true;


		var editorPref = GameObject.Find (name);
		Debug.Log ("Prefab support check " + name + " heirarchy contains " + editorPref);
		if ( editorPref != null && editorPref == this.gameObject) {
			Debug.Log ("Prefab support creation of" + editorPref);
			if (editorPref.transform.parent == null) { 
				var go = new GameObject (name + "Group");
				editorPref.transform.parent = go.transform;
			}
			var prefabsParent = editorPref.transform.parent;
            var prefabsHeirarchy = new Dictionary<string, Transform>() { };
            //var parentsProblems = new List<Transform>();

			var allChildrenGo = prefabsParent.GetComponentsInChildren<Transform> (true).Select (a => a.name).ToList ();
			var myMissingPrefabs = prefabsChildrens.Where (a => allChildrenGo.Contains (a.prefab.name) == false).ToList ();
			myMissingPrefabs.ForEach (a => {
				GameObject newPref;
				newPref = (GameObject)PrefabUtility.InstantiatePrefab (a.prefab);
                prefabsHeirarchy[newPref.name] = newPref.transform;
                
                newPref.transform.parent = string.IsNullOrEmpty(a.parentName) ? prefabsParent : prefabsHeirarchy[a.parentName];///TODO fill and test? use parenting by instance id
                newPref.transform.localPosition = a.localPosition;
                newPref.transform.localRotation = a.localRotation;
                newPref.transform.localScale = a.localScale;

                newPref.name = a.prefab.name;
				Debug.LogFormat ("{0} >>> Duplicate {1} as {2}", name, a.prefab.name, newPref.name);
			});
			return true;
		}
#endif
		return false;
	}

    private void UpdateName()
    {
        if (name != prefabGroupName)
        {
            name = prefabGroupName;
#if UNITY_EDIT
            EditorUtility.SetDirty(this);
#endif
        }
    }
	private void OnValidate()
	{
        UpdateName();

        if (SetupPrefab ()) validateFinished = true;
//		SetupChildrens ();

	}


#if UNITY_EDITOR
    private void Update()
    {
        if (Application.isEditor)
        {
            if (SetupPrefab()) validateFinished = true;
        }
	}
#endif

}
