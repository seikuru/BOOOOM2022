using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResultAudioNode
{
    [SerializeField] AudioClip clipDefalt, clipNoize;
    [SerializeField] public AudioSource audioSource;

    public void SetClip(bool isDefaltClip)
    {
        audioSource.clip = isDefaltClip ? clipDefalt : clipNoize;
    }

    public void PlayScheduled(double DpsTime)
    {
        audioSource?.PlayScheduled(DpsTime);
    }

    public bool CheckClip()
    {
        return audioSource.clip == clipDefalt;
    }
}

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
    [SerializeField] ResultAudio[] resultAudios;
    [SerializeField] float WaitStartCount = 0.3f;

    Dictionary<MusicType, int> AudioLength;

    void AudioClipSetting()
    {
        AudioLength = new Dictionary<MusicType, int>();

        int current = 0, max = 1;
        foreach (var ra in resultAudios)
        {
            ScoreManager.TakeMusicValue(ra.type, ref current, ref max);

            AudioLength[ra.type] = Mathf.Min(ra.RAN.Length,max);

            for (int i = 0; i < /*ra.RAN.Length*/ AudioLength[ra.type]; i++)
            {
                ra.RAN[i].SetClip(i < current);       
            }
        }

        StartCoroutine(WarmUpAudio());

    }

    void AllBGMPlay()
    {
        double currentStartDspTime = AudioSettings.dspTime + 0.1;
        foreach (var ra in resultAudios)
        {

            for (int i = 0; i < AudioLength[ra.type]; i++)
            {
                ra.RAN[i].PlayScheduled(currentStartDspTime);
                if (ra.RAN[i].CheckClip())
                {
                    ra.RAN[i].audioSource.time = 1.0f;
                }
                else
                {
                    ra.RAN[i].audioSource.time = 1.0f;
                }
            }
            /*
            foreach (var ran in ra.RAN)
            {
                ran.PlayScheduled(currentStartDspTime);
            }
            */
        }
    }

    IEnumerator WarmUpAudio()
    {
        Queue<Tuple<bool, float>> AudioParameters = new();
        foreach (var ra in resultAudios)
        {
            foreach (var ran in ra.RAN)
            {
                var a = ran.audioSource;

                AudioParameters.Enqueue(new(a.mute, a.volume));

                a.mute = false;
                a.volume = 0f;
                a.Play();
            }
        }

        // Audio Thread
        yield return null;

        foreach (var ra in resultAudios)
        {
            foreach (var ran in ra.RAN)
            {
                var a = ran.audioSource;
                var AP = AudioParameters.Dequeue();

                a.Stop();
                a.mute = AP.Item1;
                a.volume = AP.Item2;
            }
        }

        Invoke("AllBGMPlay", WaitStartCount);
    }

    void Start()
    {
        AudioClipSetting();      
    }
}
