using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioGageManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ScoreText;

    [SerializeField] List<CollectImageClass> GageImages;

    [System.Serializable]
    class CollectImageClass
    {
        public MusicType musicType;
        public Image[] CollectImages;
        public Color SetColor;
    }

    public void SetScore(float value) => ScoreText.SetText(value.ToString());

    public void SetColorImage(MusicType musictype)
    {
        foreach (var item in GageImages)
        {
            if(item.musicType == musictype)
            {
                foreach (var image in item.CollectImages)
                {
                    if(image.color != item.SetColor)
                    {
                        image.color = item.SetColor;
                        return;
                    }  
                }
            }
        }
    }
}
