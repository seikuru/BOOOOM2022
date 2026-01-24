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
        public float[] MaskValue;
        public RectMask2D mask2D;
        private int MaskIndex = 0;

        public void SetMask()
        {
            if (MaskIndex >= MaskValue.Length - 1) return;
            MaskIndex++;
            var currentPadding = mask2D.padding;
            currentPadding.x = MaskValue[MaskIndex];
            mask2D.padding = currentPadding;
        }
    }

    public void SetScore(float value) => ScoreText.SetText(value.ToString());

    public void SetColorImage(MusicType musictype)
    {
        Debug.Log(musictype);
        foreach (var item in GageImages)
        {
            if(item.musicType == musictype)
            {
                item.SetMask();
                return;
            }
        }
    }
}
