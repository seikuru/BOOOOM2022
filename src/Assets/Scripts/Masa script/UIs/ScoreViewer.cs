using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreViewer : MonoBehaviour
{
    /// スコア表示用UIクラス
    /// 現在値/最大値の表示と、マスク・位置を使った進捗表現を行う

    [SerializeField] MusicType musicType;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] RectMask2D rectMask2D;
    [SerializeField] RectTransform KonbRT;
    [SerializeField] float RightPaddingMax = 560;// 進捗100%時の右側のpadding値

    // Start is called before the first frame update
    void Start()
    {
        ScoreReflection();
    }

    /// <summary>
    /// スコア情報を取得し、UIへ反映する
    /// </summary>
    public void ScoreReflection()
    {
        int current = 0,max = 1;

        // 指定MusicTypeに対応したスコア情報を取得
        ScoreManager.TakeMusicValue(musicType, ref current, ref  max);

        // スコアテキスト更新（現在値 / 最大値）
        scoreText.SetText(current.ToString() + "/" + max.ToString());

        // 進捗率を0から1に正規化
        float clamp = Mathf.Clamp01((float)current / max);

        // RectMask2Dの右側Paddingを調整してゲージ表示
        var vector4 = rectMask2D.padding;
        vector4.z = RightPaddingMax - RightPaddingMax * clamp;
        rectMask2D.padding = vector4;

        // 進捗率に応じてアイコン（KonbRT）を右方向へ移動
        KonbRT.position = new(KonbRT.position.x + RightPaddingMax * clamp, KonbRT.position.y); 
    }
}
