using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] BGMControll BGMcontroll;

    static int ScoreValue;
    static int BonusValue;

    /// <summary>
    /// タイプ、現在の取得量、最大取得量
    /// </summary>
    static Dictionary<MusicType, (int, int)> TakeMusic;

    public static ScoreManager instance;

    // Start is called before the first frame update
    void Start()
    {
        ScoreValue = 0;
        BonusValue = 0;
        TextSet();

        TakeMusic = new();
        TakeMusic = Enum.GetValues(typeof(MusicType))
        .Cast<MusicType>()
        .ToDictionary( type => type, type => (0, BGMcontroll.GetTypeMaxValue(type)));

        instance = this;
    }

    void TextSet()
    {
        if (ScoreText == null)
            return;

        ScoreText.SetText((ScoreValue + BonusValue).ToString());
    } 

    public void AddScore(int _add)
    {
        ScoreValue += _add;
        TextSet();
    }

    public void AddScoreDestroy()
    {
        AddScore(500);
    }

    public void AddScoreBonus(int _time)
    {
        BonusValue += (_time / 10 + 50);

        Debug.Log(BonusValue);
        TextSet();
    }

    public void AddMusicType(MusicType type)
    {
        var v = TakeMusic[type];
        v.Item1++;
        TakeMusic[type] = v;
    }

    public static int GetScore() => ScoreValue;

    public static int GetBonus() => BonusValue;

    public static void TakeMusicValue(MusicType type ,ref int current,ref int maxValue)
    {
        if (TakeMusic == null || !TakeMusic.ContainsKey(type))
            return;

        current = TakeMusic[type].Item1;
        maxValue = TakeMusic[type].Item2;
    }
}
