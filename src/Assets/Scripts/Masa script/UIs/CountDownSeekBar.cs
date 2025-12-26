using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountDownSeekBar : CountDownTimer
{
    [SerializeField] TextMeshProUGUI TMProUGUI; // タイマー表示用のUIテキスト(TMPro)

    public float GetCurrentTimeClamp() =>Mathf.Clamp01((float)seconds / (float)StartCount);

    /// <summary>
    /// テキスト表示の更新処理
    /// NoCoronTextフラグに応じて時分秒形式か数値のみかを切り替え
    /// </summary>
    protected override void UpdateText()
    {
        if (TMProUGUI == null)
            return;

        // コロン区切り表示（時:分:秒形式）
        if (!NoCoronText)
        {
            // 時間計算（負数対応のため絶対値を使用）
            string h = Mathf.Abs(seconds / (MaxCountSecond * MaxCountMinutes)).ToString(); // 時間部分
            string m = Mathf.Abs(seconds % (MaxCountSecond * MaxCountMinutes) / MaxCountSecond).ToString(); // 分部分
            // string s = Mathf.Abs(seconds % MaxCountSecond).ToString(); // 秒部分

            if (m.Length == 1)
            {
                m = "0" + m;
            }

            // 負数の場合はマイナス記号を付加して表示
            TMProUGUI.SetText((seconds < 0 ? "-" : "") + h + ":" + m);// + ":" + s;
        }
        else
        {
            // 数値のみ表示
            TMProUGUI.SetText(seconds.ToString());
        }
    }
}
