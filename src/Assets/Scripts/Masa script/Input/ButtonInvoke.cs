using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInvoke : MonoBehaviour
{
    [SerializeField] Button button;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            button.onClick.Invoke();
    }


   
    /// 　以下テスト用コード(音源再生テストで使用)
    /*
    [Header("同期再生する AudioSource 群")]
    [SerializeField]
    private List<AudioSource> audioSources = new List<AudioSource>();

    [Header("再生開始までの予約時間（秒）")]
    [SerializeField]
    private double scheduleOffset = 0.1;

    // 現在の再生開始DSP時間
    private double currentStartDspTime;

    /// <summary>
    /// Button などから呼ぶ同期再生
    /// </summary>
    public void PlayAllScheduled()
    {
        // すでに再生中なら止める（設計次第で削除可）
        StopAllScheduled();

        // 未来の DSP 時間を取得
        currentStartDspTime = AudioSettings.dspTime + scheduleOffset;

        // 全 AudioSource を同一 DSP 時間で再生予約
        foreach (var src in audioSources)
        {
            if (src == null || src.clip == null)
                continue;

            src.PlayScheduled(currentStartDspTime);
        }
    }

    /// <summary>
    /// 全停止（即時）
    /// </summary>
    public void StopAllScheduled()
    {
        foreach (var src in audioSources)
        {
            if (src == null)
                continue;

            src.Stop();
        }
    }

    public void WarmUpAudio()
    {
        foreach (var src in audioSources)
        {
            if (src == null || src.clip == null)
                continue;

            src.volume = 0f;
            src.Play();
            src.Stop();
            src.volume = 1f;
        }
    }
    */
}
