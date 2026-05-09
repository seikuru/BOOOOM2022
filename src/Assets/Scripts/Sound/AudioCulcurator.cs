using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
class AudioSourceGroup
{
    public MusicType musicType;
    public List<AudioSource> audioSources;
}

public class AudioCulcurator : MonoBehaviour
{
    enum FFT_resolusion
    {
        _8192 = 8192, _4096 = 4096, _2048 = 2048, _1024 = 1024, _512 = 512, _256 = 256, _128 = 128, _64 = 64
    }

    // アタッチされた音源のデータを返すクラス
    [SerializeField] List<AudioSourceGroup> sources;
    [SerializeField] FFT_resolusion fft_res = FFT_resolusion._256;

    // musicType ごとのオーディオクリップ格納用
    public Dictionary<MusicType, AudioClip[]> musicClips = new Dictionary<MusicType, AudioClip[]>();
    // musicType ごとのオーディオデータ格納用
    private Dictionary<MusicType, List<float[]>> musicData = new Dictionary<MusicType, List<float[]>>();


    private void Start()
    {
        
    }

    void OnEnable()
    {
        musicClips = new Dictionary<MusicType, AudioClip[]>();
        musicData = new Dictionary<MusicType, List<float[]>>();
        for (int i = 0; i < sources.Count; i++) // musicType ごとにループ
        {
            AudioClip[] audioClips = new AudioClip[sources[i].audioSources.Count];
            for (int j = 0; j < sources[i].audioSources.Count; j++)
            {
                // AudioClip を取得
                audioClips[j] = sources[i].audioSources[j].clip;
            }
            musicClips[sources[i].musicType] = audioClips;
            // Debug.Log("Loaded MusicType: " + sources[i].musicType.ToString() + " with " + musicClips[sources[i].musicType][0].name);

            //List<float[]> audioDataList = new List<float[]>();
            musicData.Add(sources[i].musicType, new List<float[]>());
            for (int j = 0; j < audioClips.Length; j++)
            {
                
                AudioClip clip = audioClips[j];
                //float[] audioData = new float[clip.samples * clip.channels];
                //clip.GetData(audioData, 0);
                //audioDataList.Add(audioData);

                musicData[sources[i].musicType].Add(new float[clip.samples * clip.channels]);
                //bool isDone = clip.GetData(audioData, 0);

                //Debug.Log("GetData status : " + isDone);

                /*
                AudioClip clip = audioClips[j];
                float[] audioData = new float[clip.samples * clip.channels];
                //clip.GetData(audioData, 0);
                //audioDataList.Add(audioData);

                musicData[sources[i].musicType].Add(new float[clip.samples * clip.channels]);
                bool isDone = clip.GetData(audioData, 0);

                Debug.Log("GetData status : " + isDone);
                */
                //Debug.Log("  Loaded AudioClip: " + clip.name + " , samples: " + clip.samples + " , channels: " + clip.channels);
                //Debug.Log("    Sample Data Check: " + musicData[sources[i].musicType][j][480000]);
            }
            // musicData[sources[i].musicType] = audioDataList;
        }

        Debug.Log("music type : " + sources[0].musicType.ToString() + " , random data : " + musicData[sources[0].musicType][0][480000]);
    }

    /// <summary>
    /// FFT Resolusion の値を返す
    /// </summary>
    /// <returns> int FFT_resolusion </returns>
    public int GetFFTResolusion()
    {
        return (int)fft_res;
    }

    /// <summary>
    /// 現在の時間から決められた長さ分の波形データを渡す関数
    /// </summary>
    /// <param name="target"> この配列に値が格納される、長さは dataLength </param>
    /// <param name="dataLength"> 配列の長さ </param>
    public void GetWaveData(ref float[] target, int dataLength)
    {
        target = new float[dataLength];

        for(int i = 0; i < sources.Count; i++) // musicType ごとにループ
        {
            if (sources[i].audioSources == null) continue;

            int startIndex = sources[i].audioSources[0].timeSamples;
            for (int j = 0; j < sources[i].audioSources.Count; j++) // 各 AudioSource ごとにループ
            {
                float strength = sources[i].audioSources[j].mute == true ? 0 : 1;
                strength *= sources[i].audioSources[j].volume;

                for (int k = 0; k < dataLength; k++)
                {
                    target[k] += musicData[sources[i].musicType][j][startIndex + k] * strength / sources[i].audioSources.Count;
                }
            }
        }
    }

