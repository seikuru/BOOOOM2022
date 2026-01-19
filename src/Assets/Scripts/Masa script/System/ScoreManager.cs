using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] BGMControll BGMcontroll;

    [Space, Header("スコア数値")]
    [SerializeField]
    int symbolScore = 3000;
    [SerializeField]
    int enemyScore = 300;
    [SerializeField]
    int coinScore = 50;

    static int Symbol, Enemy, Coin;

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
        Symbol = symbolScore;
        Enemy = enemyScore;
        Coin = coinScore;

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

    void AddScore(int _add)
    {
        ScoreValue += _add;
        TextSet();
    }

    void AddScoreBonus(int _add)
    {
        BonusValue += _add;
        TextSet();
    }

    public void AddScoreSymbol()
    {
        AddScore(Symbol);
    }

    public void AddScoreEnemy()
    {
        AddScore(Enemy);
    }

    public void AddScoreCoin()
    {
        AddScoreBonus(Coin);
    }

    public void AddScoreBonusCoin(int _time)
    {
        AddScoreBonus(_time / 10 + Coin);
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
