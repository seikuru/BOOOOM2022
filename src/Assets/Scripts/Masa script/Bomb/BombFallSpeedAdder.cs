using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombFallSpeedAdder : MonoBehaviour
{
    /// 爆弾の落下速度を段階的に加速させるクラス
    /// 落下中に時間経過と共に重力を強化し、よりスピードのある体験を演出

    [SerializeField] Rigidbody BombRB; // 爆弾のRigidbody
    [SerializeField] bool FallFlag = true; // 落下速度加算機能の有効/無効フラグ
    [SerializeField,Header("デバッグログ出力フラグ")] bool PrintDebug = false;

    [SerializeField] float FallSpeed = 65.0f;
    [SerializeField,Header("何秒後に加速するか")] float SecondsUntilFall = 2f;//何秒後に加速するか
    [SerializeField] float FallSpeedDiameter = 0.0007f;

    float TimeCounter; // 落下継続時間のカウンター
    Vector3 BeforeVerocity; // 前フレームの速度（落下判定用）

    void Start()
    {
        // 時間カウンターを初期化
        TimeCounter = 0;

        // Rigidbodyコンポーネントがない場合の再取得
        if (BombRB == null && TryGetComponent<Rigidbody>(out var component))
        {
            BombRB = component;
        }
    }

    void FixedUpdate()
    {
        // 機能が無効な場合は処理を終了
        if (BombRB == null || !FallFlag || BombRB.isKinematic == true)
            return;

        else
        {
            // 落下時間カウンターを増加（時間×倍率）
            if (TimeCounter < SecondsUntilFall)
                TimeCounter += Time.fixedDeltaTime;

            else 
            {
                // 追加重力の計算
                float AddAcceleration = TimeCounter;
                
                //下向きのベクトルを値で入力（最小値で制限）
                BombRB.velocity = new Vector3()
                {
                    x = BombRB.velocity.x * 0.99f,
                    y = Mathf.Max(-FallSpeed, BombRB.velocity.y - AddAcceleration),
                    z = BombRB.velocity.z * 0.99f
                };

                // 落下時間カウンターを増加（時間×倍率）
                TimeCounter += (TimeCounter * FallSpeed * FallSpeedDiameter);
            }
        }

        // 次フレーム用に現在の速度を保存
        BeforeVerocity = BombRB.velocity;

        // デバッグ出力
        if (PrintDebug)
            Debug.Log(BombRB.velocity.y);
    }
}
