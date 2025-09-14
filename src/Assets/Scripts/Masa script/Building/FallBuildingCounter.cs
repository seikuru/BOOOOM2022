using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FallBuildingCounter : MonoBehaviour
{
    /// 建物の破壊・落下カウンター管理クラス
    /// 指定されたタグの建物の破壊数を追跡し、達成率に応じてクリア処理を実行

    [SerializeField] Text CounterText; // 破壊数表示用のUIテキスト

    [SerializeField] KillCountGauge killCountGauge; // 破壊数に連動するゲージUI

    [SerializeField] string TagName = "Building"; // 対象建物のタグ名

    [SerializeField] int toLillCountThreshold = 15; // ゲージ色変化の閾値

    [Range(0f, 1f)]
    [SerializeField] float ClearValue; // クリア条件の達成率（0.0～1.0）

    [SerializeField] UnityEvent ClearEvent; // クリア時に実行されるイベント

    HashSet<GameObject> FallObjects; // 破壊された建物のセット（重複防止）

    int MaxObjectValue; // シーン内の対象建物の総数
    bool ClearFlag; // クリア済みフラグ（重複実行防止用）

    /// <summary>
    /// 現在の破壊数を閾値で割った段階レベルを取得
    /// ゲージの段階計算などに使用
    /// </summary>
    /// <returns>段階レベル値</returns>
    public int GetToFallCount() => FallObjects.Count / toLillCountThreshold;

    void Awake()
    {
        // 建物セットを初期化
        FallObjects = new HashSet<GameObject>();      
    }

    void Start()
    {
        // シーン内の対象タグを持つ建物の総数を取得
        MaxObjectValue = GameObject.FindGameObjectsWithTag(TagName).Length;

        UpdateUI(); // 初期UI表示を設定
        ClearFlag = false; // クリアフラグを初期化
    }

    /// <summary>
    /// 破壊された建物をカウンターに追加
    /// HashSetを使用して同じ建物の重複登録を防止
    /// </summary>
    /// <param name="fallObject">破壊された建物オブジェクト</param>
    public void AddFallObject(GameObject fallObject) 
    {
        if (fallObject == null)
            return;

        FallObjects.Add(fallObject); // セットに追加（重複は自動的に無視される）
        UpdateUI(); // UI表示を更新

        // クリア条件の判定
        if (!ClearFlag && (float)((float)FallObjects.Count / MaxObjectValue) >= ClearValue)
        {
            ClearFlag = true; // 重複実行防止フラグを設定
            ClearEvent?.Invoke(); // クリアイベントを実行
        }
    }

    /// <summary>
    /// UI表示の更新処理
    /// カウンターテキストとゲージの表示を更新
    /// </summary>
    void UpdateUI()
    {
        // カウンターテキストの更新（破壊数/総数の形式）
        if (CounterText != null)
            CounterText.text = FallObjects.Count.ToString() + "/" + MaxObjectValue.ToString();

        // ゲージUIの色更新（閾値に基づく段階表示）
        killCountGauge.UpdateColor(FallObjects.Count, toLillCountThreshold);
    }
}
