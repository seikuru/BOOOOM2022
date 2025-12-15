using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCulcurator : MonoBehaviour
{
    enum FFT_resolusion
    {
        _8192 = 8192, _4096 = 4096, _2048 = 2048, _1024 = 1024, _512 = 512, _256 = 256, _128 = 128, _64 = 64
    }

    // アタッチされた音源のデータを返すクラス
    [SerializeField] List<AudioSource> sources;
    [SerializeField] FFT_resolusion fft_res = FFT_resolusion._256;

    private AudioClip[] clips = default;

    // オーディオデータ用
    private List<float[]> data = new List<float[]>();

    void Start()
    {
        clips = new AudioClip[sources.Count];
        for(int i = 0; i < sources.Count; i++)
        {
            clips[i] = sources[i].clip;
        }

        for(int i = 0; i < sources.Count; i++)
        {
            // data の初期化
            data.Add(new float[clips[i].channels * clips[i].samples]);
            clips[i].GetData(data[i], 0);
        }
    }

    void Update()
    {
        
    }

    /// <summary>
    /// FFT Resolusion の値を返す
    /// </summary>
    /// <returns>int FFT_resolusion</returns>
    public int GetFFTResolusion()
    {
        return (int)fft_res;
    }

    /// <summary>
    /// 現在の時間から決められた長さ分の波形データを渡す関数
    /// </summary>
    /// <param name="target">この配列に値が格納される、長さは dataLength</param>
    /// <param name="dataLength">配列の長さ</param>
    public void GetWaveData(ref float[] target, int dataLength)
    {
        target = new float[dataLength];

        for(int i = 0; i < sources.Count; i++)
        {
            float strength = sources[i].mute == true ? 0 : 1;
            strength *= sources[i].volume;

            int startIndex = sources[i].timeSamples;

            for(int j = 0; j < dataLength; j++)
            {
                target[j] += data[i][startIndex + j] * strength / sources.Count;
            }
        }
    }

    /// <summary>
    /// 現在から0.1秒後までの音量の和を返す関数
    /// </summary>
    /// <returns>float 音量</returns>
    public float GetCurrentData()
    {
        float res = 0;

        for(int i = 0; i < sources.Count; i++)
        {
            int currentIndex = sources[i].timeSamples;
            int sampleNum = clips[i].frequency / 10;
            sampleNum = 1;
            float sum = 0;

            float strength = sources[i].mute == true ? 0 : 1;
            strength *= sources[i].volume;

            if (strength <= 0.01f) continue; // 十分に小さかったらスキップ

            for(int j = 0; j < sampleNum; j++)
            {
                sum += Mathf.Abs(data[i][currentIndex + j]);
            }

            res += sum * strength;
        }

        return res;
    }

    /// <summary>
    /// 現在のスペクトルを渡す関数
    /// </summary>
    /// <param name="target">この配列に値が格納される、長さは fft_res</param>
    public void GetSpectrum(ref float[] target)
    {
        float[] spec = new float[(int)fft_res];
        target = new float[(int)fft_res];
        for(int i = 0; i < sources.Count; i++)
        {
            float strength = sources[i].mute == true ? 0 : 1;
            strength *= sources[i].volume;

            sources[i].GetSpectrumData(spec, 0, FFTWindow.Rectangular);

            for(int j = 0; j < (int)fft_res; j++)
            {
                target[j] += spec[j] * strength / sources.Count;
            }
        }
    }
}
