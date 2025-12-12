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
    float OneMeasure;
    Coroutine[] AudioCoroutine;
    [SerializeField] float FadeInTime = 3.0f;
    [SerializeField] float FirstBigTime = 3.0f;
    float FadeInTimeOne = 0;
    float FirstBigTimeOne = 0;
    int CoroutineCount = 0;
    WaitForSeconds wfs1frame;
    // Start is called before the first frame update
    void Start()
    {
        wfs1frame = new WaitForSeconds(Time.fixedDeltaTime);
        OneMeasure = 240 / BPM;

        FadeInTimeOne = 1 / FadeInTime * Time.fixedDeltaTime;
        FirstBigTimeOne = 1 / FirstBigTime * Time.fixedDeltaTime;

        beforeCollectPoint = CollectObject.CollectPoint;
        int i = 0;
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
                }
            }
        }

        StartBGM.enabled = true;
        StartBGM.mute = false;
        StartBGM.volume = 1.0f;
        Debug.Log(i);

        AudioCoroutine = new Coroutine[i];
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        Count += Time.deltaTime;


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
                            if (Count >= OneMeasure)
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
                        }
                    }


                    ObjectNumber++;
                }

            }
            ObjectNumber = 0;
        }

        if (Count >= OneMeasure)
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

        audio.mute = false;

        while (audio.volume < 1.0f)
        {
            audio.volume += FadeInTimeOne;
            yield return wfs1frame ;
        }

        StopCoroutine(AudioCoroutine[CoroutineCount]);
    }

    IEnumerator firstBigAudio(AudioSource audio, int CoroutineCount)
    {
        float time = 0.0f;

        foreach (var a1 in CMC)
        {
            foreach (var a2 in a1.ASC)
            {
                if (a2.AudioSource.mute == false)
                {
                    a2.AudioSource.volume = 0.6f;
                }
            }
        }
        audio.mute = false;
        audio.volume = 1.0f;

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
                        yield return null;
                    }
                }
            }
            time += Time.deltaTime;
        }

        StopCoroutine(AudioCoroutine[CoroutineCount]);

    }
}
