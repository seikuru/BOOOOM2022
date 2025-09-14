using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    /// プレイヤーの回転アクション管理クラス
    /// 身体の即座のY軸回転とモデルのスムーズな回転補間を制御
    /// キューを使用して段階的な回転アニメーションを実現

    [SerializeField] Transform PlayerBodyTransform; // プレイヤー本体のTransform（瞬間回転用）
    [SerializeField] Transform PlayerModelTransform; // プレイヤーモデルのTransform（補間回転用）

    /// <summary>
    /// 回転補間用のデータ構造
    /// Slerpアニメーションの各フレームの情報を格納
    /// </summary>
    struct QuaternionSlape
    {
        public Quaternion player; // 開始時の回転
        public Quaternion Look; // 目標回転
        public float clamp; // 補間率（0.0～1.0）
    }

    Queue<QuaternionSlape> slapesQueue; // 回転補間データのキュー

    static readonly float FullRotation = 360f; // 一周の角度（角度正規化用）

    /// <summary>
    /// プレイヤー本体のY軸回転を瞬間的に設定
    /// 角度の正規化を行い、負の角度や360度以上の値も適切に処理
    /// </summary>
    /// <param name="angle">設定するY軸角度</param>
    public void RotateBody_Y(float angle)
    {
        // 現在のオブジェクトの回転（オイラー角）を取得
        Vector3 localAngle = transform.eulerAngles;

        // Y軸回転を設定。360fを足してからmod 360で負の角度や360度以上を正規化
        // 例: angle = -30 → 330度として扱われる
        localAngle.y = (FullRotation + angle) % FullRotation;

        // オブジェクトの回転に反映
        PlayerBodyTransform.eulerAngles = localAngle;
    }

    /// <summary>
    /// モデルの回転補間アニメーションを予約
    /// 指定されたフレーム数で段階的に目標回転まで補間するキューを作成
    /// </summary>
    /// <param name="lookRotation">目標となる回転</param>
    /// <param name="slapeFlame">補間に使用するフレーム数（デフォルト4フレーム）</param>
    public void RecordingModelRotate(Quaternion lookRotation, float slapeFlame = 4f)
    {
        slapesQueue = new Queue<QuaternionSlape>(); // 既存のキューをクリア

        // 指定フレーム数分の補間データを生成
        for (float i = 0; i <= slapeFlame; i += 1f)
        {
            QuaternionSlape quaternionSlape = new();

            quaternionSlape.player = PlayerModelTransform.transform.rotation; // 現在の回転
            quaternionSlape.Look = lookRotation; // 目標回転
            quaternionSlape.clamp = i / slapeFlame; // 補間率（0→1まで段階的に）
            slapesQueue.Enqueue(quaternionSlape); // キューに追加
        }
    }

    void Start()
    {
        // 回転補間キューを初期化
        slapesQueue = new Queue<QuaternionSlape>();
    }

    void FixedUpdate()
    {
        // キューに補間データが残っている場合
        if (slapesQueue.Count > 0)
        {
            // 次の補間データを取得
            var slape = slapesQueue.Dequeue(); 

            // Slerpを使用してスムーズな回転補間を計算
            var quaternion = Quaternion.Slerp(slape.player, slape.Look, slape.clamp);

            // 計算結果をモデルの回転に適用
            PlayerModelTransform.transform.rotation = quaternion;
        }
    }
}
