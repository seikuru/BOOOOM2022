using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 enum MusicType
{
    Bass,Drums,Chord,Melody
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
    public AudioSource[] AudioSources;
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
    // Start is called before the first frame update
    void Start()
    {
        OneMeasure = 240 / BPM;

        beforCollectPoint = CollectObject.CollectPoint;
        foreach (var a1 in CMC)
        {
            foreach (var b2 in a1.AudioSources)
            {
                if (b2 != null)
                {
                    b2.enabled = true;
                    b2.mute = true;
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        Count += Time.deltaTime;

        Debug.Log(Count);

        if (Count >= OneMeasure)
        {
            Debug.Log("o+wow");
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

                        if (CMC[i].AudioSources.Length > ObjectNumber
                            && CMC[i].AudioSources[ObjectNumber] != null)
                        {
                            CMC[i].AudioSources[ObjectNumber].mute = false;
                        }
                        ObjectNumber++;
                    }

                }
                ObjectNumber = 0;
            }
            Count = 0;
        }
    }

    void AudioChange()
    {
        //AS[CollectObject.CollectPoint].mute = false;
    }
}
