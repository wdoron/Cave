using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UvRotate : MonoBehaviour
{
    private readonly Vector2 ShiftUvFactor = Vector2.one * .5f;

    private Mesh mesh;
    private Vector2[] startUv;
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        startUv = mesh.uv.ToArray();

        Rotate();

    }

    private Vector2 UvWithRotate(Vector2 uv)
    {
        var circle = UvToCircle(uv);
        var angle = CirclePosToAngle(circle);
        var rotated = AngleWithRotationToUvPos(angle);
        return rotated;
    }
    private Vector2 UvToCircle(Vector2 uv)
    {
        return (uv - ShiftUvFactor) *2;
    }

    private float CirclePosToAngle(Vector2 cPos)
    {
        return Mathf.Atan2(cPos.y, cPos.x);
    }

    public float radPerSec;
    private Vector2 AngleToUvPos(float angle)
    {
        return new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * .5f + ShiftUvFactor; ;
    }

    private Vector2 AngleWithRotationToUvPos(float angle)
    {
        return AngleToUvPos( angle + radPerSec * Time.time);

    }
    private void PrintUv()
    {
        Vector2[] oldUv = mesh.uv;
        System.String str = "";
        for (int i = 0; i < oldUv.Length; i++)
        {
            str += string.Format("{0} = {1}\n", i, oldUv[i].ToMiliString());
        }
        Debug.LogFormat(str);
    }

    private void Rotate()
    {
        Vector2[] newUv = new Vector2[mesh.uv.Length];
        for (int i = 0; i < startUv.Length; i++)
        {
             newUv[i] = UvWithRotate(startUv[i]);
        }
     
        mesh.uv = newUv;
    }
    // Update is called once per frame
    void Update()
    {
        Rotate();
        //PrintUv();
    }
}
