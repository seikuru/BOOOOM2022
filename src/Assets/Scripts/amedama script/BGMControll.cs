using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum MusicType

{
    Bass,Drums,Chord,Melody,

    Max
}

enum MusicEntry
{
    fadeIn, Measure
}

[Serializable]
class AudioSourceClass
{
    public AudioSource AudioSource;
    public MusicEntry entryType;
}

[Serializable]
class CollectObjectClass
{
    
    public MusicType COCMusicType;
    public GameObject[] CollectObject;
}

[Serializable]
class CollectMusicClass
{
    public MusicType CMCMusicType;
    public AudioSourceClass[] ASC;

    int currentIndex = 0;
    public bool CrrentIndexCheck() => (ASC != null && ASC.Length > currentIndex);

    public int GetIndex() => currentIndex;

    public void NextIndex() => currentIndex++;

    public int useIndex = 1;
}

public class BGMControll : MonoBehaviour
{

    [SerializeField] AudioSource StartBGM;
    [SerializeField] CollectObjectClass[] COC;
    [SerializeField] CollectMusicClass[] CMC;

    [SerializeField] Text DebugText;
    [SerializeField] UnityEvent BounsTimeEvent;
    [SerializeField] AudioGageManager GageManager;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] SymbolEmission symbolEmission;
    [SerializeField] float Count = 0;
    [SerializeField] int BPM = 150;
    [SerializeField] float FadeInTime = 3.0f;
    [SerializeField] float FirstBigTime = 3.0f;
    [SerializeField] float FirstBigSeparate = 16.0f;
    [SerializeField] float DownVolume = 0.6f;
    //int beforeCollectPoint = 0;
    int ObjectNumber = 0;
    int CoroutineCount = 0;
  
    float OneMeasureMult;
    float FadeInTimeOne = 0;
    float FirstBigTimeOne = 0;
    float note16 = 0.0f;
    float VolumeControl = 0.0f;
    
    WaitForSeconds wfs1frame;
    WaitForSeconds wfs1beat;
    WaitForSeconds wfs16note;
   
    Coroutine[] AudioCoroutine;
    Queue<AudioSourceClass> MeasureEntryASC;

    [SerializeField]
    

    public bool AllCollectBGMCheck => AllCollectPoint <= CurrentCollectPoint;

    int AllCollectPoint, CurrentCollectPoint;
    bool Once = true;

    static Queue<MusicType> openTypes;

    public static void OpenTypeSetting(MusicType type) => openTypes.Enqueue(type);

    public void ResultBGMPlay(MusicType type)
    {
        // 未来の DSP 時間を取得
        double currentStartDspTime = AudioSettings.dspTime + 0.1;
        foreach (var cmc in CMC)
        {
            // MusicType が一致したら探索終了
            if (cmc.CMCMusicType != type)
            {
                continue;
            }

            if (!cmc.CrrentIndexCheck())
            {           
                return;
            }

            var ASC = cmc.ASC[cmc.GetIndex()];

            cmc.NextIndex();

            if (ASC == null)
            {
                return;
            }
            else
            {
                ASC.AudioSource.PlayScheduled(currentStartDspTime);
                ASC.AudioSource.mute = false;
                ASC.AudioSource.volume = 0.8f;
                return;
            }
        }
    } 


    // イベント駆動のため、いる
    public void BGMPlay()
    {
        // 未来の DSP 時間を取得
        double currentStartDspTime = AudioSettings.dspTime + 0.1;
        foreach (var a1 in CMC)
        {
            foreach (var b2 in a1.ASC)
            { 
                if (b2.AudioSource != null)
                {
                    b2.AudioSource.PlayScheduled(currentStartDspTime);
                    Debug.Log(b2.AudioSource.name);
                }
            }
        }
        
    }

    public int GetTypeMaxValue(MusicType type)
    {
        int length = 0;
        foreach(var cmc in CMC)
        {
            if(cmc.CMCMusicType == type)
            {
                length =  cmc.useIndex;
            }
        }

        return length;
    }

    // Start is called before the first frame update
    void Start()
    {
        openTypes = new Queue<MusicType>();

        wfs1frame = new WaitForSeconds(Time.fixedDeltaTime);
        OneMeasureMult = (float)BPM / 60.0f;
        wfs1beat = new WaitForSeconds(1.0f / OneMeasureMult);
        wfs16note = new WaitForSeconds(1.0f / OneMeasureMult / 16.0f);

        if (FirstBigTime >= FirstBigSeparate)
        {
            FirstBigTime = FirstBigSeparate;
        }

        FadeInTimeOne = Time.fixedDeltaTime * OneMeasureMult / FadeInTime;
        FirstBigTimeOne = (1.0f - DownVolume) / (FirstBigTime * 10.0f);

        //beforeCollectPoint = CollectObject.CollectPoint;

        int i = 0; int FadeInNumber = 0;
        AllCollectPoint = 0;

        foreach (var a1 in CMC)
        {
            foreach (var b2 in a1.ASC)
            {
                i++;
                if (b2.AudioSource != null)
                {
                    
                    b2.AudioSource.enabled = true;
                    b2.AudioSource.mute = true;
                    b2.AudioSource.volume = 0;
                    
                    if (b2.entryType == MusicEntry.fadeIn)
                    {
                        FadeInNumber++;
                    }
                }
            }

            AllCollectPoint += a1.useIndex;
        }

        if(StartBGM != null)
        {
            StartBGM.enabled = true;
            StartBGM.mute = false;
            StartBGM.volume = 1.0f;
        }

        StartCoroutine(WarmUpAudio());

        AudioCoroutine = new Coroutine[i + FadeInNumber];
        MeasureEntryASC = new();

        CurrentCollectPoint = 0;
    }

    IEnumerator WarmUpAudio()
    {
        foreach (var a1 in CMC)
        {
            foreach (var b2 in a1.ASC)
            {
                if (b2.AudioSource != null)
                {
                    var a = b2.AudioSource;
                    float originalVolume = a.volume;
                    bool mute = a.mute;

                    a.volume = 0.01f;
                    a.mute = false;
                    a.Play();

                    // Audio Thread に 1 フレーム渡す
                    yield return null;

                    a.Stop();
                    a.mute = mute; 
                    a.volume = originalVolume;
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 経過時間カウント
        // Time.deltaTime に拍子倍率を掛けてカウントを進める
        Count += Time.deltaTime * OneMeasureMult;

        while(openTypes != null && openTypes.Count != 0)
        {
            var type = openTypes.Dequeue();

            // CMC から MusicType が一致するものを線形探索
            foreach (var cmc in CMC)
            {
                // MusicType が一致したら探索終了
                if (cmc.CMCMusicType != type)
                {
                    continue;
                }

                // スコア加算
                scoreManager.AddScoreSymbol();

                if (!cmc.CrrentIndexCheck())
                {
                    Debug.Log("Index超過");
                    scoreManager?.AddScoreBonus(CountDownTimer.BonusTimeValue);
                    break;
                }

                var ASC = cmc.ASC[cmc.GetIndex()];

                cmc.NextIndex();

                if (ASC == null)
                {
                    Debug.Log("AudioSourceClassがnull");
                    break;
                }

                // ゲージの色を MusicType に応じて変更
                GageManager?.SetColorImage(cmc.CMCMusicType);
                // MusicType に応じてエミッション起動
                symbolEmission.SetEmission(cmc.CMCMusicType);
                // 取得した MusicType をスコア側に通知
                scoreManager?.AddMusicType(cmc.CMCMusicType);

                CurrentCollectPoint++;

                if (CurrentCollectPoint == AllCollectPoint)
                    BounsTimeEvent.Invoke();

                AudioEntry(ASC);
            }
        }

        /*
        // CollectObject 内で使用するインデックス（音声配列参照用）
        ObjectNumber = 0;

        // COC（CollectObjectClass）を順に処理
        foreach (var b1 in COC)
        {
            // 各 COC が保持している CollectObject を走査
            foreach (var b2 in b1.CollectObject)
            {
                // CollectObject が null の場合のみ処理
                if (b2 == null)
                {
                    int i = 0;
                    // CMC から MusicType が一致するものを線形探索
                    foreach (var a1 in CMC)
                    {
                        // MusicType が一致したら探索終了
                        if (a1.CMCMusicType == b1.COCMusicType)
                        {
                            break;
                        }
                        i++;
                    }

                    // ASC 配列の範囲チェックと AudioSource の null チェック
                    if (CMC[i].ASC.Length > ObjectNumber
                        && CMC[i].ASC[ObjectNumber].AudioSource != null)
                    {
                        // デバッグ用ログ
                        Debug.Log(b1.COCMusicType + " " + (CMC[i].ASC[ObjectNumber].AudioSource.volume == 0.0f));

                        if(CMC[i].ASC[ObjectNumber].AudioSource.volume == 0.0f)
                        {
                            // ゲージの色を MusicType に応じて変更
                            GageManager?.SetColorImage(CMC[i].CMCMusicType);
                            // スコア加算（破壊扱い）
                            scoreManager?.AddScoreDestroy();
                            // 取得した MusicType をスコア側に通知
                            scoreManager?.AddMusicType(CMC[i].CMCMusicType);

                            // エントリータイプが Measure の場合
                            if (CMC[i].ASC[ObjectNumber].entryType == MusicEntry.Measure)
                            {
                                // 一定時間経過後のみ処理
                                if (Count >= FirstBigSeparate)
                                {
                                    CurrentCollectPoint++;

                                    // 最初の大音量再生コルーチンを開始
                                    AudioCoroutine[CoroutineCount] = StartCoroutine(firstBigAudio(CMC[i].ASC[ObjectNumber].AudioSource));
                                    CoroutineCount++;

                                    if (CurrentCollectPoint == AllCollectPoint)
                                        BounsTimeEvent.Invoke();
                                }
                            }
                            // エントリータイプが FadeIn の場合
                            else if (CMC[i].ASC[ObjectNumber].entryType == MusicEntry.fadeIn)
                            {
                                CurrentCollectPoint++;

                                // フェードイン再生
                                AudioCoroutine[CoroutineCount] = StartCoroutine(FadeInAudio(CMC[i].ASC[ObjectNumber].AudioSource));
                                CoroutineCount++;
                                // 他の音量を下げるフェード処理
                                AudioCoroutine[CoroutineCount] = StartCoroutine(FadeInOtherDown(CMC[i].ASC[ObjectNumber].AudioSource));
                                CoroutineCount++;
                            }
                        }          
                    }

                    // CollectObject 内の AudioSource インデックスを進める
                    ObjectNumber++;           
                }
            }
            // 次の COC に移る前に ObjectNumber をリセット
            ObjectNumber = 0;
        }         
        */

        // 一定時間経過後にMeasureで起動する処理を起動する
        if (Count >= FirstBigSeparate)
        {
            Count = 0;
            while (MeasureEntryASC != null && MeasureEntryASC.Count != 0)
            {
                var ASC = MeasureEntryASC.Dequeue();
                if(ASC != null)
                {
                    // 最初の大音量再生コルーチンを開始
                    AudioCoroutine[CoroutineCount] = StartCoroutine(firstBigAudio(ASC.AudioSource));
                    CoroutineCount++;
                }
            }     
        }
    }

    void AudioEntry(AudioSourceClass asc)
    {
        if (asc.AudioSource == null)
        {
            Debug.Log("AudioSourceがnull");
            return;
        }

        if (asc.AudioSource.volume == 0.0f)
        {
            // エントリータイプが Measure の場合
            if (asc.entryType == MusicEntry.Measure)
            {
                // 一定時間経過後のみ処理を待機
                MeasureEntryASC.Enqueue(asc);                              
            }

            // エントリータイプが FadeIn の場合
            else if (asc.entryType == MusicEntry.fadeIn)
            {
                // フェードイン再生
                AudioCoroutine[CoroutineCount] = StartCoroutine(FadeInAudio(asc.AudioSource));
                CoroutineCount++;
                // 他の音量を下げるフェード処理
                AudioCoroutine[CoroutineCount] = StartCoroutine(FadeInOtherDown(asc.AudioSource));
                CoroutineCount++;
            }
        }
    }
    
    IEnumerator FadeInAudio(AudioSource audio)
    {
        //CurrentCollectPoint++;
        audio.mute = false;
        audio.time = StartBGM.time;

        while (audio.volume < 1.0f)
        {
            audio.volume += FadeInTimeOne;
            yield return wfs1frame;
        }

        //yield return wfs1beat;

        yield break;
    }

    IEnumerator FadeInOtherDown(AudioSource audio)
    {


        yield return wfs1beat;
        yield return wfs1beat;

        StartBGM.volume = DownVolume;
        foreach (var a1 in CMC)
        {
            foreach (var a2 in a1.ASC)
            {
                if (a2.AudioSource.volume >= DownVolume && a2.AudioSource != audio)
                {
                    a2.AudioSource.volume = DownVolume;

                }
            }
        }

        //�ꏬ�ߑ҂�
        for (int i = 0; i < 4; i++)
        {
            yield return wfs1beat;
        }

        int secondsFlame = (int)(DownVolume / Time.fixedDeltaTime) + 1;

        for (int i = 0; i < secondsFlame; i++)
        {

            foreach (var a1 in CMC)
            {
                foreach (var a2 in a1.ASC)
                {
                    if (a2.AudioSource.mute == false)
                    {
                        a2.AudioSource.volume += Time.deltaTime;
                        StartBGM.volume += Time.deltaTime;
                        yield return wfs1frame;
                    }
                }
            }

        }

        yield break;

    }

    IEnumerator firstBigAudio(AudioSource audio)
    {

        VolumeControl = DownVolume;
        float VolumeControlBefore1frame = DownVolume;
        int CountBefore = 0;
        int CountMax = (int)(FirstBigTime / Time.fixedDeltaTime) + 1;

        StartBGM.volume = VolumeControl;

        foreach (var a1 in CMC)
        {
            foreach (var a2 in a1.ASC)
            {
                if (a2.AudioSource.mute == false)
                {
                    a2.AudioSource.volume = VolumeControl;
                }
            }
        }

        yield return wfs1beat;

        //CurrentCollectPoint++;
        audio.mute = false;
        audio.volume = 1.0f;
        audio.time = StartBGM.time;

        yield return null;

        while (VolumeControl <= 1.0f )
        { 

            if (VolumeControl == VolumeControlBefore1frame)
            {
                VolumeControl += FirstBigTimeOne;// * Time.deltaTime;       
            }

            if ((int)(Count * 10.0f) != CountBefore)
            {
                CountBefore = (int)(Count * 10.0f);

                StartBGM.volume = VolumeControl;
                foreach (var a1 in CMC)
                {
                    foreach (var a2 in a1.ASC)
                    {
                        if (a2.AudioSource.mute == false)
                        {
                            a2.AudioSource.volume = VolumeControl;
                            VolumeControlBefore1frame = VolumeControl;

                        }
                    }
                }
            }

            yield return wfs1frame;
        }
        yield break;
    }
}
