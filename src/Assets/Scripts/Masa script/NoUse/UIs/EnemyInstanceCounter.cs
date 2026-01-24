using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyInstanceCounter : MonoBehaviour
{
    /// 敵のスポーン・撃破管理クラス
    /// 敵オブジェクトの追加・削除を監視し、撃破カウントとUI表示を管理する
    /// クリア条件達成時にイベントを発生させる 

    [SerializeField] Text CounterText; // 撃破カウンター表示用のUIテキスト

    [SerializeField] KillCountGauge killCountGauge; // 撃破数に連動するゲージUI

    [SerializeField] int ClearValue = 20; // クリアに必要な撃破数

    [SerializeField] int ToLillCountThreshold = 10; // ゲージ色変化の閾値

    [SerializeField] UnityEvent ClearEvent; // クリア時に実行されるイベント

    List<GameObject> EnemyList; // 生成された敵オブジェクトのリスト

    int MaxObjectValue; // 今までに生成された敵の総数
    int KillCount; // 撃破した敵の数

    bool ClearFlag; // クリア済みフラグ（重複実行防止用）

    /// <summary>
    /// 敵オブジェクトをリストに追加
    /// 生成された敵を監視対象に登録し、UI更新を行う
    /// </summary>
    /// <param name="enemy">追加する敵オブジェクト</param>
    public void AddEnemyObject(GameObject enemy)
    {
        if (enemy == null)
            return;

        EnemyList.Add(enemy); // リストに追加

        MaxObjectValue++; // 総生成数をカウント

        UpdateUI(); // UI表示を更新
    }

    void Awake()
    {
        // 敵リストを初期化
        EnemyList = new List<GameObject>();
    }

    void Start()
    {
        MaxObjectValue = 0; // 総生成数を初期化
        KillCount = 0; // 撃破数を初期化
        ClearFlag = false; // クリアフラグを初期化
        UpdateUI(); // 初期UI表示を設定
    }

    /// <summary>
    /// フレーム終了時に実行される更新処理
    /// 敵オブジェクトの削除監視とクリア判定を行う
    /// </summary>
    void LateUpdate()
    {
        // リストを逆順でチェック（削除処理のため）
        for (int i = EnemyList.Count - 1; i >= 0; i--)
        {
            // nullになったオブジェクト（破壊された敵）を検出
            if (EnemyList[i] == null)
            {
                EnemyList.RemoveAt(i); // リストから削除
                KillCount++; // 撃破カウントを増加
            }
        }

        UpdateUI(); // UI表示を更新

        // クリア条件の判定と処理
        if (!ClearFlag && KillCount >= ClearValue)
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
        // カウンターテキストの更新（撃破数/総生成数の形式）
        if (CounterText != null)
            CounterText.text = KillCount.ToString() + "/" + MaxObjectValue.ToString();

        // ゲージUIの色更新（閾値に基づく）
        if (killCountGauge != null)
            killCountGauge.UpdateColor(KillCount, ToLillCountThreshold);
    }
}
