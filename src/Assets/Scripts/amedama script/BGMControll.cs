using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 enum MusicType
{
    Type1, Type2
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
    // Start is called before the first frame update
    void Start()
    {
        beforCollectPoint = CollectObject.CollectPoint;
        foreach (var a1 in CMC)
        {
            foreach (var b2 in a1.AudioSources)
            {
                if (b2 != null)
                {
                    b2.mute = true;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        ObjectNumber = 0;
        foreach (var b1 in COC)
        {
            foreach (var b2 in b1.CollectObject)
            {
                if (b2 == null)
                {
                    if (CMC[(int)b1.COCMusicType].AudioSources.Length > ObjectNumber 
                        && CMC[(int)b1.COCMusicType].AudioSources[ObjectNumber] != null)
                    {
                        CMC[(int)b1.COCMusicType].AudioSources[ObjectNumber].mute = false;
                    }
                    ObjectNumber++;
                }
                
            }
            ObjectNumber = 0;
        }
    }

    void AudioChange()
    {
        //AS[CollectObject.CollectPoint].mute = false;
    }
}
