using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;

//namespace Dweiss
//{
public static class Extensions
{
    public static readonly Color[] colorArray = new Color[] { Color.white, Color.red, Color.blue, Color.green, Color.magenta, Color.cyan, Color.yellow, Color.gray, Color.black };
    public static Color GetColor(int index) { return colorArray[index % colorArray.Length]; }

    public static float NextFloat(this System.Random rnd)
    {
        return (float)rnd.NextDouble();
    }

    public static bool NextBool(this System.Random rnd)
    {
        return rnd.NextDouble() < .5f;
    }
    public static float NextSign(this System.Random rnd)
    {
        return rnd.NextDouble() < .5f ? 1 : -1;
    }
    public static float Range(this System.Random rnd, float max, bool notNegative = true)
    {
        if (notNegative)
            return rnd.NextFloat() * max;
        else
            return rnd.NextFloat() * max * rnd.NextSign();
    }
    public static float Range(this System.Random rnd, float min, float max)
    {
        return rnd.NextFloat() * (max - min) + min;
    }
    public static Vector3 RandomInVector(this System.Random rnd, Vector3 p, bool notNegative = true)
    {
        return new Vector3(rnd.Range(p.x, notNegative), rnd.Range(p.y, notNegative), rnd.Range(p.z, notNegative));
    }

    public static Vector3 RandomInBounds(this System.Random rnd, Bounds b)
    {
        return b.center + rnd.RandomInVector(b.extents, false);
    }
    public static Vector3 RandomInCollider(this System.Random rnd, Collider cldr)
    {
        return rnd.RandomInBounds(cldr.bounds);
    }
    public static Color ChangeAlpha(this Color clr, float alpha)
    {
        return new Color(clr.r, clr.g, clr.b, alpha);
    }

    public static bool HasLayer(this LayerMask mask, int layerValue)
    {
        //Debug.LogFormat("raw: {0} | {1}  log: {2} | {3}", mask.value, layerValue, Mathf.Log(mask.value, 2f), Mathf.Log(layerValue));
        return ((1 << layerValue) & mask.value) != 0;
    }
    public static bool HasLayer(this LayerMask mask, GameObject go)
    {
        return HasLayer(mask, go.layer);
    }

    public static bool HasLayer(this GameObject go, LayerMask mask)
    {
        return HasLayer(mask, go.layer);
    }

    public static Vector3 GetAbs(this Vector3 t)
    {
        return new Vector3(Mathf.Abs(t.x), Mathf.Abs(t.y), Mathf.Abs(t.z));
    }
    public static float SignAngle(this Vector3 referenceForward, Vector3 newDirection, Vector3 referenceRight)
    {
        //Vector3 referenceRight = Vector3.Cross(Vector3.up, referenceForward);

        // Get the angle in degrees between 0 and 180
        float angle = Vector3.Angle(newDirection, referenceForward);
        // Determine if the degree value should be negative.  Here, a positive value
        // from the dot product means that our vector is on the right of the reference vector   
        // whereas a negative value means we're on the left.
        float sign = Mathf.Sign(Vector3.Dot(newDirection, referenceRight));
        return sign * angle;
    }

    private static Regex UpperCaseSplit = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

    public static string SplitUpperCase(this string str, string replace = " ")
    {
        return UpperCaseSplit.Replace(str, replace);
    }


    public static Quaternion Relative(this Quaternion that, Quaternion other)
    {
        return Quaternion.Inverse(that) * other;
    }

    //Exists in linq 
    //public static List<T> ToListCasted<T>(this System.Collections.IEnumerable that)
    //{
    //    var list = new List<T>();
    //    foreach (var t in that)
    //    {
    //        list.Add((T)t);
    //    }
    //    return list;
    //}

    public static Vector3 NearClipPos(this Camera cam, float shift = 0)
    {
        return cam.transform.position + cam.transform.forward * (cam.nearClipPlane + shift);
    }


    private static Ray AngleRayFromCameraOnViewPort(this Camera cam, Vector2 fraction)
    {
        var plane = cam.GetFieldOfViewPoint(1);
        //lR, lL, uR, uL
        var right = plane[0] - plane[1];
        var up = plane[3] - plane[1];
        var forwardToBounds = plane[1] - cam.transform.position;
        var dir = forwardToBounds + fraction.x * right + fraction.y * up;
        return new Ray(cam.transform.position, dir);

    }

    public static Bounds GetFieldOfViewBounds(this Camera cam, float distance)
    {
        var p = GetFieldOfViewPoint(cam, distance);
        var size = p[2] - p[1];
        var center = (p[2] + p[1]) / 2;
        //lR, lL, uR, uL
        return new Bounds(center, size);
    }

