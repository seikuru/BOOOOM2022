using UnityEngine;
using UnityEngine.UI;

public class CountUpTimer : MonoBehaviour
{
    /// カウントアップタイマー管理クラス
    /// 時間を加算し続け、最大時間に達すると自動停止する

    [SerializeField] Text text; // タイマー表示用のUIテキスト
    [SerializeField] int Addcount = 2; // 毎フレーム加算される値

    bool CountFlag; // カウントアップ実行フラグ

    int hours, minutes, seconds; // 時、分、秒の各カウンター

    static readonly int MaxCountSecond = 100; // 秒の最大値（次の分への繰り上げ基準）
    static readonly int MaxCountMinutes = 60; // 分の最大値（次の時への繰り上げ基準）
    static readonly int MaxCountHour = 99; // 時間の最大値（タイマー停止基準）

    /// <summary>
    /// カウントアップを停止
    /// </summary>
    public void CountStop() => CountFlag = false;

    void Start()
    {
        // 初期化処理
        CountFlag = true; // カウントアップ開始
        hours = 0; // 時間を初期化
        minutes = 0; // 分を初期化
        seconds = 0; // 秒を初期化
    }

    void FixedUpdate()
    {
        if(CountFlag)
        {
            // 秒の加算
            seconds += Addcount;

            // 秒の桁上がり処理
            if (MaxCountSecond <= seconds)
            {
                seconds = 0; // 秒をリセット
                minutes++; // 分を加算

                // 分の桁上がり処理
                if (MaxCountMinutes <= minutes)
                {
                    minutes = 0; // 分をリセット
                    hours = Mathf.Min(++hours, MaxCountHour); // 時間を加算（上限制限付き）

                    // 最大時間に達した場合の停止処理
                    if (hours == MaxCountHour)
                        CountStop();
                }
            }

            // UI表示更新
            if (text != null)
            {
                text.text = hours.ToString() + ":" + minutes.ToString() + ":" + seconds.ToString();
            }
        }
    }
}
