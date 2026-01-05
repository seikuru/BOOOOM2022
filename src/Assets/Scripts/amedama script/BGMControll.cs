using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Unity.Animations.SpringBones.GUIElements;

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

    [SerializeField] float Count = 0;
    [SerializeField] int BPM = 150;
    [SerializeField] float FadeInTime = 3.0f;
    [SerializeField] float FirstBigTime = 3.0f;
    [SerializeField] float FirstBigSeparate = 16.0f;
    [SerializeField] float DownVolume = 0.6f;
    int beforeCollectPoint = 0;
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

    public bool AllCollectBGMCheck => AllCollectPoint <= CurrentCollectPoint;

    int AllCollectPoint, CurrentCollectPoint;
    bool Once = true;

    // Start is called before the first frame update
    void Start()
    {
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

        beforeCollectPoint = CollectObject.CollectPoint;
        int i = 0; int FadeInNumber = 0;
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
        }

        StartBGM.enabled = true;
        StartBGM.mute = false;
        StartBGM.volume = 1.0f;

        AudioCoroutine = new Coroutine[i + FadeInNumber];

        AllCollectPoint = i;
        CurrentCollectPoint = 0;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Count += Time.deltaTime * OneMeasureMult;

        ObjectNumber = 0;
        foreach (var b1 in COC)
        {
            foreach (var b2 in b1.CollectObject)
            {

                if (b2 == null)
                {
                    int i = 0;
                    foreach (var a1 in CMC)
                    {

                        if (a1.CMCMusicType == b1.COCMusicType)
                        {
                            break;
                        }
                        i++;
                    }
                    if (CMC[i].ASC.Length > ObjectNumber
                        && CMC[i].ASC[ObjectNumber].AudioSource != null)
                    {

                        if (CMC[i].ASC[ObjectNumber].entryType == MusicEntry.Measure
                            && CMC[i].ASC[ObjectNumber].AudioSource.volume == 0.0f)
                        {
                            if (Count >= FirstBigSeparate)
                            {
                                AudioCoroutine[CoroutineCount] = StartCoroutine(firstBigAudio(CMC[i].ASC[ObjectNumber].AudioSource)); 
                                CoroutineCount++;
                           
                                GageManager?.SetColorImage(CMC[i].CMCMusicType);

                               if (CurrentCollectPoint == AllCollectPoint)
                                    BounsTimeEvent.Invoke();
                            }
                        }
                        else if (CMC[i].ASC[ObjectNumber].entryType == MusicEntry.fadeIn
                            && CMC[i].ASC[ObjectNumber].AudioSource.volume == 0.0f)
                        {

                            AudioCoroutine[CoroutineCount] = StartCoroutine(FadeInAudio(CMC[i].ASC[ObjectNumber].AudioSource));
                            CoroutineCount++;
                            AudioCoroutine[CoroutineCount] = StartCoroutine(FadeInOtherDown(CMC[i].ASC[ObjectNumber].AudioSource));
                            CoroutineCount++;

                        }
                    }
                    ObjectNumber++;

                    scoreManager?.AddScoreDestroy();
                }
            }
            ObjectNumber = 0;
        }

        if (Count >= FirstBigSeparate)
        {
            Count = 0;
        }

    }

    IEnumerator FadeInAudio(AudioSource audio)
    {
        CurrentCollectPoint++;
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

        CurrentCollectPoint++;
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