    public static Vector3[] GetFieldOfViewPoint(this Camera cam, float distance)
    {
        var trns = cam.transform;
        var pos = trns.position;

        var halfFov = cam.fieldOfView * 0.5f * Mathf.Deg2Rad;
        var aspect = cam.aspect;

        var height = Mathf.Tan(halfFov) * distance;
        var width = height * aspect;

        Vector3 lR, lL, uR, uL;

        lR = pos + trns.forward * distance;
        lR += pos + trns.right * width;
        lR -= pos + trns.up * height;

        lL = pos + trns.forward * distance;
        lL -= pos + trns.right * width;
        lL -= pos + trns.up * height;

        uR = pos + trns.forward * distance;
        uR += pos + trns.right * width;
        uR += pos + trns.up * height;

        uL = pos + trns.forward * distance;
        uL -= pos + trns.right * width;
        uL += pos + trns.up * height;
        return new Vector3[4] { lR, lL, uR, uL };
    }


    public static T GetComponentInTree<T>(this Component that) where T : class
    {
        var t = that.GetComponentInParent<T>();
        if (t == null)
            t = that.GetComponentInChildren<T>();
        return t;
    }
    public static T GetComponentInChildrenOrParents<T>(this Component that) where T : class
    {
        var t = that.GetComponentInParent<T>();
        if (t == null)
            t = that.GetComponentInChildren<T>();
        return t;
    }
    public static T GetComponentInChildrenOnly<T>(this Component that) where T : class
    {
        foreach(Transform c in that.transform)
        {
            var comp = c.GetComponentInChildren<T>();
            if (comp != null)
                return comp;
        }
        return null;
    }

    public static T GetComponentInParentChildrens<T>(this Transform that) where T : class
    {
        return that.parent.GetComponentInChildren<T>();
    }
    public static T GetComponentInSibling<T>(this Component that) where T : class
    {
        return that.transform.parent.GetComponentInChildren<T>();
    }

    public static T GetComponentInAllFamilyTree<T>(this Transform that) where T : class
    {
        var ancestor = that.GetSuperParent();
        return ancestor.GetComponentInChildren<T>();
    }


    public static List<T> GetComponentsInLevelOneChildrens<T>(this MonoBehaviour that) where T : class
    {
        var ret = new List<T>();
        foreach (Transform t in that.transform)
        {
            var item = t.GetComponent<T>();
            if (item != null)
            {
                ret.Add(item);
            }
        }

        return ret;
    }

    public static Transform FindTagInChildren(this Transform go, string tag)
    {
        return RecursiveFindInChildren(go, tag);
    }

    private static Transform RecursiveFindInChildren(Transform trans, string tag)
    {
        if (trans.CompareTag(tag)) return trans;
        foreach (Transform t in trans)
        {
            var tagT = RecursiveFindInChildren(t, tag);
            if (tagT != null) return tagT;
        }
        return null;
    }

    public static List<Transform> FindAllChildrenWithTag(this Transform go, string tag)
    {
        List<Transform> retList = new List<Transform>();
        RecursiveFindAllChildrenWithTag(go, tag, retList);
        return retList;
    }

    private static void RecursiveFindAllChildrenWithTag(Transform trans, string tag, List<Transform> retList)
    {
        if (trans.CompareTag(tag)) retList.Add(trans);
        foreach (Transform t in trans)
        {
            RecursiveFindAllChildrenWithTag(t, tag, retList);
        }
    }

    public static Transform GetSuperParent(this Transform that)
    {
        var ancestor = that;
        while (ancestor.parent != null)
            ancestor = ancestor.parent;

        return ancestor;
    }

    public static T GetComponentInHierarchy<T>(this Component that)
    {
        var t = that.transform;
        T ret = t.GetComponentInChildren<T>();
        if (ret == null)
        {
            ret = t.GetComponentInParent<T>();
        }
        return ret;
    }


    public static Vector3 Copy(this Vector3 v)
    {
        return new Vector3(v.x, v.y, v.z);
    }
    public static Vector3 CopyWithNewX(this Vector3 vector, float value)
    {
        return new Vector3(vector.x, value, vector.z);
    }
    public static Vector3 CopyWithNewY(this Vector3 vector, float value)
    {
        return new Vector3(vector.x, value, vector.z);
    }
    public static Vector3 CopyWithNewZ(this Vector3 vector, float value)
    {
        return new Vector3(vector.x, vector.y, value);
    }

