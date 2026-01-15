using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLineRender : MonoBehaviour
{
    // LineRenderer を直線状に描画する
    [SerializeField] private AudioCulcurator AC;
    [SerializeField] private LineRenderer line;

    [SerializeField] private float lineLength = 20.0f;
    [SerializeField] private float height = 40.0f;
    [SerializeField, Range(0.0f, 100.0f)] private float visible = 100;

    private float[] spectrum = null; // spectrum のデータを受け取る配列
    private Vector3[] points = null; // line renderer の頂点位置
    private float xStep; // x座標の間隔

    void LineSetUp(int Resolusion)
    {
        // local position で指定する
        line.useWorldSpace = false;

        line.positionCount = Resolusion;
        points = new Vector3[Resolusion];
        xStep = lineLength / Resolusion;
    }

    void Update()
    {
        int resolusion = AC.GetFFTResolusion();

        if (points == null)
            LineSetUp(resolusion);

        AC.GetSpectrum(ref spectrum);

        for(int i = 0; i < points.Length; i++)
        {
            int isVisible = i <= resolusion * visible/100 ? 1 : 0;

            float x = xStep * i;
            float y = spectrum[i] * height * isVisible * i;
            points[i] = new Vector3(x, y, 0);
        }

        line.SetPositions(points);
    }
}
