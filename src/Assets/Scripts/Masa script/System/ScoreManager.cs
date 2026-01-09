using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] BGMControll BGMcontroll;

    static int ScoreValue;

    /// <summary>
    /// タイプ、現在の取得量、最大取得量
    /// </summary>
    static Dictionary<MusicType, (int, int)> TakeMusic;

    // Start is called before the first frame update
    void Start()
    {
        ScoreValue = 0;
        ScoreText?.SetText(ScoreValue.ToString());

        TakeMusic = new();
        TakeMusic = Enum.GetValues(typeof(MusicType))
        .Cast<MusicType>()
        .ToDictionary( type => type, type => (0, BGMcontroll.GetTypeMaxValue(type)));
    }

    public void AddScore(int _add)
    {
        ScoreValue += _add;
        ScoreText?.SetText(ScoreValue.ToString());
    }

    public void AddScoreDestroy()
    {
        AddScore(500);
    }

    public void AddScoreBonus(int _time)
    {
        AddScore(_time / 10 + 50);
    }

    public void AddMusicType(MusicType type)
    {
        var v = TakeMusic[type];
        v.Item1++;
        TakeMusic[type] = v;
    }

    public static int GetScore() => ScoreValue;

    public static void TakeMusicValue(MusicType type ,ref int current,ref int maxValue)
    {
        if (TakeMusic == null || !TakeMusic.ContainsKey(type))
            return;

        current = TakeMusic[type].Item1;
        maxValue = TakeMusic[type].Item2;
    }
}