    public static bool InDotRange(this Vector3 that, Vector3 dotV, float min, float max)
    {
        var dot = Vector3.Dot(that, dotV);
        return min <= dot && dot < max;
    }

    public static Vector3 Copy(this Vector3 v, float addX, float addY = 0, float addZ = 0)
    {
        return new Vector3(v.x + addX, v.y + addY, v.z + addZ);
    }

    public static Bounds TotalMeshBounds(this GameObject g, bool invisibleAsWell = false)
    {
        var meshes = g.GetComponentsInChildren<Renderer>(invisibleAsWell);
        if (meshes.Length == 0)
            return new Bounds();

        var bounds = new Bounds(meshes[0].bounds.center, meshes[0].bounds.size);
        for (int i = 1; i < meshes.Length; ++i) bounds.Encapsulate(meshes[i].bounds);
        return bounds;
    }

    public static Bounds TotalColliderBounds(this GameObject g, bool invisibleAsWell = false)
    {
        var cldrs = g.GetComponentsInChildren<Collider>(invisibleAsWell);
        if (cldrs.Length == 0)
            return new Bounds();

        var bounds = new Bounds(cldrs[0].bounds.center, cldrs[0].bounds.size);
        for (int i = 1; i < cldrs.Length; ++i) bounds.Encapsulate(cldrs[i].bounds);
        return bounds;
    }

    public static Vector3 RotateAroundPivot(this Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        return dir + pivot; // calculate rotated point

    }

    public static Quaternion CombineByFactor(this Quaternion o, Quaternion that, Vector4 f)
    {
        return new Quaternion((that.x * (1 - f.x)) + (o.x * f.x), (that.y * (1 - f.y)) + (o.y * f.y), (that.z * (1 - f.z)) + (o.z * f.z), (that.w * (1 - f.w)) + (o.w * f.w));

    }


    public static Vector3 CombineByFactor(this Vector3 o, Vector3 that, Vector3 f)
    {
        return new Vector3((that.x * (1 - f.x)) + (o.x * f.x), (that.y * (1 - f.y)) + (o.y * f.y), (that.z * (1 - f.z)) + (o.z * f.z));

    }

    public static Vector3 PointMul(this Vector3 that, Vector3 o)
    {
        return new Vector3(that.x * o.x, that.y * o.y, that.z * o.z);

    }
    public static Vector3 PointMul(this Vector3 that, float x, float y, float z)
    {
        return new Vector3(that.x * x, that.y * y, that.z * z);

    }
    public static Vector3 PointDiv(this Vector3 that, Vector3 o)
    {
        return new Vector3(that.x / o.x, that.y / o.y, that.z / o.z);
    }
    public static Vector3 PointDiv(this Vector3 that, float x, float y, float z)
    {
        return new Vector3(that.x / x, that.y / y, that.z / z);
    }

    public static bool IsInfinity(this Vector3 v)
    {
        return Mathf.Infinity == Math.Abs(v.sqrMagnitude);
    }

    public static bool IsNaN(this Vector3 v)
    {
        return float.IsNaN(v.x) || float.IsNaN(v.y) || float.IsNaN(v.z);
    }

    public static void Destroy(this UnityEngine.Object obj)
    {
        Destroy(obj);
    }


    public static string ToString(this Vector3 that, string format = "###,##0.##")
    {
        return String.Format("({0}, {1}, {2})", that.x.ToString(format), that.y.ToString(format), that.z.ToString(format));
    }

    public static string ToMiliString(this Vector3 that)
    {
        return String.Format("({0}, {1}, {2})", that.x.ToMiliStr(), that.y.ToMiliStr(), that.z.ToMiliStr());
    }
    public static string ToMiliStr(this Vector3 that)
    {
        return String.Format("({0}, {1}, {2})", that.x.ToMiliStr(), that.y.ToMiliStr(), that.z.ToMiliStr());
    }
    public static string ToMiliString(this Quaternion that)
    {
        return String.Format("({0}, {1}, {2}, {3})", that.x.ToMiliStr(), that.y.ToMiliStr(), that.z.ToMiliStr(), that.w.ToMiliStr());
    }
    public static string ToMiliStr(this Quaternion that)
    {
        return String.Format("({0}, {1}, {2}, {3})", that.x.ToMiliStr(), that.y.ToMiliStr(), that.z.ToMiliStr(), that.w.ToMiliStr());
    }
    public static string ToMiliString(this Vector2 that)
    {
        return String.Format("({0}, {1})", that.x.ToMiliStr(), that.y.ToMiliStr());
    }
    public static string ToMiliStr(this Vector2 that)
    {
        return String.Format("({0}, {1})", that.x.ToMiliStr(), that.y.ToMiliStr());
    }

