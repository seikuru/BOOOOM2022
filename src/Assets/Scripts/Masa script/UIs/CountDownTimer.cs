using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    /// カウントダウンタイマー管理クラス
    /// 時間の減算、加算、UI表示を行い、コンボシステムと連動する
   
    [SerializeField] Text text; // タイマー表示用のUIテキスト

    [SerializeField] int Subtractcount = 2; // 毎フレーム減算される値

    [SerializeField] protected int StartCount = 6000; // 開始時のカウント値

    [SerializeField] int BaseComboValue = 150; // コンボ時の基本加算値

    [SerializeField] int AddComboValue = 50; // コンボ数に応じた追加加算値

    [SerializeField] protected bool NoCoronText = false; // コロン区切り表示の有効/無効フラグ

    [SerializeField] UnityEvent TimeUpEvent;

    bool CountFlag; // カウントダウン実行フラグ
    protected int seconds; // 現在の秒数（内部カウンター）

    protected static readonly int MaxCountSecond = 100; // 秒の最大値（時間計算用）
    protected static readonly int MaxCountMinutes = 60; // 分の最大値（時間計算用）

    /// <summary>
    /// カウントダウンを停止
    /// </summary>
    public void CountStop() => CountFlag = false;

    /// <summary>
    /// 現在の秒数を取得
    /// </summary>
    /// <returns>現在のカウント値</returns>
    public int GetCountSecond() => seconds;

    /// <summary>
    /// コンボ数に応じてカウントを加算
    /// 基本値 + (コンボ数 × 追加値) の計算式を使用
    /// </summary>
    /// <param name="combo">現在のコンボ数</param>
    public void AddCountWithCombo(int combo)
    {
        //Debug.Log(combo);
        seconds += BaseComboValue + combo * AddComboValue;// コンボボーナス計算
    }

    /// <summary>
    /// 指定された値をカウントに加算
    /// </summary>
    /// <param name="count">加算する値</param>
    public void AddCountSecond(int count)
    {
        seconds += count;
    }

    void Start()
    {
        // 初期化処理
        CountFlag = true; // カウントダウン開始
        seconds = StartCount;// 初期値を設定
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // カウントダウン処理
        if (CountFlag)
        {
            seconds -= Subtractcount;// 設定値分減算

            if (seconds <= 0)
            {
                TimeUpEvent.Invoke();
                CountFlag = false;
            }
               
        }
               
        // UI表示更新
        UpdateText();
    }

    /// <summary>
    /// テキスト表示の更新処理
    /// NoCoronTextフラグに応じて時分秒形式か数値のみかを切り替え
    /// </summary>
    protected virtual void UpdateText()
    {
        if (text == null)
            return;

        // コロン区切り表示（時:分:秒形式）
        if (!NoCoronText)
        {
            // 時間計算（負数対応のため絶対値を使用）
            string h = Mathf.Abs(seconds / (MaxCountSecond * MaxCountMinutes)).ToString(); // 時間部分
            string m = Mathf.Abs(seconds % (MaxCountSecond * MaxCountMinutes) / MaxCountSecond).ToString(); // 分部分
            // string s = Mathf.Abs(seconds % MaxCountSecond).ToString(); // 秒部分

            if(m.Length == 1)
            {
                m = "0" + m;
            }

            // 負数の場合はマイナス記号を付加して表示
            text.text = (seconds < 0 ? "-" : "") + h + ":" + m;// + ":" + s;
        }
        else
        {
            // 数値のみ表示
            text.text = seconds.ToString();
        }
    }
}