    /// <summary>
    /// 指定された MusicType の現在の時間から決められた長さ分の波形データを渡す関数
    /// </summary>
    /// <param name="type"> 欲しい波形データの MusicType </param>
    /// <param name="target"> この配列に値が格納される、長さは dataLength </param>
    /// <param name="dataLength"> 配列の長さ </param>
    public void GetWaveData(MusicType type, ref float[] target, int dataLength)
    {
        target = new float[dataLength];

        int sourceIndex = -1;
        for(int i = 0; i < sources.Count; i++)
        {
            if(sources[i].musicType == type)
            {
                sourceIndex = i;
                break;
            }
        }
        if(sourceIndex == -1 || sources[sourceIndex].audioSources == null)
        {
            Debug.LogWarning("指定された MusicType の AudioSource が見つかりません: " + type);
            return;
        }

        int startIndex = sources[sourceIndex].audioSources[0].timeSamples;
        for(int j = 0; j < sources[sourceIndex].audioSources.Count; j++)
        {
            float strength = sources[sourceIndex].audioSources[j].mute == true ? 0 : 1;
            strength *= sources[sourceIndex].audioSources[j].volume;

            for(int k = 0; k < dataLength; k++)
            {
                target[k] += musicData[type][j][startIndex + k] * strength / sources[sourceIndex].audioSources.Count;
            }
        }
    }

    /// <summary>
    /// 現在から0.1秒後までの音量の和を返す関数
    /// </summary>
    /// <returns> float 音量 </returns>
    //public float GetCurrentData()
    //{
    //    float res = 0;

    //    for(int i = 0; i < sources.Count; i++)
    //    {
    //        if(sources[i].audioSources == null || sources[i].audioSources.Count <= 0) continue;

    //        int currentIndex = sources[i].audioSources[0].timeSamples;
    //        int sampleNum = sources[i].audioSources[0].clip.frequency / 10;
    //        float sum = 0f;

    //        for(int j = 0; j< sources[i].audioSources.Count; j++)
    //        {
    //            float strength = sources[i].audioSources[j].mute == true ? 0 : 1;
    //            strength *= sources[i].audioSources[j].volume;

    //            if (strength <= 0.01f) continue; // 十分に小さかったらスキップ

    //            for (int k = 0; k < sampleNum; k++)
    //            {
    //                sum += Mathf.Abs(musicData[sources[i].musicType][j][currentIndex + k]);
    //                // Debug.Log("In GetCurrentData : " + sum);
    //            }
    //            res += sum * strength / sources[i].audioSources.Count;
    //        }

    //        // Debug.Log("MusicType: " + sources[i].musicType + ", Volume: " + res);
    //    }

    //    return res;
    //}

    /// <summary>
    /// 指定した MusicType の現在から0.1秒後までの音量の和を返す関数
    /// </summary>
    /// <param name="type"> 欲しい MusicType </param>
    /// <returns> float 音量 </returns>
    //public float GetCurrentData(MusicType type)
    //{
    //    float res = 0;
    //    int sourceIndex = -1;
    //    for (int i = 0; i < sources.Count; i++)
    //    {
    //        if (sources[i].musicType == type)
    //        {
    //            sourceIndex = i;
    //            break;
    //        }
    //    }

    //    if (sourceIndex == -1 || sources[sourceIndex].audioSources == null)
    //    {
    //        Debug.LogWarning("指定された MusicType の AudioSource が見つかりません: " + type);
    //        return 0f;
    //    }

    //    int currentIndex = sources[sourceIndex].audioSources[0].timeSamples;
    //    int sampleNum = sources[sourceIndex].audioSources[0].clip.frequency / 10;
    //    float sum = 0f;
    //    for (int j = 0; j < sources[sourceIndex].audioSources.Count; j++)
    //    {
    //        float strength = sources[sourceIndex].audioSources[j].mute == true ? 0 : 1;
    //        strength *= sources[sourceIndex].audioSources[j].volume;
    //        if (strength <= 0.01f) continue; // 十分に小さかったらスキップ
    //        for (int k = 0; k < sampleNum; k++)
    //        {
    //            sum += Mathf.Abs(musicData[type][j][currentIndex + k]);
    //        }
    //        res += sum * strength / sources[sourceIndex].audioSources.Count;
    //    }

