using UnityEngine;
using System.Collections.Generic;


public static class GOManager<T> where T : Component
{


    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<T>();
            }
            return _instance;
        }
    }

    public static T Inst
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<T>();
            }
            return _instance;
        }
    }

}

public static class Single<T> where T : Component {


    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<T>();
            }
            return _instance;
        }
    }

    public static T Inst
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<T>();
            }
            return _instance;
        }
    }

}



//public class MonoBehaviorExt : MonoBehaviour
//{
//    protected Transform _transform;
//    public Transform trans { get { if (_transform == null) _transform = transform; return _transform; } }
//}


//    //TODO does this singlton causes memory leak?
//public static class GOManager
//{

//    private class GOManagerDestructor : MonoBehaviour
//    {

//        void OnDestroy()
//        {
//            var comps = GetComponentsInChildren<Transform>();
//            for(int i=0; i < comps.Length; ++i)
//            {
//                var removed = _goDic.Remove(comps[i].gameObject);
//                Debug.Log("Destroy GOManager object success " + removed + " -> " + _goDic.Count);
//            }
//        }
//    }

//    private static Dictionary<GameObject, Dictionary<System.Type, Component>> _goDic
//        = new Dictionary<GameObject, Dictionary<System.Type, Component>>();

//    public static T GetSngltn<T>(this Component c, bool checkChildren = true) where T : Component
//    {
//        var go = c.gameObject;
//        Dictionary<System.Type, Component> compList = null;
//        _goDic.TryGetValue(go, out compList);
//        if (compList == null)
//        {
//            compList = new Dictionary<System.Type, Component>();
//            _goDic[go] = compList;
//        }

//        Component comp = null;
//        compList.TryGetValue(typeof(T), out comp);
//        if (comp == null)
//        {
//            comp = c.GetComponent<T>();
//			if(comp == null && checkChildren) comp = c.GetComponentInChildren<T>();
//			if(comp == null) return null;

//            compList[typeof(T)] = comp;

//            if(c.GetComponent<GOManagerDestructor>() == null)
//            {
//                go.AddComponent<GOManagerDestructor>();
//            }
//        }
//        return comp as T;
//    }

//}


/////////

//public static class GetFirstComponent
//{

//	public static T First<T>(this Component c) where T : Component
//	{
//		var ch = c.GetSngltn<ComponentHelper>(false);
//		if (ch == null) {
//			ch = c.gameObject.AddComponent<ComponentHelper>();
//		}
//		var ret = ch.First<T> ();
//		return ret;
//	}

//	public class ComponentHelper : MonoBehaviour
//	{
//		private Dictionary<System.Type, Component> firstDic = new Dictionary<System.Type, Component>();

//		public T First<T>() where T : Component {
//			Component cmp;
//			firstDic.TryGetValue (typeof(T), out cmp);
//			if (cmp == null) {
//				cmp = GetComponent<T>();
//				firstDic[typeof(T)] = cmp;
//			}
//			return cmp as T;
//		}
//	}


//}