    public static string ToMiliStr(this float that)
    {
        return that.ToString("###,##0.###");
    }
    public static string ToSingleDotStr(this float that)
    {
        return that.ToString("###,##0.#");
    }

    public static string ToCommaString<T>(this IEnumerable<T> list, bool showCount = true)
    {
        return (showCount ? list.Count() + ". " : "") +
            String.Join(",", list.Select(n => n == null ? "Null" : n.ToString()).ToArray());
    }
    public static string ToCommaString<T>(this IEnumerable<T> list, System.Func<T, string> toString, bool showCount = true)
    {
        return (showCount ? list.Count() + ". " : "") +
            String.Join(",", list.Select(n => n == null ? "Null" : toString(n)).ToArray());
    }

    public static string DictionaryToString<K, V>(this IDictionary<K, V> map)
    {
        return String.Join(" , ", map.Select(n => "" + n.Key + "->" + n.Value).ToArray());
    }


    public static string DictionaryToString<K, V>(this IDictionary<K, V[]> map)
    {
        return String.Join(" , ", map.Select(n => "" + n.Key + "->" + n.Value.ToCommaString()).ToArray());
    }

    public static float Median(this IEnumerable<float> list)
    {
        var count = list.Count();
        var ordered = list.OrderBy(p => p);
        float median = ordered.ElementAt(count / 2) + ordered.ElementAt((count - 1) / 2);
        return median /= 2;
    }

    public static Vector3 AxisMedian(this IEnumerable<Vector3> list)
    {
        var xMedian = list.Select(p => p.x).Median();
        var yMedian = list.Select(p => p.y).Median();
        var zMedian = list.Select(p => p.z).Median();
        return new Vector3(xMedian, yMedian, zMedian);
    }

    public static Vector3 Clone(this Vector3 that)
    {
        return new Vector3(that.x, that.y, that.z);
    }

    public static void Push<T>(this IList<T> list, T newItem, int index)
    {
        //Bubble up
        for (int i = index; i < list.Count; i++)
        {
            var temp = list[i];
            list[i] = newItem;
            newItem = temp;
        }
        //Add last item
        list.Add(newItem);
    }

    public static T MinItem<T>(this IList<T> list, System.Comparison<T> cmpr)
    {
        T ret = list[0];
        for (int i = 0; i < list.Count; i++)
        {
            if (cmpr(ret, list[i]) > 0)
            {
                ret = list[i];
            }
        }
        return ret;
    }


    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = (int)UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }


    public static Vector3 RandomInBox(Vector3 boxScale)
    {
        var width = UnityEngine.Random.Range(-boxScale.x, boxScale.x);
        var height = UnityEngine.Random.Range(-boxScale.y, boxScale.y);
        var depth = UnityEngine.Random.Range(-boxScale.z, boxScale.z);
        return new Vector3(width, height, depth);
    }
    public static Vector3 RandomInBox(Bounds bounds)
    {
        var val = RandomInBox(bounds.extents);
        var res = bounds.center + val;
        return res;
    }

    public static bool InRange(this float that, float min, float max)
    {
        return min <= that && that < max;
    }
    public static bool InRange(this Vector3 that, Vector3 min, Vector3 max)
    {
        return that.x.InRange(min.x, max.x) && that.y.InRange(min.y, max.y) && that.z.InRange(min.z, max.z);
    }
    public static string ToFormatedHours(this TimeSpan t)
    {
        return string.Format("{0}:{1}:{2}",
       ((int)t.TotalHours), (int)t.Minutes, (int)t.Seconds);
    }

    public static string[] SplitOnce(this string str, char divider)
    {
        var index = str.IndexOf(divider);
        return new string[] { str.Substring(0, index), str.Substring(index) };
    }
    public static string[] SplitOnceNoSeperator(this string str, char divider)
    {
        var index = str.IndexOf(divider);
        return index >= 0 ?new string[] { str.Substring(0, index), str.Substring(index+1) } : new string[] {str };
    }

    public static string GetOnlyPrefix(this string str, string seperator, bool last = false)
    {
        var suffixIndex = last ? str.LastIndexOf(seperator) : str.IndexOf(seperator);
        return suffixIndex < 0 ? str : str.Substring(0, suffixIndex);
    }
    public static string GetOnlySuffix(this string str, string seperator, bool last = false)
    {
        var suffixIndex = last ? str.LastIndexOf(seperator) : str.IndexOf(seperator);
        return suffixIndex < 0 ? "" : str.Substring(suffixIndex + seperator.Length);
    }
}
//}