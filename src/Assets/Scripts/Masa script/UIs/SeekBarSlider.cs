using UnityEngine;
using UnityEngine.UI;

public class SeekBarSlider : MonoBehaviour
{
    ///シークバーの移動を管理するクラス

    [SerializeField] RectTransform sliderKnobRT;// シークバーのノブ
    [SerializeField] RectTransform SeekBarBodyRT;// シークバー本体

    [SerializeField] CountDownSeekBar seekBar;

    [SerializeField] RectMask2D SeekBodyMask2D;// シークバー本体のマスク
    [SerializeField] RectMask2D SeekBarMask2D;// スペクトラム表示用マスク

    [SerializeField] float StartPaddingBody = 1900f;// シークバー初期状態のpadding値
    [SerializeField] float StartPaddingSpectrum = 1884f;// スペクトラム初期状態のpadding値

    private void Update()
    {
        // 現在の再生時間を0～1で取得
        float seekBarClamp = seekBar.GetCurrentTimeClamp();

        // 残り時間を基準に表示を更新
        SeekBodyUpdate(1f - seekBarClamp);
        SpectrumMaskUpdate(1f - seekBarClamp);
    }

    /// <summary>
    /// シークバー本体の更新
    /// </summary>
    /// <param name="clampValue">残り時間割合</param>
    public void SeekBodyUpdate(float clampValue)
    {
        // ノブが移動できる最大幅を算出
        float KnobWidth = (SeekBarBodyRT.rect.width / 2 - sliderKnobRT.rect.width / 2);

        // clampValue(0～1)に応じてノブの位置を更新
        sliderKnobRT.position = 
            new Vector3(
            SeekBarBodyRT.position.x + (KnobWidth * clampValue * 2 - KnobWidth),
            sliderKnobRT.position.y,
            sliderKnobRT.position.z);

        // マスクのpaddingを変更してバー表示を更新
        var currentPadding = SeekBodyMask2D.padding;

        currentPadding.z = StartPaddingBody - (clampValue * StartPaddingBody);

        SeekBodyMask2D.padding = currentPadding;
    }

    /// <summary>
    /// スペクトラム表示用マスクの更新
    /// clampValueに応じて表示範囲を制御
    /// </summary>
    /// <param name="clampValue">残り時間割合</param>
    public void SpectrumMaskUpdate(float clampValue)
    {
        var currentPadding = SeekBarMask2D.padding;

        currentPadding.z = StartPaddingSpectrum - (clampValue * StartPaddingSpectrum);

        SeekBarMask2D.padding = currentPadding;
    }
}
