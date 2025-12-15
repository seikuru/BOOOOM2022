using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumLineConrtrol : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    [SerializeField] private AudioSource source = null;

    private const int FFT_RESOLUSION = 256;

    [SerializeField, Range(1, FFT_RESOLUSION)] private int visible = 128;
    [SerializeField] private float waveLength = 20.0f;
    [SerializeField] private float hight = 10.0f;
    [SerializeField] private int bias = 1000;

    private float[] spectrum = null;
    private float[] data = null;
    private int sampleStep = 0;
    private Vector3[] points = null;
    private Vector3[] samples = null;

    void Start()
    {
        // スペクトラム用の配列の準備
        spectrum = new float[FFT_RESOLUSION];

        // 音量用データの配列
        data = new float[source.clip.channels * source.clip.samples];
        //Debug.Log("samples : " + source.clip.samples);
        //Debug.Log("frequency : " + source.clip.frequency);
        //Debug.Log("ratio : " + source.clip.samples / source.clip.frequency);
        //Debug.Log("channels : " + source.clip.channels);
        source.clip.GetData(data, 0); // 曲全部分のデータをここで格納
        var fps = 1f / Time.fixedDeltaTime;
        sampleStep = (int)(source.clip.channels * source.clip.frequency / fps);

        //line.positionCount = data.Length/100;
        //Vector3[] p = new Vector3[data.Length];
        //for(int i = 0; i < p.Length; i+=100)
        //{
        //    p[i] = new Vector3(i / 3000, data[i] * hight, 0);
        //}
        //line.SetPositions(p);

        // ラインレンダラーの制御用配列
        points = new Vector3[FFT_RESOLUSION];
        var step = waveLength / spectrum.Length;
        for (int i = 0; i < FFT_RESOLUSION; i++)
        {
            points[i] = new Vector3(step * i, 0, 0); // 最初は直線の状態
        }

        samples = new Vector3[sampleStep];
        step = waveLength / samples.Length;
        for(int i = 0; i < samples.Length; i++)
        {
            samples[i] = new Vector3(step * i, 0, 0);
        }
    }

    void FixedUpdate()
    {
        // SpectrumRender();
        WaveRender();
    }

    private void SpectrumRender()
    {
        source.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        var xStep = waveLength / spectrum.Length;

        float sum = 0;
        float[] buf = new float[5];
        int bi = 0;
        for(int i = 0; i < 5; i++)
        {
            buf[i] = 0;
        }

        for(int i = 0; i < FFT_RESOLUSION; i++)
        {
            sum -= buf[bi];
            buf[bi] = spectrum[i];
            sum += buf[bi++];
            if (bi >= 5) bi = 0;

            int isActive = visible >= i ? 1 : 0;

            var y = (sum * hight / 5) * isActive;
            var x = xStep * i;

            points[i] = new Vector3(x, y, 0);
        }

        if(points == null)
        {
            return;
        }
        line.positionCount = points.Length;
        line.SetPositions(points);
    }

    private void WaveRender()
    {
        int startIndex = source.timeSamples + bias;
        // Debug.Log(startIndex / source.clip.frequency);
        int endIndex = Mathf.Min(startIndex + sampleStep, data.Length);

        int amount = Mathf.Max(endIndex - startIndex, 1);
        float xStep = waveLength / amount;

        for(int i = 0; i < amount; i += source.clip.channels)
        {
            var x = xStep * i;
            var y = data[startIndex + i / source.clip.channels] * hight;
            samples[i] = new Vector3(x, y, 0);
        }

        line.positionCount = amount;
        line.SetPositions(samples);
    }
}
