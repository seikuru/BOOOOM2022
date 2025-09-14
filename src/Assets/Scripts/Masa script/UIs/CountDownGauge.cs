using UnityEngine;
using UnityEngine.UI;

public class CountDownGauge : MonoBehaviour
{
    /// カウントダウンタイマーの値に連動するゲージUI管理クラス
    /// カウント値に応じてバーの長さと色を動的に変更する

    [SerializeField] CountDownTimer countDownTimer; // カウント値を取得するタイマー

    [SerializeField] RectTransform BackGround_Rtransform; // ゲージ背景のRectTransform
    [SerializeField] Image BackGroundImage; // ゲージ背景のImage

    [SerializeField] RectTransform CountGauge_Rtransform; // ゲージバーのRectTransform
    [SerializeField] Image CountGaugeImage; // ゲージバーのImage

    [SerializeField] int MaxGaugeValue = 10000; // ゲージの最大値（循環する基準値）

    [SerializeField] BarParameter[] parameters; // カウント値に応じた色設定パラメータ配列

    /// <summary>
    /// ゲージの色変化パラメータを定義するクラス
    /// </summary>
    [System.Serializable]
    class BarParameter
    {
        public int countValue; // この色に変わる閾値
        public Color colorValue; // 適用する色
    }

    Color StartBackGroundColor; // 初期の背景色（リセット用）

    void Start()
    {
        StartBackGroundColor = BackGroundImage.color;// 初期背景色を保存
    }

    /// <summary>
    /// カウント値に応じてゲージの色を更新
    /// parametersの配列を逆順で検索し、適用可能な最初の色を設定
    /// </summary>
    void UpdateColor()
    {
        int currentCount = countDownTimer.GetCountSecond(); // 現在のカウント値を取得
        int parameterIndex = -1; // 適用するパラメータのインデックス

        // 配列を逆順で検索して適用可能な色パラメータを探す
        for (int i = parameters.Length - 1; i >= 0; i--)
        {
            if (parameters[i].countValue <= currentCount)
            {
                parameterIndex = i;
                break;
            }
        }

        // 適用可能なパラメータが見つからない場合は処理終了
        if (parameterIndex == -1)
            return;

        // ゲージの色を更新
        CountGaugeImage.color = parameters[parameterIndex].colorValue;

        // 背景色の更新処理（最高段階の場合は前の段階の色、それ以外は初期色）
        if (parameterIndex == parameters.Length - 1)
            BackGroundImage.color = parameters[parameterIndex - 1].colorValue;
        else
            BackGroundImage.color = StartBackGroundColor;
    }

    /// <summary>
    /// カウント値に応じてゲージバーの長さと位置を更新
    /// MaxGaugeValueを基準に循環的にゲージの進行度を計算
    /// </summary>
    void UpdateGauge()
    {
        int currentCount = countDownTimer.GetCountSecond(); // 現在のカウント値を取得

        // MaxGaugeValue以上の場合は循環処理
        if (currentCount >= MaxGaugeValue)
            currentCount -= MaxGaugeValue;

        // ゲージの進行度を0-1の範囲で計算
        float clamp01 = currentCount >= MaxGaugeValue ? 1f : Mathf.Clamp01((float)(currentCount % MaxGaugeValue) / MaxGaugeValue);

        // ゲージの位置を更新（左端から進行度に応じて右に移動）
        CountGauge_Rtransform.anchoredPosition = new Vector2()
        {
            x = -(BackGround_Rtransform.sizeDelta.x / 2) + (BackGround_Rtransform.sizeDelta.x / 2) * clamp01,
            y = CountGauge_Rtransform.anchoredPosition.y,
        };

        // ゲージの幅を更新（進行度に応じて伸縮）
        CountGauge_Rtransform.sizeDelta = new Vector2()
        {
            x = BackGround_Rtransform.sizeDelta.x * clamp01,
            y = CountGauge_Rtransform.sizeDelta.y,
        };
    }

    void FixedUpdate()
    {
        // null チェックと配列の有効性確認
        if (countDownTimer != null && parameters.Length != 0)
        {
            UpdateColor(); // 色の更新
            UpdateGauge(); // ゲージの更新
        }
    }
}
