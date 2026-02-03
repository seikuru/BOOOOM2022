using UnityEngine;

public class DebugWireFlame : MonoBehaviour
{
    [SerializeField] float radius = 1f;
    [SerializeField] int segments = 64;
    [SerializeField] Material lineMaterial;

    LineRenderer[] rings;

    void Start()
    {
        if (rings.Length != 3)
            return;

        for (int i = 0; i < 3; i++)
        {
            LineRenderer lr = rings[i];
            lr.useWorldSpace = false;
            lr.material = lineMaterial;
            lr.widthMultiplier = 0.02f;
            lr.loop = true;
            lr.positionCount = segments;   
        }

        MakeRings();
    }

    void MakeRings()
    {
        // 各リングの座標を設定
        Vector3[] points = new Vector3[segments];
        for (int i = 0; i < segments; i++)
        {
            float angle = 2 * Mathf.PI * i / segments;
            points[i] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
        }

        // XY平面
        rings[0].SetPositions(points);

        // YZ平面
        for (int i = 0; i < segments; i++)
            points[i] = new Vector3(0, Mathf.Cos(2 * Mathf.PI * i / segments), Mathf.Sin(2 * Mathf.PI * i / segments)) * radius;
        rings[1].SetPositions(points);

        // XZ平面
        for (int i = 0; i < segments; i++)
            points[i] = new Vector3(Mathf.Cos(2 * Mathf.PI * i / segments), 0, Mathf.Sin(2 * Mathf.PI * i / segments)) * radius;
        rings[2].SetPositions(points);
    }
}
