using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioGageManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ScoreText;

    [SerializeField] List<Image> AudioGageSprites;

    public void SetScore(float value) => ScoreText.SetText(value.ToString());
}
