using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Dweiss;

public static class ComponentExtension
{

    public static IEnumerator RunForPeriod(this Component that, float activeTime, System.Action<float> action)
    {
        float startTime = Time.time;
        float endTime = Time.time + activeTime;
        float percent = 0;

        while (endTime > Time.time)
        {
            percent = (Time.time - startTime) / activeTime;
            action(percent);
            yield return 0;
        }
        action(1f);
    }

    public static string ToFormatedString(this Vector3 that, string seperator, string floatToStrFormat = null)
    {
        return floatToStrFormat == null ? string.Format("{0}{3}{1}{3}{2}", that.x, that.y, that.z, seperator) :
            string.Format("{0}{3}{1}{3}{2}", that.x.ToString(floatToStrFormat), that.y.ToString(floatToStrFormat), that.z.ToString(floatToStrFormat), seperator);
    }

    public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
    {
        return IsVisibleFrom(renderer.bounds, camera);
    }

    public static bool IsVisibleFrom(Bounds bounds, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, bounds);
    }

    public static Quaternion Inverse(this Quaternion that)
    {
        return Quaternion.Inverse(that);
    }




    public delegate void SimpleMethod();

    public static Vector3[] GetPoints(this Bounds b)
    {
        var c = b.center;
        var ex = b.extents;
        var points = new Vector3[] {
                     new Vector3(c.x + ex.x, c.y + ex.y, c.z - ex.z)
                    ,new Vector3(c.x + ex.x, c.y + ex.y, c.z + ex.z)
                    ,new Vector3(c.x + ex.x, c.y - ex.y, c.z - ex.z)
                    ,new Vector3(c.x + ex.x, c.y - ex.y, c.z + ex.z)

                    ,new Vector3(c.x - ex.x, c.y - ex.y, c.z + ex.z)
                    ,new Vector3(c.x - ex.x, c.y - ex.y, c.z - ex.z)
                    ,new Vector3(c.x - ex.x, c.y + ex.y, c.z + ex.z)
                    ,new Vector3(c.x - ex.x, c.y + ex.y, c.z - ex.z)
                };
        return points;
    }



    public static void Enable(this Behaviour[] bList, bool active)
    { for (int i = 0; i < bList.Length; ++i) bList[i].enabled = active; }
    public static void Enable(this Renderer[] bList, bool active)
    { for (int i = 0; i < bList.Length; ++i) bList[i].enabled = active; }
    public static void Enable(this Collider[] bList, bool active)
    { for (int i = 0; i < bList.Length; ++i) bList[i].enabled = active; }

    public static void Enable(this IList<Behaviour> bList, bool active)
    { for (int i = 0; i < bList.Count; ++i) bList[i].enabled = active; }
    public static void Enable(this IList<Renderer> bList, bool active)
    { for (int i = 0; i < bList.Count; ++i) bList[i].enabled = active; }
    public static void Enable(this IList<Collider> bList, bool active)
    { for (int i = 0; i < bList.Count; ++i) bList[i].enabled = active; }

    public static void SetActive(this GameObject[] goList, bool active)
    {
        for (int i = 0; i < goList.Length; ++i)
        {
            goList[i].SetActive(active);
        }
    }
    public static void SetActive(this System.Collections.Generic.IList<GameObject> goList, bool active)
    {
        for (int i = 0; i < goList.Count; ++i)
        {
            goList[i].SetActive(active);
        }
    }

    public static void SetActive(this System.Collections.Generic.IList<Transform> goList, bool active)
    {
        for (int i = 0; i < goList.Count; ++i)
        {
            goList[i].gameObject.SetActive(active);
        }
    }

    public static void PendingAnimation(this MonoBehaviour that, Animation anim, SimpleMethod f)
    {
        that.StartCoroutine(WaitForAnimation(anim, f));
    }

    public static IEnumerator WaitForAnimation(Animation animation, SimpleMethod f)
    {
        do
        {
            yield return null;
        } while (animation.isPlaying);
        f();
    }

    public static Coroutine WaitForSeconds(this MonoBehaviour that, float seconds, SimpleMethod f)
    {
        return that.StartCoroutine(WaitForSecondsHelper(seconds, f));
    }

    public static Coroutine ExactWaitForEvent(this MonoBehaviour that, float seconds, SimpleMethod f)
    {
        return that.StartCoroutine(ExactWaitForEventHelper(seconds, f));
    }
    public static Coroutine SimpleWaitWhile(this MonoBehaviour that, System.Func<bool> isTrue, SimpleMethod f)
    {
        return that.StartCoroutine(SimpleWaitWhileHelper(isTrue, f));
    }

    public static IEnumerator SimpleWaitWhileHelper(System.Func<bool> isTrue, SimpleMethod f)
    {
        yield return new WaitWhile(isTrue);
        f();
    }
    public static IEnumerator ExactWaitForEventHelper(float seconds, SimpleMethod f)
    {
        var startTime = Time.time;

        var waitTime = seconds;
        while (waitTime > .5f * Time.deltaTime)
        {
            yield return 0;
            waitTime = seconds - (Time.time - startTime);
        }
        f();
    }

    public static IEnumerator ExactWait(float seconds)
    {
        var startTime = Time.time;

        var waitTime = seconds;
        while (waitTime > .5f * Time.deltaTime)
        {
            yield return 0;
            waitTime = seconds - (Time.time - startTime);
        }
    }

    public static IEnumerator ExactWait(float targetTime, System.Func<float> currentTime)
    {
        while ((targetTime - currentTime()) > .5f * Time.deltaTime)
        {
            yield return 0;
        }
    }

    public static Coroutine WaitForSecondsFixedTime(this MonoBehaviour that, float seconds, SimpleMethod f)
    {
        return that.StartCoroutine(WaitForSecondsHelperFixedTime(seconds, f));
    }

    public static IEnumerator WaitForSecondsHelperFixedTime(float seconds, SimpleMethod f)
    {
        yield return new WaitForSeconds(seconds);
        yield return new WaitForFixedUpdate();
        f();
    }
    public static Coroutine SimpleCoroutine(this MonoBehaviour that, System.Func<float> f)
    {
        return that.StartCoroutine(CoroutineBetweenFrame(f, -1));
    }

    public static Coroutine SimpleCoroutine(this MonoBehaviour that, float waitToBegin, System.Func<float> f)
    {
        return that.StartCoroutine(CoroutineBetweenFrame(f, waitToBegin));
    }

    public static IEnumerator CoroutineBetweenFrame(System.Func<float> f, float waitToBegin)
    {
        if (waitToBegin >= 0)
            yield return new WaitForSeconds(waitToBegin);

        while (true)
        {
            var call = f();
            if (float.IsPositiveInfinity(call)) break;
            yield return new WaitForSeconds(call);
        }

    }


    public static IEnumerator WaitForSecondsHelper(float seconds, SimpleMethod f)
    {
        if (seconds >= 0)
            yield return new WaitForSeconds(seconds);

        f();
    }

    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        var ret = go.GetComponent<T>();
        if (ret == null) ret = go.AddComponent<T>();
        return ret;
    }
    public static void SetActive(this Transform t, bool active)
    {
        t.gameObject.SetActive(active);
    }
    public static void SetActive(this Component t, bool active)
    {
        t.gameObject.SetActive(active);
    }

    public static Transform FindChild(this Transform parent, System.Func<Transform, bool> filter)
    {
        Transform t = parent.transform;
        foreach (Transform tr in t)
        {
            if (filter(tr))
            {
                return tr;
            }
            var withTag = FindChild(tr, filter);
            if (withTag != null)
            {
                return withTag;
            }
        }
        return null;
    }
    public static GameObject FindChildWithTag(this GameObject parent, string tag)
    {
        var trans = FindChildWithTag(parent.transform, tag);

        return trans == null? null : trans.gameObject;
    }
    public static Transform FindChildWithTag(this Transform parent, string tag)
    {
        Transform t = parent.transform;
        foreach (Transform tr in t)
        {
            if (tr.tag == tag)
            {
                return tr;
            }
            var withTag = FindChildWithTag(tr, tag);
            if (withTag != null)
            {
                return withTag;
            }
        }
        return null;
    }

    public static List<Transform> GetChildrens(this Transform t, bool includeDisable = true)
    {
        var children = new List<Transform>(includeDisable ? t.childCount : t.childCount / 4);
        for (int i = 0; i < t.childCount; ++i)
        {
            var c = t.GetChild(i);
            if (includeDisable || c.gameObject.activeSelf)
                children.Add(c);
        }
        return children;
    }

    public static Transform RecGetTransformByHeirarchyName(this Transform current, string name, char seperator = '.')
    {
        var nameArr = name.SplitOnce(seperator);
        var children = current.GetChildrens();
        for (int i = 0; i < children.Count; ++i)
        {
            if (children[i].name == nameArr[0])
            {
                if (string.IsNullOrEmpty(nameArr[1])) return children[i];

                return RecGetTransformByHeirarchyName(children[i], nameArr[1], seperator);
            }
            //.name == nameArr[0])
        }
        return null;
    }

    public static List<T> FindObjectsOfType<T>(bool includeInactive = true)
    {
        List<T> results = new List<T>();
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; ++i)
        {
            var sc = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            //foreach (var sc in UnityEngine.SceneManagement.SceneManager.GetAllScenes()) { 
            sc.GetRootGameObjects().ToList().ForEach(g => results.AddRange(g.GetComponentsInChildren<T>(includeInactive)));
        }
        return results;
    }

    public static GameObject[] GetActiveSceneRootObject()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
    }

    public static T FindFirstInActiveScene<T>(System.Func<GameObject, T> filterFromRoot) where T : class
    {
        var gos = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        for (int i = 0; i < gos.Length; i++)
        {
            var ret = filterFromRoot(gos[i]);
            if (ret != null) return ret;
        }
        return null;
    }

    public static List<T> FindAllInActiveScene<T>(System.Func<GameObject, List<T>> filterFromRoot) where T : class
    {
        var gos = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        var ret = new List<T>();
        for (int i = 0; i < gos.Length; i++)
        {
            var list = filterFromRoot(gos[i]);
            if (list != null && list.Count != 0) ret.AddRange(list);
        }
        return ret;
    }
    public static T FindObjectOfType<T>(bool includeInactive = true) where T : class
    {
        List<T> results = FindObjectsOfType<T>(includeInactive);

        return results.Count > 0 ? results[0] : null;
    }

    public static string FullName(this MonoBehaviour mono, char seperator = '.')
    {
        var ret = "";
        Transform t = mono.transform;
        while (t != null)
        {
            ret = t.name + (string.IsNullOrEmpty(ret) ? "" : (seperator + ret));
            t = t.parent;
        }
        return ret;
    }

    public static string FullName(this Transform t, char seperator = '.')
    {
        var ret = "";
        while (t != null)
        {
            ret = t.name + (string.IsNullOrEmpty(ret) ? "" : (seperator + ret));
            t = t.parent;
        }
        return ret;
    }
    public static Component CopyComponent(Component original, GameObject destination)
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        // Copied fields can be restricted with BindingFlags
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy;
    }
    public static Vector3 FindPointOnColliderInDir(this Collider a, Vector3 dir)
    {
        var farPoint = a.transform.position + dir * a.bounds.size.sqrMagnitude;
        var pOnBounds = a.ClosestPoint(farPoint);
        return pOnBounds;
    }

    public static Vector3 DistanceBetweenBounds(this Collider a, Collider b)
    {
        var cldrBPoint = b.ClosestPoint(a.transform.position);
        var cldrAPoint = a.ClosestPoint(cldrBPoint);

        //var dir = (cldrAPoint - a.transform.position).normalized;
        //if (dir.sqrMagnitude == 0) dir = Vector3.up;

        //var timeLength = Time.deltaTime;

        //Debug.DrawLine(a.transform.position - dir, cldrAPoint + dir, Color.cyan, timeLength);
        //Debug.DrawLine(b.transform.position, cldrBPoint, Color.white, timeLength);
        //Debug.DrawLine(a.transform.position, cldrAPoint, Color.magenta, timeLength);

        //DebugExt.DrawBox(a.transform.position, a.bounds.extents, Color.magenta, timeLength);
        //Debug.DrawRay(a.transform.position, a.bounds.extents, Color.cyan, timeLength);
        //DebugExt.DrawBox(b.transform.position, b.bounds.extents, Color.white, timeLength);

        //Debug.LogFormat("{0},{1} >> {2},{3} >>>> {4}\n {5}\t{6} !!", 
        //    a.transform.position, b.transform.position, cldrAPoint, cldrBPoint, (cldrBPoint - cldrAPoint), 
        //    a.bounds.extents.ToMiliStr(), b.bounds.extents.ToMiliStr());

        return cldrBPoint - cldrAPoint;
    }

    public static bool IsInBounds(this Collider[] cldrs, Vector3 point)
    {
        for (int i = 0; i < cldrs.Length; i++)
        {
            //if((cldrs[i].ClosestPoint(point) - point).sqrMagnitude <= 0.01f) //Point in bounds
            if (cldrs[i].bounds.Contains(point))
            {
                return true;
            }
        }
        return false;
    }

    //public static bool IsInCollider(this Collider[] cldrs, Vector3 point)
    //{
    //    for (int i = 0; i < cldrs.Length; i++)
    //    {
    //        if((cldrs[i].ClosestPoint(point) - point).sqrMagnitude <= 0.01f) //Point in bounds
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}
}
