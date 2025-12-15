using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 enum MusicType
{
    Bass,Drums,Chord,Melody
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
    int beforeCollectPoint = 0;
    int ObjectNumber = 0;
    [SerializeField] float Count = 0;
    [SerializeField] int BPM = 150;
    float OneMeasureMult;
    Coroutine[] AudioCoroutine;
    [SerializeField] float FadeInTime = 3.0f;
    [SerializeField] float FirstBigTime = 3.0f;
    float FadeInTimeOne = 0;
    float FirstBigTimeOne = 0;
    int CoroutineCount = 0;
    WaitForSeconds wfs1frame;
    WaitForSeconds wfs1beat;
    [SerializeField] float DownVolume = 0.6f;

    public bool AllCollectBGMCheck => AllCollectPoint <= CurrentCollectPoint;

    int AllCollectPoint, CurrentCollectPoint;
    bool Once = true;

    // Start is called before the first frame update
    void Start()
    {
        wfs1frame = new WaitForSeconds(Time.fixedDeltaTime);
        OneMeasureMult = (float)BPM / 60.0f;
        wfs1beat = new WaitForSeconds( 1.0f / OneMeasureMult);

        FadeInTimeOne = 1 / FadeInTime * Time.fixedDeltaTime;
        FirstBigTimeOne = 1 / FirstBigTime * Time.fixedDeltaTime;

        beforeCollectPoint = CollectObject.CollectPoint;
        int i = 0;int FadeInNumber = 0;
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

                    if(b2.entryType == MusicEntry.fadeIn)
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
                            if (Count >= 4.0f * 4.0f)
                            {
                                AudioCoroutine[CoroutineCount] = StartCoroutine(firstBigAudio(CMC[i].ASC[ObjectNumber].AudioSource, CoroutineCount));
                                CoroutineCount++;
                                
                                
                            }
                        }
                        else if (CMC[i].ASC[ObjectNumber].entryType == MusicEntry.fadeIn
                            && CMC[i].ASC[ObjectNumber].AudioSource.volume == 0.0f)
                        {

                            AudioCoroutine[CoroutineCount] = StartCoroutine(FadeInAudio(CMC[i].ASC[ObjectNumber].AudioSource, CoroutineCount));
                            CoroutineCount++;
                            AudioCoroutine[CoroutineCount] = StartCoroutine(FadeInOtherDown(CMC[i].ASC[ObjectNumber].AudioSource, CoroutineCount));
                            CoroutineCount++;
                        }
                    }


                    ObjectNumber++;
                }

            }
            ObjectNumber = 0;
        }

        if (Count >= 4.0f * 4.0f)
        {
            Count = 0;
        }


    }

    void AudioChange()
    {
        //AS[CollectObject.CollectPoint].mute = false;
    }

    IEnumerator FadeInAudio(AudioSource audio, int CoroutineCount)
    {
        CurrentCollectPoint++;
        audio.mute = false;
        audio.time = StartBGM.time;

        while (audio.volume < 1.0f)
        {
            audio.volume += FadeInTimeOne;
            yield return wfs1frame;
        }

        yield return wfs1beat;
  
        StopCoroutine(AudioCoroutine[CoroutineCount]);
    }

    IEnumerator FadeInOtherDown(AudioSource audio, int CoroutineCount)
    {
        

        yield return wfs1beat;
        yield return wfs1beat;

        StartBGM.volume = DownVolume;
        foreach (var a1 in CMC)
        {
            foreach (var a2 in a1.ASC)
            {
                if (a2.AudioSource.volume >= DownVolume && a2.AudioSource != audio )
                {
                    a2.AudioSource.volume = DownVolume;
                    
                }
            }
        }

        //àÍè¨êﬂë“Ç¬
        for (int i = 0;i < 4;i++)
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
                        yield return null;
                    }
                }
            }

        }

        StopCoroutine(AudioCoroutine[CoroutineCount]);

    }

    IEnumerator firstBigAudio(AudioSource audio, int CoroutineCount)
    {
        float time = 0.0f;

        StartBGM.volume = DownVolume;

        foreach (var a1 in CMC)
        {
            foreach (var a2 in a1.ASC)
            {
                if (a2.AudioSource.mute == false)
                {
                    a2.AudioSource.volume = DownVolume;
                    
                }
            }
        }

        CurrentCollectPoint++;
        audio.mute = false;
        audio.volume = 1.0f;
        audio.time = StartBGM.time;

        yield return null;

        while (time <= FirstBigTime)
        {
            foreach (var a1 in CMC)
            {
                foreach (var a2 in a1.ASC)
                {
                    if (a2.AudioSource.mute == false)
                    {
                        a2.AudioSource.volume += FirstBigTimeOne;
                        StartBGM.volume += FirstBigTimeOne;
                        yield return null;
                    }
                }
            }
            time += Time.deltaTime;
        }

        StopCoroutine(AudioCoroutine[CoroutineCount]);

    }
}
