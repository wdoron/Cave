
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeByPosition : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Bounds")]
    public Transform startArea;
    public Transform endArea;

    [Header("Algorithem")]
    public FactorBy factorBy;
    public bool dontGoBack;

    [Header("Event")]
    public Dweiss.EventFloat onFactorChanged;

    private float lastMaxV = 0;

    public enum FactorBy
    {
        MaxAtCenter, MaxAtEnd, MaxAtStart
    }

    protected void Reset() {
        player = Camera.main.transform;
    }
    private static float InverseLerp(Vector3 a, Vector3 b, Vector3 value) {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }


    private void OnDrawGizmosSelected() {
        if (player && endArea && startArea) {
            var v = GetFactor();
            Debug.DrawRay(startArea.position, (endArea.position - startArea.position) * v, v < 0 || v > 1 ? Color.black : Color.white);
        }
    }

    [ContextMenu("ResetLastMaxV")]
    private void ResetLastMaxV() => lastMaxV = 0;

    private float GetFactor() {
        var projectedPoint = Vector3.Project(player.position, endArea.position - startArea.position);
        var v = InverseLerp(startArea.position, endArea.position, projectedPoint);
        v = Mathf.Clamp(v, 0, 1);

        if (lastMaxV < v) {
            lastMaxV = v;
        }

        if (dontGoBack) v =lastMaxV;

        switch (factorBy) {
            case FactorBy.MaxAtCenter: v = Mathf.Abs(v * 2 - 1); break;
            case FactorBy.MaxAtEnd: v = 1 - v; break;
            case FactorBy.MaxAtStart: break;
            default: break;
        }

        return v;
    }


    protected virtual void OnValueChange(float v) {

    }
    protected void Update() {
        var v = GetFactor();
        OnValueChange(v);
        onFactorChanged.Invoke(v);

    }
}
