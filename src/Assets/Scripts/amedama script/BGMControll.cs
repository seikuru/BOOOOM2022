using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] Text DebugText;

    public bool AllCollectBGMCheck => AllCollectPoint <= CurrentCollectPoint;
    int AllCollectPoint = 0, CurrentCollectPoint = 0;
    int ObjectNumber = 0;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var a1 in CMC)
        {
            AllCollectPoint += a1.AudioSources.Length;

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
        if(DebugText != null)
            DebugText.text = CurrentCollectPoint.ToString() + "/" + AllCollectPoint.ToString();
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
                        if(CMC[i].AudioSources[ObjectNumber].mute)
                        {
                            CMC[i].AudioSources[ObjectNumber].mute = false;
                            CurrentCollectPoint++;
                        }
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