    //    return 0f;
    //}

    /// <summary>
    /// 現在の音量の和を返す関数
    /// </summary>
    /// <returns> float 音量 </returns>
    public float GetOutputData()
    {
        float res = 0f;

        for(int i = 0; i < sources.Count; i++)
        {
            if (sources[i].audioSources == null) continue;

            int duration = sources[i].audioSources[0].clip.frequency / 10;

            for (int j = 0; j < sources[i].audioSources.Count; j++)
            {
                float strength = sources[i].audioSources[j].mute == true ? 0 : 1;
                strength *= sources[i].audioSources[j].volume;

                float[] arr = new float[duration];

                sources[i].audioSources[j].GetOutputData(arr, 0);
                res += arr.Select(x => x*x).Sum() * strength / (duration * sources[i].audioSources.Count);
            }
        }

        return res;
    }

    /// <summary>
    /// 指定した MusicType の現在の音量の和を返す関数
    /// </summary>
    /// <param name="type"> 欲しい MusicType </param>
    /// <returns> float 音量 </returns>
    public float GetOutputData(MusicType type)
    {
        float res = 0f;
        int sourceIndex = -1;
        for (int i = 0; i < sources.Count; i++)
        {
            if (sources[i].musicType == type)
            {
                sourceIndex = i;
                break;
            }
        }
        if (sourceIndex == -1 || sources[sourceIndex].audioSources == null)
        {
            Debug.LogWarning("指定された MusicType の AudioSource が見つかりません: " + type);
            return 0f;
        }

        int duration = sources[sourceIndex].audioSources[0].clip.frequency / 10;
        for (int j = 0; j < sources[sourceIndex].audioSources.Count; j++)
        {
            float strength = sources[sourceIndex].audioSources[j].mute == true ? 0 : 1;
            strength *= sources[sourceIndex].audioSources[j].volume;

            float[] arr = new float[duration];

            sources[sourceIndex].audioSources[j].GetOutputData(arr, 0);
            res += arr.Select(x => x * x).Sum() * strength / (duration * sources[sourceIndex].audioSources.Count);
        }
        return res;
    }

    /// <summary>
    /// 現在のスペクトルを渡す関数
    /// </summary>
    /// <param name="target"> この配列に値が格納される、長さは fft_res </param>
    public void GetSpectrum(ref float[] target)
    {
        float[] spec = new float[(int)fft_res];
        target = new float[(int)fft_res];

        for(int i = 0; i < sources.Count; i++)
        {
            if (sources[i].audioSources == null) continue;

            for(int j = 0; j < sources[i].audioSources.Count; j++)
            {
                float strength = sources[i].audioSources[j].mute == true ? 0 : 1;
                strength *= sources[i].audioSources[j].volume;

                sources[i].audioSources[j].GetSpectrumData(spec, 0, FFTWindow.Rectangular);

                for(int k = 0; k < (int)fft_res; k++)
                {
                    target[k] += spec[k] * strength / sources[i].audioSources.Count;
                }
            }
        }
    }

    /// <summary>
    /// 指定した MusicType の現在のスペクトルを渡す関数
    /// </summary>
    /// <param name="type"> 欲しい MusicType </param>
    /// <param name="target"> この配列に値が格納される、長さは fft_res </param>
    public void GetSpectrum(MusicType type, ref float[] target)
    {
        float[] spec = new float[(int)fft_res];
        target = new float[(int)fft_res];

        int sourceIndex = -1;
        for (int i = 0; i < sources.Count; i++)
        {
            if (sources[i].musicType == type)
            {
                sourceIndex = i;
                break;
            }
        }

        if (sourceIndex == -1 || sources[sourceIndex].audioSources == null)
        {
            Debug.LogWarning("指定された MusicType の AudioSource が見つかりません: " + type);
            return;
        }

        for (int j = 0; j < sources[sourceIndex].audioSources.Count; j++)
        {
            float strength = sources[sourceIndex].audioSources[j].mute == true ? 0 : 1;
            strength *= sources[sourceIndex].audioSources[j].volume;

            sources[sourceIndex].audioSources[j].GetSpectrumData(spec, 0, FFTWindow.Rectangular);

            for (int k = 0; k < (int)fft_res; k++)
            {
                target[k] += spec[k] * strength / sources[sourceIndex].audioSources.Count;
            }
        }
    }
}
