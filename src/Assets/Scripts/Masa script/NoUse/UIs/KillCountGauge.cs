using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillCountGauge : MonoBehaviour
{
    /// 撃破数に応じて色と進行度が変化するゲージUI管理クラス
    /// 閾値ごとに段階的に色が変化し、各段階内での進行度をバーで表示

    [SerializeField] RectTransform BackGround_Rtransform; // ゲージ背景のRectTransform
    [SerializeField] Image BackGroundImage; // ゲージ背景のImage

    [SerializeField] RectTransform CountGauge_Rtransform; // ゲージバーのRectTransform
    [SerializeField] Image CountGaugeImage; // ゲージバーのImage

    [SerializeField] List<Color> ColorsList; // 段階ごとの色設定リスト

    /// <summary>
    /// 撃破数に応じてゲージの色と進行度を更新
    /// 閾値で段階を区切り、段階ごとに色を変化させる
    /// </summary>
    /// <param name="killCount">現在の撃破数</param>
    /// <param name="threshold">次の段階への必要撃破数（デフォルト10）</param>
    public void UpdateColor(int killCount, int threshold = 10)
    {
        // 不正な閾値の場合は処理終了
        if (threshold <= 0)
            return;

        // 現在の段階を計算（撃破数 ÷ 閾値）
        int colorIndex = Mathf.Min(ColorsList.Count - 1, killCount / threshold);

        // バーの色を現在の段階に対応する色に設定
        CountGaugeImage.color = ColorsList[colorIndex];

        // 背景色の設定（前の段階の色を使用、最初の段階の場合はそのまま）
        if (colorIndex > 0)
            BackGroundImage.color = ColorsList[colorIndex - 1];

        // 最終段階でない場合、現在の段階内での進行度を計算してバーを更新
        if (colorIndex < ColorsList.Count - 1)
            UpdateBar(Mathf.Clamp01((float)(killCount % threshold) / threshold));
    }

    /// <summary>
    /// ゲージバーの長さと位置を進行度に応じて更新
    /// 0.0～1.0の比率でバーの表示範囲を制御
    /// </summary>
    /// <param name="ratio">進行度の比率（0.0～1.0）</param>
    void UpdateBar(float ratio)
    {
        // バーの位置を更新（左端から進行度に応じて右に移動）
        CountGauge_Rtransform.anchoredPosition = new Vector2()
        {
            x = -(BackGround_Rtransform.sizeDelta.x / 2) + (BackGround_Rtransform.sizeDelta.x / 2) * ratio,
            y = CountGauge_Rtransform.anchoredPosition.y,
        };

        // バーの幅を更新（進行度に応じて伸縮）
        CountGauge_Rtransform.sizeDelta = new Vector2()
        {
            x = BackGround_Rtransform.sizeDelta.x * ratio,
            y = CountGauge_Rtransform.sizeDelta.y,
        };
    }
}
