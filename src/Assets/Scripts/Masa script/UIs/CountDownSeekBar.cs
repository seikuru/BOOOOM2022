using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CountDownSeekBar : CountDownTimer
{
    [SerializeField] TextMeshProUGUI TMProUGUI; // タイマー表示用のUIテキスト(TMPro)

    [SerializeField] int OutputCountDownSecond = 1000;
    [SerializeField] TextMeshProUGUI CountDownTextUGUI;
    [SerializeField] Transform CountDownTextTransform;
    [SerializeField] float TextTargetScale = 7f;
    [SerializeField] UnityEvent CountDownStart;

    public float GetCurrentTimeClamp() =>Mathf.Clamp01((float)seconds / (float)StartCount);

    /// <summary>
    /// テキスト表示の更新処理
    /// NoCoronTextフラグに応じて時分秒形式か数値のみかを切り替え
    /// </summary>
    protected override void UpdateText()
    {
        if (TMProUGUI != null)
        {
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

        if (CountDownTextUGUI != null && OutputCountDownSecond >= seconds)
        {
            if(OutputCountDownSecond == seconds)
                CountDownStart.Invoke();

            if (seconds % MaxCountSecond == 0 && seconds / MaxCountSecond > 0)
            {
                //Debug.Log(seconds);
                // 負数の場合はマイナス記号を付加して表示
                CountDownTextUGUI.SetText((seconds / MaxCountSecond).ToString());
                StartCoroutine(CountDownTextMove());
            }
        }
    }

    private IEnumerator CountDownTextMove()
    {
        Color TextVertex = CountDownTextUGUI.color;
        
        var _time = 0.0f;
        
        while (_time < 0.1f)
        {
            var scaleRate = Mathf.Min(_time / 0.1f, 1.0f);
            CountDownTextTransform.localScale = Vector3.one * scaleRate * TextTargetScale;

            yield return null;
            _time += Time.fixedDeltaTime;
        }

        _time = 0;
        var color = CountDownTextUGUI.color;

        while (_time < 0.7f)
        {
            var alphaRate = Mathf.Min(_time / 0.7f, 1.0f);
            color.a = 1f - alphaRate;
            CountDownTextUGUI.color = color;
            
            yield return null;
            _time += Time.fixedDeltaTime;
        }

        CountDownTextTransform.localScale = Vector3.zero;
        CountDownTextUGUI.color = TextVertex;
    }
}
