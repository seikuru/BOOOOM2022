using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCalculation : MonoBehaviour
{
    /// リザルト画面用のスコア計算・演出制御クラス
    /// 最終スコア・ボーナス表示や、クリア演出分岐
    /// ほか背景色、ライン演出、エフェクトの制御を行う

    [SerializeField] Image fadeImage;// フェード用UI
    [SerializeField] float fadeSpeed = 0.1f;// フェード速度
    [SerializeField] TextMeshProUGUI Bouns;// ボーナススコア表示
    [SerializeField] TextMeshProUGUI Total;// 合計スコア表示
    [SerializeField] SpriteRenderer BackSprite;// 背景スプライト
    [SerializeField] MackLine[] mackLines;// 演出用ライン群
    [SerializeField] Animator PlayerAnimator;// プレイヤー演出用Animator
    [SerializeField] GameObject[] NoizeObjects;// 未クリアで表示するノイズ演出
    [SerializeField] GameObject[] KirakiraObjects;// クリアで表示する演出

    /// <summary>
    /// ラインレンダラー演出制御用クラス
    /// clamp値に応じて色を補間する
    /// </summary>
    [Serializable]
    public class MackLine
    {
        public LineRenderer lineRenderer;
        public Color DefaltColor;
        public Color NoizeColor;

        /// <summary>
        /// clamp01(0～1)に応じてラインカラーを変更
        /// </summary>
        public void SetColor(float clamp01)
        {
            Gradient g = new Gradient();

            // ノイズ色 → 通常色へ補間
            Color LerpColor = Color.Lerp(NoizeColor,DefaltColor, clamp01);

            g.SetKeys(
                new GradientColorKey[] 
                {
                    new GradientColorKey(LerpColor, 0f),
                    new GradientColorKey(LerpColor, 1f)
                },
                new GradientAlphaKey[] 
                {
                    new GradientAlphaKey(LerpColor.a, 0f),
                    new GradientAlphaKey(LerpColor.a, 1f)
                }
            );

            // グラデーションと開始・終了色を同時に設定
            lineRenderer.colorGradient = g;
            lineRenderer.startColor = LerpColor;
            lineRenderer.endColor = LerpColor;
        }
    }

    // 判定対象となるMusicType一覧
    MusicType[] types = { MusicType.Drums, MusicType.Bass, MusicType.Melody, MusicType.Chord };

    void Start()
    {
        // スコア取得
        int bonus = ScoreManager.GetBonus();
        int score = ScoreManager.GetScore();

        // スコア表示更新
        Bouns.SetText(bonus.ToString());
        Total.SetText((bonus + score).ToString());

        // 全MusicTypeの取得数合計を算出
        int allCurrent = 0, allmax = 0;
        int current = 0, max = 1;

        foreach (var type in types)
        {
            ScoreManager.TakeMusicValue(type, ref current, ref max);
            allCurrent += current;
            allmax += max;
        }

        // 取得率を0～1に正規化
        float clamp = Mathf.Clamp01((float)allCurrent / allmax);

        // 取得率に応じてクリア／未クリア演出を分岐
        // 未クリア
        if (clamp < 1)
        {
            PlayerAnimator.SetBool("GameClear", false);
            PlayerAnimator.SetBool("ResultOn", true);

            foreach (var noize in NoizeObjects)
            {
                noize.SetActive(true);
            }
        }
        // クリア
        else
        {
            PlayerAnimator.SetBool("GameClear", true);
            PlayerAnimator.SetBool("ResultOn", true);

            foreach (var kirakira in KirakiraObjects)
            {
                kirakira.SetActive(true);
            }
        }

        // 取得率に応じて背景を明るく補正
        BackSprite.color = new Color()
        {
            r = 1f - (1f - clamp) / 2f,
            g = 1f - (1f - clamp) / 2f,
            b = 1f - (1f - clamp) / 2f,
            a = 1f
        };

        // ライン演出を取得率に応じて更新
        foreach (var line in mackLines)
        {
            line.SetColor(clamp);
        }
    }

    /*
    IEnumerator FedeColorChenge()
    {
        var currentColor = fadeImage.color;
        currentColor.a = 1f;

        while (fadeImage.color.a > 0.0f)
        {
            currentColor.a -= Time.fixedDeltaTime * fadeSpeed;
            fadeImage.color = currentColor;
            yield return null;
        }

        Debug.Log("フェード完了");
    }

    
    IEnumerator BGMEnable()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (var type in types)
        {
            int getValue = 0, MaxValue = 1;
            ScoreManager.TakeMusicValue(type, ref getValue, ref MaxValue);

            while (getValue > 0) 
            { 
                getValue--;
                controll.ResultBGMPlay(type);
            }
        }
    }
    */
}
