using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dweiss
{
    public class DebugExt
    {

        public static void DrawX(Vector3 arrowStart, Vector3 arrowHead, Color c, float time, float arrowLength = .2f)
        {
            var v = arrowHead - arrowStart;
            var v2 = Vector3.up;
            var v3 = Vector3.Cross(v2, v);
            if (v3 == Vector3.zero)
            {
                v2 = Vector3.forward;
                v3 = Vector3.Cross(v2, v);
            }
            v3.Normalize();
            Debug.DrawLine(arrowHead + v2 * arrowLength, arrowHead - v2 * arrowLength, c, time);
            Debug.DrawLine(arrowHead + v3 * arrowLength, arrowHead - v3 * arrowLength, c, time);
        }


        public static void DrawLines(Vector3 origin, Color c, float time, params Vector3[] ends)
        {
            for(int i=0; i < ends.Length; ++i)
            {
                Debug.DrawLine(origin, ends[i], new Color(
                    System.MathfExt.Fraction(c.r + i * .3333f),
                    System.MathfExt.Fraction(c.g + i * .1111f),
                    System.MathfExt.Fraction(c.b + i * .7777f)), time);
            }
        }
        public static void DrawRays(Vector3 origin, Color c, float time, params Vector3[] dirs)
        {
            for (int i = 0; i < dirs.Length; ++i)
            {
                Debug.DrawRay(origin, dirs[i], new Color(
                    System.MathfExt.Fraction(c.r + i * .3333f),
                    System.MathfExt.Fraction(c.g + i * .1111f),
                    System.MathfExt.Fraction(c.b + i * .7777f)), time);
            }
        }

        public static void DrawArrowLine(Vector3 start, Vector3 end, Color c, float time, float arrowLength = .2f)
        {
            Debug.DrawLine(start,end,c, time);
            DrawX(start, end, c, time, arrowLength);
        }
        public static void DrawBox(Vector3 center, Vector3 extends, Color c, 
            float time)
        {
            var points = new Vector3[]
            {
                center + extends.PointMul(1, 1, 1)
                ,center + extends.PointMul(1, 1, -1)
                ,center + extends.PointMul(1, -1, -1)
                ,center + extends.PointMul(1, -1, 1)

                ,center + extends.PointMul(-1, -1, 1)
                ,center + extends.PointMul(-1, -1, -1)
                ,center + extends.PointMul(-1, 1, -1)
                ,center + extends.PointMul(-1, 1, 1)


                ,center + extends.PointMul(-1, -1, -1)
                ,center + extends.PointMul(1, -1, -1)

                ,center + extends.PointMul(-1, 1, -1)
                ,center + extends.PointMul(1, 1, -1)

                ,center + extends.PointMul(1, 1, 1)
                ,center + extends.PointMul(1, -1, 1)

                ,center + extends.PointMul(-1, -1, 1)
                ,center + extends.PointMul(-1, 1, 1)
            };
            for(int i=0; i < points.Length; ++i)
            {
                Debug.DrawLine(points[i], points[(i+1)% points.Length], c, time);

            }
        }

        public static void DrawArrowRay(Vector3 start, Vector3 v, Color c, float time, float arrowLength = .2f)
        {
            Debug.DrawRay(start, v, c, time);
            var end = start + v;
            DrawX(start, end, c, time, arrowLength);
        }
    }
}
