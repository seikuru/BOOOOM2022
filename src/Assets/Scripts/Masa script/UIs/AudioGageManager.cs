using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioGageManager : MonoBehaviour
{
    /// オーディオゲージ表示を管理するクラス
    /// スコア数値の表示とMusicTypeごとのゲージ演出制御
    /// 
    [SerializeField] TextMeshProUGUI ScoreText;

    [SerializeField] List<CollectImageClass> GageImages;// MusicTypeごとのゲージ情報リスト

    /// <summary>
    /// ゲージ1本分の情報をまとめたクラス
    /// RectMask2Dのpaddingを段階的に変更してゲージ進行を表現する
    /// </summary>
    [System.Serializable]
    class CollectImageClass
    {
        public MusicType musicType;
        public float[] MaskValue;
        public RectMask2D mask2D;

        // 現在適用されているMaskValueのインデックス
        private int MaskIndex = 0;

        /// <summary>
        /// ゲージを1段階進める
        /// MaskValueの配列範囲を超えないように制御
        /// </summary>
        public void SetMask()
        {
            if (MaskIndex >= MaskValue.Length - 1) return;
            MaskIndex++;

            // 現在のpaddingを取得し、X方向のみ更新
            var currentPadding = mask2D.padding;
            currentPadding.x = MaskValue[MaskIndex];
            mask2D.padding = currentPadding;
        }
    }

    /// <summary>
    /// スコア表示を更新(デバッグ用)
    /// </summary>
    public void SetScore(float value) => ScoreText.SetText(value.ToString());

    /// <summary>
    /// 指定したMusicTypeに対応するゲージを進行させる
    /// </summary>
    public void SetColorImage(MusicType musictype)
    {
        // Debug.Log(musictype);
        // 対応するMusicTypeのゲージのみを更新
        foreach (var item in GageImages)
        {
            if(item.musicType == musictype)
            {
                item.SetMask();
                return;
            }
        }
    }
}
