using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class AudioGageManager : MonoBehaviour
{
    /// オーディオゲージ表示を管理するクラス
    /// スコア数値の表示とMusicTypeごとのゲージ演出制御
    /// 
    [SerializeField] TextMeshProUGUI ScoreText;

    [SerializeField] List<CollectImageClass> GageImages;// MusicTypeごとのゲージ情報リスト

    [SerializeField] float gageWaitTime = 1.4f;
    [SerializeField] float gageMoveTime = 0.4f;

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
        public bool SetMaskCheck()
        {
            if (MaskIndex >= MaskValue.Length - 1) 
                return false;

            MaskIndex++;

            return true;
        }

        public float GetMaskValue() => MaskValue[MaskIndex];
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
                if(item.SetMaskCheck())              
                    StartCoroutine(MoveGage(item.GetMaskValue(), item.mask2D));
                return;
            }
        }
    }

    /// <summary>
    /// 現在のpaddingを取得し、X方向のみ更新
    /// </summary>
    IEnumerator MoveGage(float TargetValue, RectMask2D rectMask2D)
    {
        yield return new WaitForSeconds(gageWaitTime);

        var currentPadding = rectMask2D.padding;

        float BeforeValue = currentPadding.x;
        float count = 0f;

        while (count < gageMoveTime)
        {
            count += Time.deltaTime;

            float clamp = Mathf.Clamp01(count / gageMoveTime);

            currentPadding.x = BeforeValue + (TargetValue - BeforeValue) * clamp;
            rectMask2D.padding = currentPadding;

            yield return null;
        }

        currentPadding.x = TargetValue;
        rectMask2D.padding = currentPadding;
    }
}

