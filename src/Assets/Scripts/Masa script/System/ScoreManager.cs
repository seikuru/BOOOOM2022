using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    /// スコア管理を行うクラス
    /// 通常スコア／ボーナススコアの管理・表示更新・MusicTypeごとの取得状況を管理
    
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] BGMControll BGMcontroll;

    [Space, Header("スコア数値")]
    [SerializeField]
    int symbolScore = 3000; // シンボル開放時のスコア
    [SerializeField]
    int enemyScore = 300; // 敵撃破時のスコア
    [SerializeField]
    int coinScore = 50; // コイン取得時のボーナススコア

    // 実際に使用するスコア値（StartでSerializeField値を代入）
    static int Symbol, Enemy, Coin;

    static int ScoreValue;// 通常スコア
    static int BonusValue;// ボーナススコア

    static int ComboCount;// コンボのカウント

    /// <summary>
    /// MusicTypeごとの取得状況管理
    /// タイプ、現在の取得量、最大取得量
    /// </summary>
    static Dictionary<MusicType, (int, int)> TakeMusic;

    // 簡易シングルトン参照
    public static ScoreManager instance;

    // Start is called before the first frame update
    private void Start()
    {
        // インスペクターで設定したスコア値を内部用変数に反映
        Symbol = symbolScore;
        Enemy = enemyScore;
        Coin = coinScore;

        // スコア初期化
        ScoreValue = 0;
        BonusValue = 0;

        ComboCount = 0;

        // 初期スコア表示
        TextSet();

        // MusicTypeごとの取得数と最大値を初期化
        // 最大値はBGMControllから取得
        TakeMusic = new();
        TakeMusic = Enum.GetValues(typeof(MusicType))
        .Cast<MusicType>()
        .ToDictionary( type => type, type => (0, BGMcontroll.GetTypeMaxValue(type)));

        // インスタンス登録
        instance = this;

        // ランキングの保存先を設定
        ScoreRanking.SetCurrentGameMode();
    }

    /// <summary>
    /// スコア表示を更新する
    /// 通常スコア＋ボーナススコアを合算して表示
    /// </summary>
    private void TextSet()
    {
        if (ScoreText == null)
            return;

        ScoreText.SetText((ScoreValue + BonusValue).ToString());
    }

    /// <summary>
    /// 通常スコアを加算
    /// </summary>
    private void AddScore(int _add)
    {
        ScoreValue += _add;
        TextSet();
    }

    /// <summary>
    /// ボーナススコアを加算
    /// </summary>
    private void AddScoreBonus(int _add)
    {
        BonusValue += _add;
        TextSet();
    }

    private void AddCombo()
    {
        int BounsValue = (int)Mathf.Pow(ComboCount++,2) * 7;
        AddScoreBonus(BounsValue);
    }

    /// <summary>
    /// シンボル開放時のスコア加算
    /// </summary>
    public void AddScoreSymbol()
    {
        AddScore(Symbol);
    }

    /// <summary>
    /// 敵撃破時のスコア加算
    /// </summary>
    public void AddScoreEnemy()
    {
        AddScore(Enemy);
        AddCombo();
    }

    /// <summary>
    /// コイン取得時のボーナススコア加算
    /// </summary>
    public void AddScoreCoin()
    {
        AddScoreBonus(Coin);
        AddCombo();
    }

    /// <summary>
    /// 時間に応じたコインボーナス加算
    /// </summary>
    public void AddScoreBonusCoin(int _time)
    {
        AddScoreBonus(_time / 10 + Coin);
    }

    /// <summary>
    /// 指定したMusicTypeの取得数を加算
    /// </summary>
    public void AddMusicType(MusicType type)
    {
        /// ValueTupleは直接変更できないため、一度取り出して再代入
        var v = TakeMusic[type];
        v.Item1++;
        TakeMusic[type] = v;
    }

    /// <summary>
    /// 現在の通常スコアを取得
    /// </summary>
    public static int GetScore() => ScoreValue;

    /// <summary>
    /// 現在のボーナススコアを取得
    /// </summary>
    public static int GetBonus() => BonusValue;

    /// <summary>
    /// 指定したMusicTypeの取得状況を取得
    /// </summary>
    public static void TakeMusicValue(MusicType type ,ref int current,ref int maxValue)
    {
        if (TakeMusic == null || !TakeMusic.ContainsKey(type))
            return;

        current = TakeMusic[type].Item1;
        maxValue = TakeMusic[type].Item2;
    }

    /// <summary>
    /// コンボをリセットする
    /// </summary>
    public static void ComboReset() => ComboCount = 0;
}
