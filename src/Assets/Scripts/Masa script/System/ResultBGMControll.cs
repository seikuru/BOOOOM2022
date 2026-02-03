using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// リザルト画面の音声ノード
/// 通常クリップとノイズクリップを切り替え可能
/// </summary>
[Serializable]
public class ResultAudioNode
{
    [SerializeField] AudioClip clipDefalt, clipNoize;
    [SerializeField] public AudioSource audioSource;

    /// <summary>
    /// 通常クリップまたはノイズクリップを設定
    /// </summary>
    public void SetClip(bool isDefaltClip)
    {
        audioSource.clip = isDefaltClip ? clipDefalt : clipNoize;
    }

    /// <summary>
    /// 指定したDSP時間で再生予約
    /// </summary>
    public void PlayScheduled(double DpsTime)
    {
        audioSource?.PlayScheduled(DpsTime);
    }

    /// <summary>
    /// 現在のクリップが通常クリップかチェック
    /// </summary>
    public bool CheckClip()
    {
        return audioSource.clip == clipDefalt;
    }
}

/// <summary>
/// 音楽タイプごとの音声ノード配列を保持
/// </summary>
[Serializable]
public class ResultAudio
{
    [SerializeField]
    public MusicType type;
    [SerializeField]
    public ResultAudioNode[] RAN;
}

public class ResultBGMControll : MonoBehaviour
{
    /// リザルト画面のBGM制御クラス
    /// スコアに応じて通常/ノイズクリップを切り替え、全音源を同期再生

    [SerializeField] ResultAudio[] resultAudios;
    [SerializeField] float WaitStartCount = 0.3f;

    // 音楽タイプごとの有効な音声ノード数
    Dictionary<MusicType, int> AudioLength;

    /// <summary>
    /// スコアに基づいて各音声ノードのクリップを設定
    /// 達成度に応じて通常クリップとノイズクリップを切り替え
    /// </summary>
    void AudioClipSetting()
    {
        AudioLength = new Dictionary<MusicType, int>();

        int current = 0, max = 1;

        // 各音楽タイプについて処理
        foreach (var ra in resultAudios)
        {
            // スコアマネージャーから達成状況を取得
            ScoreManager.TakeMusicValue(ra.type, ref current, ref max);

            // 有効な音声ノード数を決定(配列長とmax値の小さい方)
            AudioLength[ra.type] = Mathf.Min(ra.RAN.Length,max);

            // 各音声ノードにクリップを設定
            for (int i = 0; i < AudioLength[ra.type]; i++)
            {
                // currentより小さいインデックスは通常クリップ、それ以外はノイズクリップ
                ra.RAN[i].SetClip(i < current);       
            }
        }

        // オーディオのウォームアップ処理を開始
        StartCoroutine(WarmUpAudio());
    }

    /// <summary>
    /// 全てのBGMを同期して再生開始
    /// DSP時間を使用して正確なタイミングで再生
    /// </summary>
    void AllBGMPlay()
    {
        // 現在のDSP時間から0.1秒後を開始時刻として設定
        double currentStartDspTime = AudioSettings.dspTime + 0.1;

        foreach (var ra in resultAudios)
        {
            // 有効な音声ノードのみ再生
            for (int i = 0; i < AudioLength[ra.type]; i++)
            {
                ra.RAN[i].PlayScheduled(currentStartDspTime);

                // 通常クリップ・ノイズクリップ共に1秒から開始
                if (ra.RAN[i].CheckClip())
                {
                    ra.RAN[i].audioSource.time = 1.0f;
                }
                else
                {
                    ra.RAN[i].audioSource.time = 1.0f;
                }
            }
        }
    }

    /// <summary>
    /// オーディオのウォームアップ処理
    /// 一度無音で再生→停止することでオーディオスレッドを準備し、
    /// 本番の同期再生時の遅延を防ぐ
    /// </summary>
    IEnumerator WarmUpAudio()
    {
        // 元の設定を保存するキュー
        Queue<Tuple<bool, float>> AudioParameters = new();

        // 全音源を無音で再生開始
        foreach (var ra in resultAudios)
        {
            foreach (var ran in ra.RAN)
            {
                var a = ran.audioSource;
                // 現在の設定を保存
                AudioParameters.Enqueue(new(a.mute, a.volume));
                // 無音設定にして再生
                a.mute = false;
                a.volume = 0f;
                a.Play();
            }
        }

        // Audio Threadの準備を待つ
        yield return null;

        // 全音源を停止し、元の設定に戻す
        foreach (var ra in resultAudios)
        {
            foreach (var ran in ra.RAN)
            {
                var a = ran.audioSource;
                var AP = AudioParameters.Dequeue();

                a.Stop();
                // 元の設定を復元
                a.mute = AP.Item1;
                a.volume = AP.Item2;
            }
        }

        // 指定時間後に本番の再生を開始
        Invoke("AllBGMPlay", WaitStartCount);
    }

    void Start()
    {
        AudioClipSetting();      
    }
}
