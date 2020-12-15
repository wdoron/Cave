// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[RequireComponent (typeof(LineRenderer))]
//[AddComponentMenu ("LineRenderer")]
public class AimingLine : MonoBehaviour
{


    public GameObject linePrefab;


    public int numOfFrameForLine = 10;
    private int _currentFrameForLine = 0;
    private float _objShiftLen;

    private List<GameObject> _lineBag = new List<GameObject>(100);

    private GameObject _lineGroup;

    [SerializeField] private Transform source;
    [SerializeField] private Transform target;

    void Start()
    {
        if (source == null)
        {
            source = transform;
        }

        _lineGroup = new GameObject();
        _lineGroup.name = "lineGroup";

        _objShiftLen = linePrefab.transform.localScale.z / (numOfFrameForLine / 2);

    }

    private void OnEnable()
    {
        if(_lineGroup != null)
            _lineGroup.SetActive(true);
    }

    private void onDisable()
    {
        if (_lineGroup != null)
            _lineGroup.SetActive(false);
    }

    private GameObject GetOneLine(int i, Vector3 originPos, Quaternion rot)
    {
        if (i == _lineBag.Count)
        {

            _lineBag.Add((GameObject)Instantiate(linePrefab));
            //Debug.Log("lineBag: i / count " + i + "/" + _lineBag.Count);
            _lineBag[i].transform.position = new Vector3(-1000, -1000, -1000);
            _lineBag[i].transform.parent = _lineGroup.transform;
        }
        //Debug.Log("i / count " + i + "/" + _lineBag.Count);
        var ret = _lineBag[i];
        ret.transform.position = originPos;
        ret.transform.rotation = rot;
        return ret;
    }




    private void CreateMultipleOnLineOfSight(Vector3 start, Vector3 end, Vector3 dir, Quaternion rot)
    {

        var distance = Mathf.Abs((start - end).magnitude);

        var oneLineLength = linePrefab.transform.localScale.z;

        //var dir= (end - start).normalized;
        var nextObjPos = start + oneLineLength / 2 * dir;

        int counter = 0;
        for (var totalLen = oneLineLength; totalLen < distance; totalLen = totalLen + 2 * oneLineLength)
        {

            GetOneLine(counter, nextObjPos, rot);
            nextObjPos = nextObjPos + 2 * oneLineLength * dir;
            counter++;
        }

        for (; counter < _lineBag.Count; ++counter)
        {
            if (_lineBag[counter].transform.position == new Vector3(-1000, -1000, -1000))
            {
                break;
            }
            GetOneLine(counter, new Vector3(-1000, -1000, -1000), rot);
        }

    }

    private void OnDisable()
    {
        for (var i=0; i < _lineBag.Count; ++i)
        {
            if (_lineBag[i].transform.position == new Vector3(-1000, -1000, -1000))
            {
                break;
            }
            GetOneLine(i, new Vector3(-1000, -1000, -1000), Quaternion.identity);
        }
    }


    private void Update()
    {
        if(target!= null)
        {
            DrawLineOfSight(source.position, target.position);
        }
    }

    public void DrawLineOfSight(Vector3 lineStart, Vector3 lineEnd)
    {

        if ((lineStart - lineEnd).magnitude < 0.001f)
        {
            CreateMultipleOnLineOfSight(Vector3.zero, Vector3.zero, Vector3.zero, Quaternion.LookRotation(Vector3.forward));

            _currentFrameForLine = (_currentFrameForLine + 1) % numOfFrameForLine;
            return;
        }
        var relativePos = lineEnd - lineStart;
        var rotationAngle = Quaternion.LookRotation(relativePos);
        var rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), rotationAngle, 1);

        var dir = (lineEnd - lineStart).normalized;
        var shift = (_objShiftLen * _currentFrameForLine) * dir;

        CreateMultipleOnLineOfSight(lineStart + shift, lineEnd, dir, rotation);

        _currentFrameForLine = (_currentFrameForLine + 1) % numOfFrameForLine;

    }


}

