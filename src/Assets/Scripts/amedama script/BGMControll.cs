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
    int beforCollectPoint = 0;
    int ObjectNumber = 0;
    [SerializeField]float Count = 0;
    [SerializeField]int BPM = 150;
    float OneMeasure;
    Coroutine[] AudioCoroutine;
    [SerializeField] float FadeInTime = 3.0f;
    [SerializeField] float FirstBigTime = 3.0f;
    float FadeInTimeOne;
    int CoroutineCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        OneMeasure = 240 / BPM;

        FadeInTimeOne = FadeInTime * Time.fixedDeltaTime;
        
        beforCollectPoint = CollectObject.CollectPoint;
        int i = 0;
        foreach (var a1 in CMC)
        {
            foreach (var b2 in a1.ASC)
            {
                i++;
                if (b2.AudioSource != null)
                {
                    b2.AudioSource.enabled = true;
                    b2.AudioSource.volume = 0;
                }
            }
        }
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
                                AudioCoroutine[CoroutineCount] = StartCoroutine(firstBigAudio(CMC[i].ASC[ObjectNumber].AudioSource));
                                CoroutineCount++;
                            }
                        }
                        else if (CMC[i].ASC[ObjectNumber].entryType == MusicEntry.fadeIn 
                            && CMC[i].ASC[ObjectNumber].AudioSource.volume == 0.0f)
                        {

                            AudioCoroutine[CoroutineCount] = StartCoroutine(FadeInAudio(CMC[i].ASC[ObjectNumber].AudioSource));
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

    IEnumerator FadeInAudio(AudioSource audio)
    {
        while (audio.volume < 0.9f)
        {
            audio.volume += FadeInTimeOne;
            yield return null;
        }

        StopCoroutine(AudioCoroutine[CoroutineCount]);
    }

    IEnumerator firstBigAudio(AudioSource audio)
    {

        audio.volume = 1.0f;

        yield return new WaitForSeconds(FirstBigTime);

        audio.volume = 0.9f;

        StopCoroutine(AudioCoroutine[CoroutineCount]);

    }
}
